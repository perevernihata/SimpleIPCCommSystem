using SimpleIPCCommSystem.Utilities;

namespace SimpleIPCCommSystem.Messages {

    public enum IPCDispatchType {
        Async,
        Sync
    }

    public interface IIPCBaseMessage {
        IIPCGUID SenderID { get; set; }
        IPCDispatchType MessageType { get; }
    }
}
