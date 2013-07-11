using System;
using System.Linq;
using System.Threading;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem.Utilities;
using SimpleIPCCommSystem.GUIDS;

namespace SimpleIPCCommSystem.Dispatchers {
    public class BaseIPCDispatcher : IIPCDispatcher, IDisposable {
        IIPCGUID _receaverID;
        IIPCGUID _dispatcherID;
        private EventWaitHandle _receaverWaitHandle;
        private EventWaitHandle _dispatcherWaitHandle;
        private IPCBaseMessagesQueue _receaverQueue;

        private static IpcClientChannel clientChannel = new IpcClientChannel();
        private static IpcServerChannel serverChannel;

        public BaseIPCDispatcher(IIPCGUID receaverID) {
            // create receaver wait handle
            _receaverID = receaverID;
            _receaverWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset,
                _receaverID.Value);

            // create dispatcher wait handle
            _dispatcherID = new IPCDispatcherGUID();
            _dispatcherWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset,
                _dispatcherID.Value);
            if (!ChannelServices.RegisteredChannels.Any(i => i == clientChannel)) {
                ChannelServices.RegisterChannel(clientChannel, true);
            }
        }

        public IPCDispatchResult Dispatch(IIPCBaseMessage message) {

            string receaverUri = new IPCUri(_receaverID, IPCBaseMessagesQueue.URISuffix).Value;

            _receaverQueue = (IPCBaseMessagesQueue)Activator.GetObject(typeof(IPCBaseMessagesQueue),
                receaverUri);

            try {
                if (RemotingServices.IsTransparentProxy(_receaverQueue)) {
                    switch (message.MessageType) {
                        case IPCDispatchType.Async:
                            return DoDispatchAsyncMessage(message as IPCBaseAsyncMessage, _receaverQueue);
                        case IPCDispatchType.Sync:
                            return DoDispatchSyncMessage(message as IPCBaseSyncMessage, _receaverQueue);
                        default:
                            break;
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

        protected IPCDispatchResult DoDispatchAsyncMessage(IPCBaseAsyncMessage message, IPCBaseMessagesQueue receaverQueue) {
            if (message == null || receaverQueue == null) {
                return IPCDispatchResult.Fail;
            }
            _receaverQueue.EnqueueMessage(message);
            _receaverWaitHandle.Set();
            return IPCDispatchResult.Success;
        }

        protected IPCDispatchResult DoDispatchSyncMessage(IPCBaseSyncMessage message, IPCBaseMessagesQueue receaverQueue) {
            if (message == null || receaverQueue == null) {
                return IPCDispatchResult.Fail;
            }

            // ensure that IPC server is alive
            if (!ChannelServices.RegisteredChannels.Any(i => i.ChannelName == _dispatcherID.Value)) {
                serverChannel = new IpcServerChannel(_dispatcherID.Value, _dispatcherID.Value);
                if (!ChannelServices.RegisteredChannels.Any(i => i == serverChannel)) {
                    ChannelServices.RegisterChannel(serverChannel, false);
                }
            }

            // share object
            RemotingServices.Marshal(message,
                message.UriSuffix,
                message.GetType());
            try {
                // notify receaver
                IIPCGUID helperID = new IPCGUID(message.GetHashCode());
                IPCSyncHelperMessage helperMessage = new IPCSyncHelperMessage(new IPCUri(_dispatcherID, message).Value, message.GetType(), helperID);
                helperMessage.DispatherID = _dispatcherID;
                message.DispatherID = _dispatcherID;
                receaverQueue.EnqueueMessage(helperMessage);
                _receaverWaitHandle.Set();
                if (!_dispatcherWaitHandle.WaitOne(message.TimeOut))
                    return IPCDispatchResult.Timeout;
            } finally {
                RemotingServices.Disconnect(message);
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
