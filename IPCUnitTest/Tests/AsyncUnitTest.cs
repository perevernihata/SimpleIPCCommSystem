using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using ICPTestSlave;
using System.Threading;
using SharedMessages;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem.Dispatchers;
using SimpleIPCCommSystem.GUIDS;

namespace IPCUnitTest.Tests {
    [TestClass]
    public class AsyncUnitTest {
        private bool responceFromSlaveReceaved = false;
        private bool messageDispatchedToCurrentReceaver = false;
        private string messageData = "some important things";
        private bool messageNotCorrupted = false;

        public AsyncUnitTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage += OnReceaveMessage;
        }

        ~AsyncUnitTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage -= OnReceaveMessage;
        }

        public void OnReceaveMessage(object sender, IIPCBaseMessage message) {

            TestAsyncMessage testAsyncMessage = message as TestAsyncMessage;
            IIPCBaseReceaver currentReceaver = sender as IIPCBaseReceaver;
            if (currentReceaver != null && testAsyncMessage != null && currentReceaver.ReceaverID.Equals(testAsyncMessage.SenderID)) {
                messageNotCorrupted = String.Equals(testAsyncMessage.StrData, messageData);
                messageDispatchedToCurrentReceaver = true;
            }

            if (testAsyncMessage != null) {
                responceFromSlaveReceaved = true;
                // responce to master                
                Assert.IsTrue(String.Equals(SlaveResponces.TestAsyncResponceString, testAsyncMessage.StrData),
                    "Unexpected responce!");
            }
        }

        [Priority(1), Timeout(3630000), TestMethod]
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

        [Timeout(3630000), TestMethod]
        public void SlaveSimpleAsyncMessage() {
            using (SlaveManager currentSlaveManager = new SlaveManager()) {
                IIPCGUID slaveReceaverGUID = new IPCReceaverGUID(currentSlaveManager.LaunchSlave());
                // wait for slave is launched and ininialized
                Thread.CurrentThread.Join(3000);
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                    TestAsyncMessage test = new TestAsyncMessage(ReceaverHolder.GlobalApplicationReceaver.ReceaverID);
                    test.StrData = "Hi Slave!";
                    Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.Success, "Unable to send message");
                }
                Thread.CurrentThread.Join(3000);
                Assert.IsTrue(responceFromSlaveReceaved, "Slave keep silence =(");
            }
        }
    }
}
