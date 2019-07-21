using EasyNetQ;
using System;
using System.Linq;

namespace Pitch.Squad.Api.Supporting
{
    public class SimpleTypeNameSerializer : ITypeNameSerializer
    {
        //TODO is this the memory leak?
        public Type DeSerialize(string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(t => t.Name == typeName); //TODO performance/caching
        }

        public string Serialize(Type type)
        {
            return type.Name;
        }
    }
}
