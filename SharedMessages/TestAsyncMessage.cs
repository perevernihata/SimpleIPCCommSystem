using System;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem;

namespace SharedMessages {

    /// <summary>
    /// Test async message used to show how this IPCCommSystem.dll can handle
    /// async IPC messaging
    /// </summary>
    [Serializable]
    public class TestAsyncMessage : IPCBaseAsyncMessage {
        public string StrData { get; set; }

        public TestAsyncMessage(IIPCGUID senderID): base(senderID) {
        }
    }
}
