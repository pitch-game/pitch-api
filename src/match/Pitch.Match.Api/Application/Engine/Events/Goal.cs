using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    [BsonDiscriminator("Goal")]
    public class Goal : EventBase, IEvent
    {
        public Goal(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) {}
    }
}
