using SimpleIPCCommSystem.Messages;

namespace SimpleIPCCommSystem {

    public delegate void ReceaveIPCMessageEventHandler(object sender, IIPCBaseMessage Message);

    public interface IIPCBaseReceaver {
        IIPCGUID ReceaverID { get; }
        event ReceaveIPCMessageEventHandler OnReceaveIPCMessage;
    }
}
