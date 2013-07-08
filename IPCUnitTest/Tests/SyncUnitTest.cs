using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using SharedMessages;
using System.Threading;
using ICPTestSlave;
using SimpleIPCCommSystem.Utilities;

namespace IPCUnitTest.Tests {
    [TestClass]
    public class SyncUnitTest {
         
        [TestMethod]
        public void SlaveSimpleSyncMessage() {
            using (SlaveManager currentSlaveManager = new SlaveManager()) {
                IIPCGUID slaveReceaverGUID = new IPCGUID(currentSlaveManager.LaunchSlave());
                // wait for slave is launched and ininialized;
                Thread.CurrentThread.Join(3000);
                TestSyncMessage test = new TestSyncMessage(new IPCGUID(), 0);
                test.StrIn = "Hi Slave!";
                test.TimeOut = SlaveResponces.SyncMessageSlaveDelay + 2000;
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                    IPCDispatchResult dispatchResult = dispatcher.Dispatch(test);
                    Assert.IsTrue(dispatchResult == IPCDispatchResult.Success, "Unable to send message error because of the reason {0}", dispatchResult);
                }
                Assert.IsTrue(String.Equals(test.StrOut, SlaveResponces.TestSyncResponceString), "Time is up");

                test.TimeOut = SlaveResponces.SyncMessageSlaveDelay - 2000;
                using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                    Assert.IsTrue(dispatcher.Dispatch(test)
                        == IPCDispatchResult.Timeout, "Timeout is an expected result");
                }
            }
        }

    }
}
