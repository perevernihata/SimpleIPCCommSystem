using System;
using SimpleIPCCommSystem.Utilities;

namespace SimpleIPCCommSystem.Messages {

    /// <summary>
    /// Helper class used to translate info about synchronous message to messages queue 
    /// </summary>
    [Serializable]
    internal class IPCSyncHelperMessage : IPCBaseAsyncMessage {
        public IIPCGUID RealMessageID { get; private set; }

        public string OwnerFullUri { get; private set; }
        public Type OwnerType { get; private set; }

        public IPCSyncHelperMessage(IPCBaseSyncMessage owner, IIPCGUID id)
            : base(id) {
            RealMessageID = owner.SenderID;
            OwnerType = owner.GetType();
            OwnerFullUri = new IPCUri(owner.SenderID, owner).Value;
        }
    }

}
