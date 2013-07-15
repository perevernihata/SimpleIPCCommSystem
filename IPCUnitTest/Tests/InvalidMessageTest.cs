using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleIPCCommSystem;
using SimpleIPCCommSystem.Dispatchers;
using SharedMessages;
using SimpleIPCCommSystem.GUIDS;

namespace IPCUnitTest.Tests {
    [TestClass]
    public class InvalidMessageTest {
        [TestMethod]
        public void DoInvalidMessageTest() {
            IIPCGUID receaverGUID = new IPCReceaverGUID();
            using (BaseIPCDispatcher dispatcher = new BaseIPCDispatcher(ReceaverHolder.GlobalApplicationReceaver.ReceaverID)) {
                TestInvalidMessage test = new TestInvalidMessage(receaverGUID);
                IPCDispatchResult tmpResult = dispatcher.Dispatch(test);
                Assert.IsTrue(tmpResult == IPCDispatchResult.InvalidMessageClass, "Receaver not exists but dispatch result is {0}", tmpResult);
            }
        }
    }
}
