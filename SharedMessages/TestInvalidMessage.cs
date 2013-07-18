using System;
using SimpleIPCCommSystem.Messages;
using System.Xml;
using SimpleIPCCommSystem;

namespace SharedMessages {
    [Serializable]
    public class TestInvalidMessage : IPCBaseAsyncMessage {

        // [NonSerialized] 
        private XmlDocument _invalidProp = new XmlDocument();

        public XmlDocument InvalidProp {
            get { return _invalidProp; }
            set { _invalidProp = value; }
        }
        

        public XmlDocument InvalidProperty { get; set; }

        public TestInvalidMessage(IIPCGUID senderID):base(senderID) {
        }
    }
}
