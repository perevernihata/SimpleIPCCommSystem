using SimpleIPCCommSystem.Messages;

namespace SimpleIPCCommSystem {

    public enum IPCDispatchResult {
        Fail,
        Success,
        Timeout
    }

    public interface IIPCDispatcher {
        IPCDispatchResult Dispatch(IIPCBaseMessage message);

        IIPCGUID Receaver { get; }
    }
}
