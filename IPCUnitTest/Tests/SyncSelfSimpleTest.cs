using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using SimpleIPCCommSystem.Dispatchers;
using SharedMessages;
using System.Threading;
using SimpleIPCCommSystem.GUIDS;
using SimpleIPCCommSystem.Messages;
using System;

namespace IPCUnitTest.Tests {
    [TestClass]
    public class SyncSelfSimpleTest {

        private string TestMessageIn = "TestMessageIn";
        private string TestMessageOut = "TestMessageOut";

        public void OnReceaveMessage(object sender, ReceaveMessageEventArgs e) {

            TestSyncMessage testSyncMessage = e.Message as TestSyncMessage;
            IIPCReceaver currentReceaver = sender as IIPCReceaver;
            if (currentReceaver != null && testSyncMessage != null) {
                Assert.IsTrue(currentReceaver.ReceaverID.Equals(testSyncMessage.SenderID), "Message receaved from wrong receaver");
                Assert.IsTrue(TestMessageIn.Equals(testSyncMessage.StrIn), "Message is corrupted");
                testSyncMessage.StrOut = TestMessageOut;
            }
        }

        public SyncSelfSimpleTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage += OnReceaveMessage;
        }

        [Timeout(3630000), TestMethod]
        public void DoSyncSelfSimpleTest() {
            IIPCGUID receaverGUID = new IPCReceaverGUID();
            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(receaverGUID)) {
                TestSyncMessage test = new TestSyncMessage(receaverGUID);
                test.StrIn = TestMessageIn;
                Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.Success, "Unable to send message");
                Assert.IsTrue(TestMessageOut.Equals(test.StrOut), "Message is corrupted");
            }
        }
    }
}
