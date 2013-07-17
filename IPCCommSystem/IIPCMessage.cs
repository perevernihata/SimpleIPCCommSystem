namespace SimpleIPCCommSystem.Messages {

    /// <summary>
    /// Used to specify method of dispathing for any message
    /// </summary>
    public enum IPCDispatchType {
        /// <summary>
        /// After message id send stack will return immediately
        /// </summary>
        Async,
        /// <summary>
        /// Dispather will freeze, until receaver release message, or time is out
        /// </summary>
        Sync
    }

    public interface IIPCMessage {
        /// <summary>
        /// Unique ID of receaver, which is able to receave responce
        /// </summary>
        IIPCGUID SenderID { get; set; }

        /// <summary>
        /// Unique ID for dispathcer object that had been used to sent message
        /// </summary>
        IIPCGUID DispatherID { get; set; }

        /// <summary>
        /// Get dispatch type for current message
        /// </summary>
        IPCDispatchType MessageType { get; }

        /// <summary>
        /// Show if message can be send via communication system
        /// </summary>
        bool IsValid { get; }
    }
}
