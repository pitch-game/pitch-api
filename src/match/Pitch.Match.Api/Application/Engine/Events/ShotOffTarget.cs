using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    [BsonDiscriminator("ShotOffTarget")]
    public class ShotOffTarget : EventBase, IEvent
    {
        public ShotOffTarget(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Shot Off Target";

        public bool ShowInTimeline => true;
    }
}
