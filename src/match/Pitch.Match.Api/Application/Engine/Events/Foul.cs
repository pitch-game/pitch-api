using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    [BsonDiscriminator("Foul")]
    public class Foul : EventBase, IEvent
    {
        public Foul(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Foul";

        public bool ShowInTimeline => false;
    }
}
