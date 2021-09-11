using MongoDB.Bson.Serialization;

namespace Pitch.Match.API.Supporting
{
    public static class BsonClassMapExtensions
    {
        public static void TryRegisterClassMap<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>();
            }
        }
    }
}