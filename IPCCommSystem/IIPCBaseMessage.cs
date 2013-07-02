
namespace SimpleIPCCommSystem {

    public enum IPCDispatchType {
        Async,
        Sync
    }

    public interface IIPCBaseMessage {
        IIPCGUID SenderID { get; set; }
        IPCDispatchType MessageType { get; }
    }
}
