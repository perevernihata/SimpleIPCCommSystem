using System;
using SharedMessages;
using System.Threading;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem.GUIDS;
using SimpleIPCCommSystem.Receavers;
using SimpleIPCCommSystem.Dispatchers;

namespace ICPTestSlave {
    class SlaveMain {
        static BaseIPCReceaver recipient = new BaseIPCReceaver();

        static void Main(string[] args) {
            recipient.OnReceaveIPCMessage += OnReceaveMessage;
            Console.ReadLine();
        }

        public static void OnReceaveMessage(object sender, IIPCBaseMessage message) {

            Console.WriteLine(String.Format("Receaved message of type - {0} from sender with id = {1}", message.MessageType, message.SenderID.Value));
            TestAsyncMessage testAsyncMessage = message as TestAsyncMessage;
            if (testAsyncMessage != null) {
                Console.WriteLine("Preparing responce to the master...");
                TestAsyncMessage test = new TestAsyncMessage(new IPCReceaverGUID());
                test.StrData = SlaveResponces.TestAsyncResponceString;
                Console.WriteLine("Forward responce to the master...");
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(testAsyncMessage.SenderID)) {
                    dispatcher.Dispatch(test);
                    Console.WriteLine("Message has been forwarded succesfully!");
                }
                return;
            }

            TestSyncMessage testSyncMessage = message as TestSyncMessage;
            if (testSyncMessage != null) {
                Console.WriteLine("Emulation of some processing");
                Thread.CurrentThread.Join(SlaveResponces.SyncMessageSlaveDelay);
                testSyncMessage.StrOut = SlaveResponces.TestSyncResponceString;
                Console.WriteLine("Continue work....");
            }
        }
    }
}
