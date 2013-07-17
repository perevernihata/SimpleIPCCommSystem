using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleIPCCommSystem {
    internal interface IIPCSharedObject {
        string UriSuffix { get; }
    }
}
