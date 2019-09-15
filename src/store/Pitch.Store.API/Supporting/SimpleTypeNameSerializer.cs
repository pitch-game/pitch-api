using EasyNetQ;
using System;
using System.Linq;

namespace Pitch.Store.API.Supporting
{
    public class SimpleTypeNameSerializer : ITypeNameSerializer
    {
        private readonly Type[] _typesInAssembly;
        public SimpleTypeNameSerializer(Type[] typesInAssembly)
        {
            _typesInAssembly = typesInAssembly;
        }

        public Type DeSerialize(string typeName)
        {
            return _typesInAssembly.FirstOrDefault(t => t.Name == typeName);
        }

        public string Serialize(Type type)
        {
            return type.Name;
        }
    }
}
