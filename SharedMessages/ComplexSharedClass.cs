using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SharedMessages {

    /// <summary>
    /// Used to test how complex objects can be passed throught the communication system
    /// </summary>
    [Serializable]
    public class ComplexSharedClass : IEquatable<ComplexSharedClass> {

        public DateTime CreatedDate { get; private set; }

        private IEnumerable<string> _testList;
        public IEnumerable<string> TestList {
            get { return _testList; }
            set { _testList = value; }
        }
        

        public ComplexSharedClass(IEnumerable<string> testList) {
            _testList = testList;
            CreatedDate = DateTime.Today;
        }

        public bool Equals(ComplexSharedClass other) {
            return _testList.Equals(other.TestList)
                && CreatedDate.Equals(other.CreatedDate);
        }

    }
}
