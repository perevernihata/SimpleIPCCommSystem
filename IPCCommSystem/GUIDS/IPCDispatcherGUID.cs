using System;

namespace SimpleIPCCommSystem.GUIDS {
    [Serializable]
    public class IPCDispatcherGUID : IPCGUID {
        protected override string GetPrefix() {
            return "IPCDispatcherGUID";
        }
    }
}
