using System;
using SimpleIPCCommSystem;

namespace SharedMessages {
    /// <summary>
    /// Test async message used to show how this IPCCommSystem.dll can handle
    /// async IPC messaging
    /// </summary>
    [Serializable()]
    public class TestSyncMessage : BaseIPCMessage {
        public string StrIn { get; set; }
        public string StrOut { get; set; }
        public TestSyncMessage(string strIn)
            : base(IPCDispatchType.Sync) {
            StrIn = strIn;
        }
    }
}
