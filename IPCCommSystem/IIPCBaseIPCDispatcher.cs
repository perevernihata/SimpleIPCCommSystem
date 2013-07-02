using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleIPCCommSystem {

    public enum IPCDispatchResult {
        Fail,
        Success,
        OutOfTimeout
    }

    public interface IIPCBaseIPCDispatcher {
        IPCDispatchResult Dispatch(IIPCBaseMessage message, int timeout = 0);

        IIPCGUID Receaver { get; }
    }
}
