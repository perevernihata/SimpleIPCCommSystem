using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedMessages;
using SimpleIPCCommSystem;
using SimpleIPCCommSystem.Dispatchers;
using SimpleIPCCommSystem.GUIDS;
using SimpleIPCCommSystem.Messages;
using ICPTestSlave;

namespace IPCUnitTest.Tests {
    [TestClass]
    public class AsyncComplexSlaveTest {
        private bool responceFromSlaveReceaved = false;
        private void OnReceaveMessage(object sender, ReceaveMessageEventArgs e) {
            
            TestAsyncComplexMessage testAsyncMessage = e.Message as TestAsyncComplexMessage;
            if (testAsyncMessage != null) {
                responceFromSlaveReceaved = true;
                ComplexSharedClass tmpAdditionalInfo = SlaveResponces.ConstructComplexResponceTemplate();
                bool isTheSameInfo = tmpAdditionalInfo.Equals(testAsyncMessage.AdditionalInfo);
                Assert.IsTrue(isTheSameInfo,
                    "Unexpected responce!");
            }
        }

        public AsyncComplexSlaveTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage += OnReceaveMessage;
        }

        // this test do not pass sometimes without any reason. 
        // Test result - The agent process was stopped while the test was running.
        // Reproduces only in case it was started not from IDE
        [Timeout(3630000), TestMethod]
        public void DoAsyncComplexSlaveTest() {
            using (SlaveManager currentSlaveManager = new SlaveManager()) {
                IIPCGUID slaveReceaverGUID = new IPCReceaverGUID(currentSlaveManager.LaunchSlave());
                // wait for slave is launched and ininialized
                Thread.CurrentThread.Join(3000);
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                    TestAsyncComplexMessage test = new TestAsyncComplexMessage(ReceaverHolder.GlobalApplicationReceaver.ReceaverID, null);
                    Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.Success, "Unable to send message");
                }
                Thread.CurrentThread.Join(3000);
                Assert.IsTrue(responceFromSlaveReceaved, "Slave keep silence =(");
            }
        }
    }
}
