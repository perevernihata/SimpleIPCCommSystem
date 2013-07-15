using System;

namespace SimpleIPCCommSystem {

    /// <summary>
    /// Responsible for encapsulation logic of creating valid guid's for any kind of IPC objects
    /// </summary>
    public interface IIPCGUID : IEquatable<IIPCGUID> {
        /// <summary>
        /// Value of GUID
        /// </summary>
        string Value { get; }
    }
}
