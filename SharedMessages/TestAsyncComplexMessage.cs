using System;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem;

namespace SharedMessages {

    [Serializable]
    public class TestAsyncComplexMessage : IPCBaseAsyncMessage {

        private ComplexSharedClass _additionalnfo;
        public ComplexSharedClass AdditionalInfo {
            get { return _additionalnfo; }
            set { _additionalnfo = value; }
        }
        
        public TestAsyncComplexMessage(IIPCGUID senderID, ComplexSharedClass additionalInfo)
            : base(senderID) {
                _additionalnfo = additionalInfo;
        }
    }
}
