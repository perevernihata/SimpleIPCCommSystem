using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleIPCCommSystem {
    public class IPCBaseMessagesQueue : MarshalByRefObject {
        public static string URISuffix = "IPCMessagesQueue";

        Queue<IIPCBaseMessage> tasks = new Queue<IIPCBaseMessage>();
        public IPCBaseMessagesQueue() {
        }

        public void EnqueueMessage(IIPCBaseMessage task) {
            tasks.Enqueue(task);
        }

        public IIPCBaseMessage DequeueMessage() {
            return tasks.Dequeue();
        }

        public int Count() {
            return tasks.Count;
        }

        public override object InitializeLifetimeService() {
            return null;
        }
    }
}
