using SharedMessages;
using System.Collections;
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

            // prepare XmlDocument
            XmlDocument tmpXml = new XmlDocument();
            tmpXml.LoadXml("<Root><Child>1</Child><Child>2</Child><Child>3</Child></Root>");

            ComplexSharedClass tmpResult = new ComplexSharedClass(testList, tmpXml);

            return tmpResult;
    }
    }
}
