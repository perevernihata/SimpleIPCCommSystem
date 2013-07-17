using SimpleIPCCommSystem.Messages;
using System;

namespace SimpleIPCCommSystem {

    public class ReceaveMessageEventArgs : EventArgs {
        private IIPCMessage _message;

        public IIPCMessage Message {
            get { return _message; }
            set { _message = value; }
        }

        public ReceaveMessageEventArgs(IIPCMessage message) {
            _message = message;
        }

    }


    /// <summary>
    /// Declaration for receive message event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="Message"></param>
    public delegate void ReceaveIPCMessageEventHandler(object sender, ReceaveMessageEventArgs e);

    /// <summary>
    /// Used to receave message from any other application or from the same application
    /// </summary>
    public interface IIPCReceaver {
        /// <summary>
        /// Gets current receaver ID
        /// </summary>
        IIPCGUID ReceaverID { get; }

        /// <summary>
        /// Fires when some messages arrive.
        /// </summary>
        event ReceaveIPCMessageEventHandler OnReceaveIPCMessage;
    }
}
