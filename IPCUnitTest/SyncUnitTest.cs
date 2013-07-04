using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using SharedMessages;
using System.Threading;
using ICPTestSlave;

namespace IPCUnitTest {
    [TestClass]
    public class SyncUnitTest {

        public SyncUnitTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage += OnReceaveMessage;
        }

        ~SyncUnitTest() {
            ReceaverHolder.GlobalApplicationReceaver.OnReceaveIPCMessage -= OnReceaveMessage;
        }

        [TestMethod]
        public void DoTestSimpleSyncMessage() {
            IIPCGUID slaveReceaverGUID = new IIPCGUID(SlaveManager.Instance().LaunchSlave());
            // wait for slave is launched and ininialized;
            Thread.CurrentThread.Join(1000);
            TestSyncMessage test = new TestSyncMessage("Hi Slave!");

            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                Assert.IsTrue(dispatcher.Dispatch(test, SlaveResponces.SyncMessageSlaveDelay + 2000) ==
                    IPCDispatchResult.Success, "Time is up");
            }

            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(slaveReceaverGUID)) {
                Assert.IsTrue(dispatcher.Dispatch(test, SlaveResponces.SyncMessageSlaveDelay - 2000)
                    == IPCDispatchResult.Timeout, "Timeout is explected result");
            }                    
        }
    }
}
