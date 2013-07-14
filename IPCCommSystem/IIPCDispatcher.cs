using SimpleIPCCommSystem.Messages;

namespace SimpleIPCCommSystem {

    /// <summary>
    /// Used to obtain dispatching result. Theoretically some other results can be implemented:
    /// ReceaverNotExists
    /// InvalidMessage
    /// etc.
    /// </summary>
    public enum IPCDispatchResult {
        UnexpectedFail,
        Success,
        Timeout
    }

    public interface IIPCDispatcher {
        IPCDispatchResult Dispatch(IIPCBaseMessage message);

        IIPCGUID Receaver { get; }
    }
}
