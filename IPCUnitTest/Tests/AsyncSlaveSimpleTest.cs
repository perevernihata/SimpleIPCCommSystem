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
    public class AsyncSlaveSimpleTest {
        private bool responceFromSlaveReceaved = false;

        private void OnReceaveMessage(object sender, ReceaveMessageEventArgs e) {
            TestAsyncMessage testAsyncMessage = e.Message as TestAsyncMessage;
            if (testAsyncMessage != null) {
                responceFromSlaveReceaved = true;
                // responce to master                
                Assert.IsTrue(String.Equals(SlaveResponces.TestAsyncResponceString, testAsyncMessage.StrData),
                    "Unexpected responce!");
            }
        }

        public AsyncSlaveSimpleTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage += OnReceaveMessage;
        }

        [Timeout(3630000), TestMethod]
        public void DoAsyncSlaveSimpleTest() {
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
