using SimpleIPCCommSystem.Messages;

namespace SimpleIPCCommSystem {

    public enum IPCDispatchResult {
        Fail,
        Success,
        Timeout
    }

    public interface IIPCBaseIPCDispatcher {
        IPCDispatchResult Dispatch(IIPCBaseMessage message);

        IIPCGUID Receaver { get; }
    }
}
