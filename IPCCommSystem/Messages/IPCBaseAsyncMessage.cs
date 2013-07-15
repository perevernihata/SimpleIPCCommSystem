using System;
using System.Linq;
using System.Reflection;

namespace SimpleIPCCommSystem.Messages {

    [Serializable]
    public class IPCBaseAsyncMessage : IIPCMessage {

        public IPCBaseAsyncMessage(IIPCGUID senderID) {
            _senderID = senderID;            
            _isValid = DoValidateMessage();
        }

        protected bool DoValidateMessage() {
            PropertyInfo[] propertyInfos = this.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic // Get public and non-public
                | BindingFlags.Static | BindingFlags.Instance  // Get instance + static        
                | BindingFlags.FlattenHierarchy); // Search up the hierarchy
            // check if all properties is serializable (ignore interfaced properties)
            return !propertyInfos.Any(p => !p.PropertyType.IsSerializable && !p.PropertyType.IsInterface);
        }

        private IIPCGUID _senderID;
        public IIPCGUID SenderID {
            get { return _senderID; }
            set { _senderID = value; }
        }

        public IPCDispatchType MessageType {
            get { return IPCDispatchType.Async; }
        }


        private IIPCGUID _dispatherID;
        public IIPCGUID DispatherID {
            get {
                return _dispatherID;
            }
            set {
                _dispatherID = value;
            }
        }

        private bool _isValid;
        public bool IsValid {
            get {
                return _isValid;
            }
        }
    }
}
