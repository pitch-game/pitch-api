using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pitch.Match.Api.Application.Engine.Events
{
    [BsonDiscriminator("YellowCard")]
    public class YellowCard : EventBase, IEvent
    {
        public YellowCard(int minute, Guid cardId, Guid squadId) : base(minute, cardId, squadId) { }
    }
}
