using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Lambda;

namespace System.Reflection
{
    public class DynVal
    {
        private MemoryStream _ums;
        private TypeInfo _type;
        private BinaryFormatter _formatter;

        private DynVal(byte[] buffer, TypeInfo ti, BinaryFormatter formatter)
        {
            _ums = new MemoryStream(buffer);
            _type = ti;
            _formatter = formatter;
        }

        public TypeInfo Type => _type;

        public static DynVal Init(object value)
        {
            var f = new BinaryFormatter();
            var ti = value.GetType().GetTypeInfo();
            var ms = new MemoryStream();
            f.Serialize(ms, value);
            var buffer = ms.ToArray();
            return new DynVal(buffer, ti, f);
        }

        public DynVal Set(object value) => Init(value);

        public T Get<T>() => (T)_formatter.Deserialize(_ums);
        public override string ToString()
        {
            if (_type.AsType() == typeof(string))
            {
                return Get<string>();
            }
            else
            {
                return Get<byte[]>().ToBase64();
            }
        }
        public override bool Equals(object obj) => obj.GetType().GetTypeInfo() == _type ? true : false;

        public override int GetHashCode()
        {
            var hashCode = -1897959600;
            hashCode = hashCode * -1521134295 + EqualityComparer<MemoryStream>.Default.GetHashCode(_ums);
            hashCode = hashCode * -1521134295 + EqualityComparer<TypeInfo>.Default.GetHashCode(_type);
            hashCode = hashCode * -1521134295 + EqualityComparer<BinaryFormatter>.Default.GetHashCode(_formatter);
            hashCode = hashCode * -1521134295 + EqualityComparer<TypeInfo>.Default.GetHashCode(Type);
            return hashCode;
        }

        public static bool operator ==(DynVal val1, DynVal val2) => EqualityComparer<DynVal>.Default.Equals(val1, val2);
        public static bool operator !=(DynVal val1, DynVal val2) => !(val1 == val2);
    }
}