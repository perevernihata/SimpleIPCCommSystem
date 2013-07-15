using System;
using SharedMessages;
using System.Threading;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem.GUIDS;
using SimpleIPCCommSystem.Receavers;
using SimpleIPCCommSystem.Dispatchers;
using SimpleIPCCommSystem;

namespace ICPTestSlave {
    class SlaveMain {
        static BaseIPCReceaver recipient = new BaseIPCReceaver();

        static void Main(string[] args) {
            recipient.OnReceaveIPCMessage += OnReceaveMessage;
            Console.ReadLine();
        }

        private static void ForwardResponce(IIPCMessage message, IIPCGUID receaver) {
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(receaver)) {
                    if (dispatcher.Dispatch(message) == IPCDispatchResult.Success)
                        Console.WriteLine("Message has been forwarded succesfully!");
                    else
                        Console.WriteLine("Unable to forward message");
                }
        }

        public static void OnReceaveMessage(object sender, IIPCMessage message) {

            Console.WriteLine(String.Format("Receaved message of type - {0} from sender with id = {1}", message.MessageType, message.SenderID.Value));
            TestAsyncMessage testAsyncMessage = message as TestAsyncMessage;
            if (testAsyncMessage != null) {
                Console.WriteLine("Preparing responce to the master..." + testAsyncMessage.GetType().FullName);
                TestAsyncMessage test = new TestAsyncMessage(new IPCReceaverGUID());
                test.StrData = SlaveResponces.TestAsyncResponceString;
                Console.WriteLine("Forward responce to the master...");
                ForwardResponce(test, testAsyncMessage.SenderID);
                return;
            }

            TestAsyncComplexMessage testComplexAsyncMessage = message as TestAsyncComplexMessage;
            if (testComplexAsyncMessage != null) {
                Console.WriteLine("Preparing responce to the master..." + testComplexAsyncMessage.GetType().FullName);
                TestAsyncComplexMessage test = new TestAsyncComplexMessage(new IPCReceaverGUID(),SlaveResponces.ConstructComplexResponceTemplate());
                Console.WriteLine("Forward responce to the master...");
                ForwardResponce(test, testComplexAsyncMessage.SenderID);
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
