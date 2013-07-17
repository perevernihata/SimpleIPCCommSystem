using System;
using System.Collections.Generic;
using SimpleIPCCommSystem.Messages;

namespace SimpleIPCCommSystem {
    internal class IPCBaseMessagesQueue : MarshalByRefObject, IIPCSharedObject {
        public static string URISuffix = "IPCMessagesQueueSuffix";

        private Queue<IIPCMessage> tasks = new Queue<IIPCMessage>();
        public IPCBaseMessagesQueue() {
        }

        public void EnqueueMessage(IIPCMessage message) {
            tasks.Enqueue(message);
        }

        public IIPCMessage DequeueMessage() {
            IIPCMessage tmpResult = tasks.Dequeue();
            if (tmpResult is IPCSyncHelperMessage) {
                    IPCSyncHelperMessage messageHelper = tmpResult as IPCSyncHelperMessage;
                    IIPCMessage realMessage = (IIPCMessage)Activator.GetObject(messageHelper.OwnerType,
                        messageHelper.OwnerFullUri);
                    return realMessage;
            } else
                return tmpResult;

        }

        public int Count() {
            return tasks.Count;
        }

        public override object InitializeLifetimeService() {
            return null;
        }

        public string UriSuffix {
            get { return URISuffix; }
        }
    }
}
