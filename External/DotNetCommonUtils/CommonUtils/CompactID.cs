using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CommonUtils
{
    [Serializable]
    [DataContract(Namespace = "")]
    public struct CompactID : ISerializable
    {
        private readonly Guid guid;
        public CompactID(Guid guid)
        {
            this.guid = guid;
        }

        private CompactID(SerializationInfo info, StreamingContext context)
        {
            this.guid =  new Guid(Utils.Base64StringToByteArray(info.GetString("v")));
        }

        public CompactID(byte[] data)
        {
            this.guid = new Guid(data);
        }

        public Guid Guid
        {
            get { return this.guid; }
        }

        public override string ToString()
        {
            return Utils.ByteArrayToBase64String(this.guid.ToByteArray());
        }

        public override bool Equals(object anotherObject)
        {
            return guid.Equals(anotherObject);
        }

        public static bool operator ==(CompactID a, CompactID b) { return a.Equals(b); }
        public static bool operator !=(CompactID a, CompactID b) { return !a.Equals(b); }

        public override int GetHashCode()
        {
            return this.guid.GetHashCode();
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("v", this.ToString());
        }

        #endregion
    }
}
