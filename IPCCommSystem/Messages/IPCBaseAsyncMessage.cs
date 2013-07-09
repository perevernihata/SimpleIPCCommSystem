using System;

namespace SimpleIPCCommSystem.Messages {

    [Serializable]
    public class IPCBaseAsyncMessage : IIPCBaseMessage {

        public IPCBaseAsyncMessage(IIPCGUID senderID) {
            _senderID = senderID;
        }

        private IIPCGUID _senderID;
        public IIPCGUID SenderID {
            get { return _senderID; }
            set { _senderID = value; }
        }

        public IPCDispatchType MessageType {
            get { return IPCDispatchType.Async; }
        }
    }
}
