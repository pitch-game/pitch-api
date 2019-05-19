﻿using EasyNetQ;
using System;
using System.Linq;

namespace Pitch.Store.Api.Supporting
{
    public class SimpleTypeNameSerializer : ITypeNameSerializer
    {
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