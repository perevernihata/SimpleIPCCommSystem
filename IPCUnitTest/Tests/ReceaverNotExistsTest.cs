using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem.GUIDS;
using SimpleIPCCommSystem;
using SharedMessages;
using SimpleIPCCommSystem.Dispatchers;

namespace IPCUnitTest.Tests {
    [TestClass]
    public class ReceaverNotExistsTest {

        private const int NonExistentPID = 32768;

        [TestMethod]
        public void DoReceaverNotExistsTest() {
            IIPCGUID receaverGUID = new IPCReceaverGUID(NonExistentPID);
            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(receaverGUID)) {
                TestAsyncMessage test = new TestAsyncMessage(receaverGUID);
                Assert.IsTrue(dispatcher.Dispatch(test) == IPCDispatchResult.UnexpectedFail, "Receaver not exists but message was sent");
            }
        }
    }
}
