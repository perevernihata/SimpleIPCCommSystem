using System;
using SimpleIPCCommSystem;

namespace SharedMessages {

    /// <summary>
    /// Test async message used to show how this IPCCommSystem.dll can handle
    /// async IPC messaging
    /// </summary>
    [Serializable()]   
    public class TestAsyncMessage : BaseIPCMessage {
        public string StrData { get; set; }
        public TestAsyncMessage(string strData)
            : base(IPCDispatchType.Async) {
            StrData = strData;
        }
    }
}
