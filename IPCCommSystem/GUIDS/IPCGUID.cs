using System;
using System.Diagnostics;

namespace SimpleIPCCommSystem.GUIDS {

    [Serializable()]
    public class IPCGUID : IIPCGUID {

        public static IPCGUID Uncpecifyed = new IPCGUID("unspecifyed");
        
        protected string value;

        protected virtual string GetPrefix()
        {
            return "IPCSimpleCommSystem";
        }

        public string Value {
            get {
                return value;
            }
        }

        public IPCGUID() {
            value = GetPrefix() + Process.GetCurrentProcess().Id;
        }

        public IPCGUID(string guid) {
            value = GetPrefix() + guid;
        }

        public IPCGUID(int guid) {
            value = GetPrefix() + guid.ToString();
        }

        public bool Equals(IIPCGUID other) {
            return String.Equals(other.Value, Value);
        }
    }
}
