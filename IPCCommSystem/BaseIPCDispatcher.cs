using System;
using System.Linq;
using System.Threading;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace SimpleIPCCommSystem {
    public class BaseIPCDispatcher : IIPCBaseIPCDispatcher, IDisposable {
        IIPCGUID _receaverID;
        IIPCGUID _dispatcherID;
        private EventWaitHandle _receaverWaitHandle;
        private EventWaitHandle _dispatcherWaitHandle;
        private IPCBaseMessagesQueue _receaverQueue;

        private static IpcClientChannel channel = new IpcClientChannel();

        public BaseIPCDispatcher(IIPCGUID receaverID) {
            // create receaver wait handle
            _receaverID = receaverID;
            _receaverWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset,
                _receaverID.Value);
            
            // create dispatcher wait handle
            _dispatcherID = new IIPCGUID();
            _dispatcherWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset,
                _dispatcherID.Value);

            if (!ChannelServices.RegisteredChannels.Any(i => i == channel)) {
                ChannelServices.RegisterChannel(channel, true);
            }
        }

        public IPCDispatchResult Dispatch(IIPCBaseMessage message,
            int timeout = 0) {

            string receaverUri = 
                String.Format("ipc://{0}/{1}",_receaverID.Value, IPCBaseMessagesQueue.URISuffix);
            Console.WriteLine("Dispatch uri = " + receaverUri);

            _receaverQueue = (IPCBaseMessagesQueue)Activator.GetObject(typeof(IPCBaseMessagesQueue),
                receaverUri);
            message.SenderID = _dispatcherID;
            try {
                if (RemotingServices.IsTransparentProxy(_receaverQueue)) {
                    Console.WriteLine(_receaverQueue.Count());
                    _receaverQueue.EnqueueMessage(message);
                    _receaverWaitHandle.Set();
                    // TODO: implement timeout
                    if (message.MessageType == IPCDispatchType.Sync) {
                        _dispatcherWaitHandle.WaitOne();
                    }
                }
            } catch (Exception ex) {               
                if (ex is RemotingException || ex is SerializationException) {
                    return IPCDispatchResult.Fail;
                }
                throw;
            }

            return IPCDispatchResult.Success;
        }

        public IIPCGUID Receaver {
            get { return _receaverID; }
        }

        public void Dispose() {
            _receaverWaitHandle.Dispose();
            _dispatcherWaitHandle.Dispose();
        }
    }
}
