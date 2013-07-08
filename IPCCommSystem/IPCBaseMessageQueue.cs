using System;
using System.Collections.Generic;
using SimpleIPCCommSystem.Messages;

namespace SimpleIPCCommSystem {
    public class IPCBaseMessagesQueue : MarshalByRefObject, IIPCSharedObject {
        public static string URISuffix = "IPCMessagesQueueSuffix";

        Queue<IIPCBaseMessage> tasks = new Queue<IIPCBaseMessage>();
        public IPCBaseMessagesQueue() {
        }

        public void EnqueueMessage(IIPCBaseMessage message) {
            tasks.Enqueue(message);
        }

        public IIPCBaseMessage DequeueMessage() {
            IIPCBaseMessage tmpResult = tasks.Dequeue();
            if (tmpResult is IPCSyncHelperMessage) {
                    IPCSyncHelperMessage messageHelper = tmpResult as IPCSyncHelperMessage;
                    IIPCBaseMessage realMessage = (IIPCBaseMessage)Activator.GetObject(messageHelper.OwnerType,
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
