using SimpleIPCCommSystem.Messages;

namespace SimpleIPCCommSystem {

    /// <summary>
    /// Used to obtain dispatching result.   
    /// </summary>
    public enum IPCDispatchResult {
        /// <summary>
        /// Message was sent successfully
        /// </summary>
        Success,

        /// <summary>
        /// This type of message can't be send via communication system
        /// </summary>
        InvalidMessageClass,

        /// <summary>
        /// Receaver can be found
        /// </summary>
        ReceaverNotExists,

        /// <summary>
        /// Can emerge only if message if synchronous. Show that receaver did not perform message in time
        /// </summary>
        Timeout,

        /// <summary>
        /// Message was not sent, and the reason is unknown
        /// </summary>
        UnexpectedFail
    }
    /// <summary>
    /// Used to pass message via communication system (but only to one receaver)
    /// </summary>
    public interface IIPCDispatcher {

        /// <summary>
        /// Dispatch message to receceaver
        /// </summary>
        /// <param name="message">Message that should be dispatched</param>
        /// <returns></returns>
        IPCDispatchResult Dispatch(IIPCMessage message);
        
        /// <summary>
        /// Gets appropriate receiver ID for current dispatcher
        /// </summary>
        IIPCGUID Receaver { get; }
    }
}
