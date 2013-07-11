using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem;

namespace SharedMessages {
    /// <summary>
    /// Test async message used to show how this IPCCommSystem.dll can handle
    /// async IPC messaging
    /// </summary>
    public class TestSyncMessage : IPCBaseSyncMessage {
        public string StrIn { get; set; }
        public string StrOut { get; set; }

        public TestSyncMessage(IIPCGUID senderID,
            int timeout = IPCBaseSyncMessage.InfiniteTimeout)
            : base(senderID, timeout) {
        }

    }
}
