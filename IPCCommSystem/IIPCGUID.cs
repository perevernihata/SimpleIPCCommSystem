using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SimpleIPCCommSystem {

    [Serializable()]
    public class IIPCGUID {

        public static IIPCGUID Uncpecifyed = new IIPCGUID("unspecifyed");

        private string value;
        private readonly string prefix = "IPCWinCommSystem";

        public string Value {
            get {
                return value;
            }
        }

        public IIPCGUID() {
            value = prefix + Process.GetCurrentProcess().Id;
        }

        public IIPCGUID(string guid) {
            value = prefix + guid;
        }

        public IIPCGUID(int guid) {
            value = prefix + guid.ToString();
        }
    }
}
