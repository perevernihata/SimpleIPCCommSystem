using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using ICPTestSlave;
using System.Threading;
using SharedMessages;
using SimpleIPCCommSystem.Messages;
using SimpleIPCCommSystem.Utilities;

namespace IPCUnitTest.Tests {
    [TestClass]
    public class AsyncUnitTest {
        private bool responceReceaved = false;

        public AsyncUnitTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage += OnReceaveMessage;
        }

        ~AsyncUnitTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage -= OnReceaveMessage;
        }

        public void OnReceaveMessage(object sender, IIPCBaseMessage message) {
            TestAsyncMessage testAsyncMessage = message as TestAsyncMessage;
            if (testAsyncMessage != null) {
                responceReceaved = true;
                // responce to master                
                Assert.IsTrue(String.Equals(SlaveResponces.TestAsyncResponceString, testAsyncMessage.StrData),
                    "Unexpected responce!");
            }
        }

        [TestMethod]
        public void SlaveSimpleAsyncMessage() {
            using (SlaveManager currentSlaveManager = new SlaveManager()) {
                IIPCGUID slaveReceaverGUID = new IPCGUID(currentSlaveManager.LaunchSlave());
                // wait for slave is launched and ininialized
                Thread.CurrentThread.Join(3000);
                TestAsyncMessage test = new TestAsyncMessage(new IPCGUID());
                test.StrData = "Hi Slave!";
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                    Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.Success, "Unable to send message");
                }
                Thread.CurrentThread.Join(3000);
                Assert.IsTrue(responceReceaved, "Slave keep silence =(");
            }
        }
    }
}
