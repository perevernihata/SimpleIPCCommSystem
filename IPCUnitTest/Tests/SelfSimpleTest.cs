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
    public class SelfSimpleTest {

        private bool messageDispatchedToCurrentReceaver = false;
        private string messageData = "some important things";
        private bool messageNotCorrupted = false;

        public void OnReceaveMessage(object sender, IIPCBaseMessage message) {

            TestAsyncMessage testAsyncMessage = message as TestAsyncMessage;
            IIPCBaseReceaver currentReceaver = sender as IIPCBaseReceaver;
            if (currentReceaver != null && testAsyncMessage != null && currentReceaver.ReceaverID.Equals(testAsyncMessage.SenderID)) {
                messageNotCorrupted = String.Equals(testAsyncMessage.StrData, messageData);
                messageDispatchedToCurrentReceaver = true;
            }
        }

        public SelfSimpleTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage += OnReceaveMessage;
        }

        ~SelfSimpleTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage -= OnReceaveMessage;
        }


        [Timeout(3630000), TestMethod]
        public void SelfSimpleAsyncMessage() {
            IIPCGUID receaverGUID = new IPCReceaverGUID();
            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(receaverGUID)) {
                TestAsyncMessage test = new TestAsyncMessage(receaverGUID);
                test.StrData = messageData;
                Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.Success, "Unable to send message");
            }
            Thread.Sleep(3000);
            Assert.IsTrue(messageDispatchedToCurrentReceaver, "Message was not dispathed to the current receaver");
            Assert.IsTrue(messageNotCorrupted, "Message dispathed to the current receaver has been corrupted");
        }
    }
}
