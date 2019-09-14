using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pitch.User.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public int XP { get; set; }
        public int Money { get; set; }

        [BsonIgnore]
        public int Level => XP > 1000 ? XP/1000 : 1; //TODO not linear
    }
}
