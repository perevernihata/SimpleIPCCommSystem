using System;
using SimpleIPCCommSystem;
using SharedMessages;
using System.Threading;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem.Utilities;

namespace ICPTestSlave {
    class SlaveMain {
        static BaseIPCReceaver recipient = new BaseIPCReceaver();

        static void Main(string[] args) {
            recipient.OnReceaveIPCMessage += OnReceaveMessage;
            Console.ReadLine();
        }

        public static void OnReceaveMessage(object sender, IIPCBaseMessage message) {

            TestAsyncMessage testAsyncMessage = message as TestAsyncMessage;
            if (testAsyncMessage != null) {
                // responce to master
                TestAsyncMessage test = new TestAsyncMessage(new IPCGUID());
                test.StrData = SlaveResponces.TestAsyncResponceString;
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(testAsyncMessage.SenderID)) {
                    dispatcher.Dispatch(test);
                }
                return;
            }

            TestSyncMessage testSyncMessage = message as TestSyncMessage;
            if (testSyncMessage != null) {
                // responce to master - does not work now
                Thread.CurrentThread.Join(SlaveResponces.SyncMessageSlaveDelay);
                testSyncMessage.StrOut = "testOut";
            }
        }
    }
}
