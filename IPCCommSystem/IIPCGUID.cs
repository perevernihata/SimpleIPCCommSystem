using System;

namespace SimpleIPCCommSystem {
    public interface IIPCGUID : IEquatable<IIPCGUID> {
        string Value { get; }
    }
}
