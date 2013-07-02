using System;

namespace SimpleIPCCommSystem {
    [Serializable()]
    public class BaseIPCMessage : IIPCBaseMessage {

        public BaseIPCMessage(IIPCGUID senderID,
            IPCDispatchType MessageType = IPCDispatchType.Async) {
            _senderID = senderID;
            _messageType = MessageType;
        }

        public BaseIPCMessage(IPCDispatchType MessageType = IPCDispatchType.Async) {
            _senderID = IIPCGUID.Uncpecifyed;
            _messageType = MessageType;
        }

        private IIPCGUID _senderID;
        public IIPCGUID SenderID {
            get { return _senderID; }
            set { _senderID = value; }
        }

        public IPCDispatchType _messageType = IPCDispatchType.Async;
        public IPCDispatchType MessageType {
            get { return _messageType; }
        }
    }
}
