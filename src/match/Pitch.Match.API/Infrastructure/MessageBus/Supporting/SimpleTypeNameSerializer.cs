using EasyNetQ;
using System;
using System.Linq;

namespace Pitch.Match.Api.Infrastructure.MessageBus.Supporting
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
