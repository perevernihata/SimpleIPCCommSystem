using System;
using System.Diagnostics;

namespace SimpleIPCCommSystem.Utilities {

    [Serializable()]
    public class IPCGUID : IIPCGUID {

        public static IPCGUID Uncpecifyed = new IPCGUID("unspecifyed");
        
        private string value;
        private readonly string prefix = "IPCSimpleCommSystem";

        public string Value {
            get {
                return value;
            }
        }

        public IPCGUID() {
            value = prefix + Process.GetCurrentProcess().Id;
        }

        public IPCGUID(string guid) {
            value = prefix + guid;
        }

        public IPCGUID(int guid) {
            value = prefix + guid.ToString();
        }
    }
}
