using System;

namespace SimpleIPCCommSystem.GUIDS {
    [Serializable]
    public class IPCReceaverGUID : IPCGUID {

        public IPCReceaverGUID()
            : base() {
        }

        public IPCReceaverGUID(string guid)
            : base(guid) {
        }

        public IPCReceaverGUID(int guid)
            : base(guid) {
        }

        protected override string GetPrefix() {
            return "IPCReceaverGUID";
        }
    }
}
