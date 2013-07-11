namespace SimpleIPCCommSystem.Messages {

    public enum IPCDispatchType {
        Async,
        Sync
    }

    public interface IIPCBaseMessage {
        IIPCGUID SenderID { get; set; }
        IIPCGUID DispatherID { get; set; }
        IPCDispatchType MessageType { get; }
    }
}
