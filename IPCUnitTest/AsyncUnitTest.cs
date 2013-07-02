using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using ICPTestSlave;
using System.Threading;
using SharedMessages;

namespace IPCUnitTest {
    [TestClass]
    public class AsyncUnitTest {
        private BaseIPCReceaver recipient = new BaseIPCReceaver();
        private bool responceReceaved = false;

        public AsyncUnitTest() {
            recipient.OnReceaveIPCMessage += OnReceaveMessage;
        }

        [TestMethod]
        public void DoTest() {
            IIPCGUID slaveReceaverGUID = new IIPCGUID(SlaveManager.Instance().LaunchSlave());
            // wait for slave is launched and ininialized;
            // TODO: contimue after receaving message ???
            Thread.CurrentThread.Join(3000);
            TestAsyncMessage test = new TestAsyncMessage("Hi Slave!");
            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.Success, "Unable to send message");
            }
            Thread.CurrentThread.Join(3000);
            Assert.IsTrue(responceReceaved, "Slave keep silence =(");
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



    }
}
