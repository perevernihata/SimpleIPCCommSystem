using SharedMessages;
using System.Collections.Generic;
using System.Xml;

namespace ICPTestSlave {
    internal class SlaveResponces {
        public const string TestAsyncResponceString = "Hi Master!";
        public const string TestSyncResponceString = "Synchronous responce";
        public const int SyncMessageSlaveDelay = 2000;
        public static ComplexSharedClass ConstructComplexResponceTemplate() {

            // prepare list
            List<string> testList = new List<string>();
            for (int i = 0; i <= 42; i++) {
                testList.Add(string.Format("Number {0}", i));
            }
            ComplexSharedClass tmpResult = new ComplexSharedClass(testList);
            return tmpResult;
        }
    }
}
