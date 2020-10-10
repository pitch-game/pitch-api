using MongoDB.Bson.Serialization;

namespace Pitch.Match.API.Supporting
{
    public static class BsonClassMapExtensions
    {
        public static void TryRegisterClassMap<T>()
        {
            var exists = BsonClassMap.IsClassMapRegistered(typeof(T));
            if (exists) return;

            BsonClassMap.RegisterClassMap<T>();
        }
    }
}
