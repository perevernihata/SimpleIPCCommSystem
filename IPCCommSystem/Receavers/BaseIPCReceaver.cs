using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using SimpleIPCCommSystem.Resources;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem.Utilities;
using SimpleIPCCommSystem.GUIDS;

namespace SimpleIPCCommSystem.Receavers {
    public class BaseIPCReceaver : IIPCBaseReceaver, IDisposable {
        private IIPCGUID _ownGUID;
        private Thread _worker;
        private object _locker = new object();
        private EventWaitHandle _currentWaitHandle;
        private IPCBaseMessagesQueue _currentQueue;
        private IpcServerChannel channel;

        public BaseIPCReceaver() {
            _ownGUID = new IPCReceaverGUID();
            _currentWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset,
                 _ownGUID.Value);
            _currentQueue = DoCreateQueue();

            // do not use security
            // ensure that IPC server is alive
            if (!ChannelServices.RegisteredChannels.Any(i => i.ChannelName == _ownGUID.Value)) {
                channel = new IpcServerChannel(_ownGUID.Value, _ownGUID.Value);
                if (!ChannelServices.RegisteredChannels.Any(i => i == channel)) {
                    ChannelServices.RegisterChannel(channel, false);
                }
            }

            ObjRef QueueRef = RemotingServices.Marshal(_currentQueue,
                _currentQueue.UriSuffix,
                typeof(IPCBaseMessagesQueue));
            QueueRef.URI = new IPCUri(_ownGUID, _currentQueue).Value; // TODO: get rid of this code?
            _worker = new Thread(ListenQueue);
            _worker.Start();
        }

        private void ListenQueue() {
            while (true) {
                IIPCBaseMessage message = null;
                    lock (_locker) {
                        if (_currentQueue.Count() > 0) {
                            message = _currentQueue.DequeueMessage();
                        }
                    }
                    if (message != null) {
                        if (message.MessageType == IPCDispatchType.Sync) {
                            using (EventWaitHandle _receaverWaitHandle =
                                new EventWaitHandle(false, EventResetMode.AutoReset, message.SenderID.Value)) {
                                OnReceaveIPCMessage(this, message);
                                _receaverWaitHandle.Set();
                            };
                        } else {
                            OnReceaveIPCMessage(this, message);
                        }
                    } else
                        _currentWaitHandle.WaitOne();
            }
        }

        protected virtual IIPCGUID DoGetReceaverID() {
            return _ownGUID;
        }

        protected virtual IPCBaseMessagesQueue DoCreateQueue() {
            return new IPCBaseMessagesQueue();
        }

        public IIPCGUID ReceaverID {
            get { return DoGetReceaverID(); }
        }


        public void Dispose() {
            _worker.Join();
            _currentWaitHandle.Close();
        }

        public event ReceaveIPCMessageEventHandler OnReceaveIPCMessage;
    }
}
