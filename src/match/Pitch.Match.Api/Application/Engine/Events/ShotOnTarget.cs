using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    [BsonDiscriminator("ShotOnTarget")]
    public class ShotOnTarget : EventBase, IEvent
    {
        public ShotOnTarget(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }

        public string Name => "Shot Saved";

        public bool ShowInTimeline => true;
    }
}
