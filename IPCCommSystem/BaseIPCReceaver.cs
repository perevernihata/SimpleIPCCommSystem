using System;
using SimpleIPCCommSystem;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;

namespace SimpleIPCCommSystem {
    public class BaseIPCReceaver : IIPCBaseReceaver, IDisposable {
        private IIPCGUID _ownGUID;
        private Thread _worker;
        private object _locker = new object();
        private EventWaitHandle _currentWaitHandle;
        private IPCBaseMessagesQueue _currentQueue;
        private IpcServerChannel channel;

        public BaseIPCReceaver() {
            _ownGUID = new IIPCGUID(Process.GetCurrentProcess().Id);
            _currentWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset,
                 _ownGUID.Value);
            _currentQueue = DoCreateQueue();

            channel = new IpcServerChannel(_ownGUID.Value);
            ChannelServices.RegisterChannel(channel, true);
            ObjRef QueueRef = RemotingServices.Marshal(_currentQueue,
                IPCBaseMessagesQueue.URISuffix, 
                typeof(IPCBaseMessagesQueue));
            QueueRef.URI = String.Format("ipc://{0}/{1}", _ownGUID.Value, IPCBaseMessagesQueue.URISuffix);
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
