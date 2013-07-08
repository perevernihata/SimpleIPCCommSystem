using System;

namespace SimpleIPCCommSystem.Utilities {
    public interface IIPCGUID : IEquatable<IIPCGUID> {
        string Value { get; }
    }
}
