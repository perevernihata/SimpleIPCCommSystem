using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using SharedMessages;
using System.Threading;
using ICPTestSlave;
using SimpleIPCCommSystem.Utilities;
using SimpleIPCCommSystem.Messages;

namespace IPCUnitTest {
    [TestClass]
    public class SyncUnitTest {

        public SyncUnitTest() {
        }

        ~SyncUnitTest() {
        }

        [TestMethod]
        public void DoTestSimpleSyncMessage() {
            IIPCGUID slaveReceaverGUID = new IPCGUID(SlaveManager.Instance().LaunchSlave());
            // wait for slave is launched and ininialized;
            Thread.CurrentThread.Join(1000);
            TestSyncMessage test = new TestSyncMessage(new IPCGUID(), 0);
            test.StrIn = "Hi Slave!";
            test.TimeOut = Int32.MaxValue; // !! //SlaveResponces.SyncMessageSlaveDelay + 2000;
            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(test.SenderID)) {
                Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.Success, "Time is up");
            }

            test.TimeOut = Int32.MaxValue;// SlaveResponces.SyncMessageSlaveDelay - 2000;
            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(test.SenderID)) {
                Assert.IsTrue(dispatcher.Dispatch(test)
                    == IPCDispatchResult.Timeout, "Timeout is an expected result");
            }                    
        }
    }
}
