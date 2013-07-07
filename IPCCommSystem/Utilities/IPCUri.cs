using System;
using SimpleIPCCommSystem.Resources;

namespace SimpleIPCCommSystem.Utilities {

    [Serializable]
    internal class IPCUri: IIPCUri {

        private IIPCGUID _sharedObjGuid;
        private string _suffix;

        public IPCUri(IIPCGUID sharedObjGuid, IIPCSharedObject suffix) {
            _sharedObjGuid = sharedObjGuid;
            _suffix = suffix.UriSuffix;
        }

        public string Value {
            get { return String.Format(CommonResource.BaseUri, _sharedObjGuid.Value, _suffix); }
        }
    }
}
