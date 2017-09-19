using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Reflection
{
    public class Let
    {
        private MemoryStream _ums;
        private TypeInfo _type;
        private BinaryFormatter _formatter;

        private Let(byte[] buffer, TypeInfo ti, BinaryFormatter formatter)
        {
            _ums = new MemoryStream(buffer);
            _type = ti;
            _formatter = formatter;
        }

        public TypeInfo Type => _type;

        public static Let Set(object value)
        {
            var f = new BinaryFormatter();
            var ti = value.GetType().GetTypeInfo();
            var ms = new MemoryStream();
            f.Serialize(ms, value);
            var buffer = ms.ToArray();
            return new Let(buffer, ti, f);
        }

        public T Get<T>() => (T)_formatter.Deserialize(_ums);
    }
}