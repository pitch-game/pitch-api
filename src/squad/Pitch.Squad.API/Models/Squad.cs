using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Squad.API.Infrastructure;

namespace Pitch.Squad.API.Models
{
    public class Squad : IEntity
    {
        public Squad()
        {
            Formation = SquadFormation.FourFourTwo;
            Lineup = FormationLookup.AllowedPositions[Formation].ToDictionary(x => x.ToString(), x => (Guid?)null);
            Subs = new Guid?[6];
            Instructions = new SquadInstructions();
            LastUpdated = DateTime.Now;
        }
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public SquadFormation Formation;
        public IDictionary<string, Guid?> Lineup { get; set; }
        public Guid?[] Subs { get; set; }
        public SquadInstructions Instructions { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class SquadInstructions
    {
        public CoreStyle Core { get; set; }
        public AttackingStyle Attacking { get; set; }
        public DefensiveStyle Defensive { get; set; }
    }

    public enum CoreStyle
    {
        Default,
        Attacking,
        Defensive
    }

    public enum AttackingStyle
    {
        Default,
        Direct,
        Short
    }

    public enum DefensiveStyle
    {
        Default,
        Press,
        Contain
    }

    public enum SquadFormation
    {
        FourFourTwo
    }

    public enum Position
    {
        GK,
        LB,
        LCB,
        RCB,
        RB,
        LM,
        LCM,
        RCM,
        RM,
        LST,
        RST
    }

    public static class FormationLookup
    {
        public static IDictionary<SquadFormation, Position[]> AllowedPositions => new Dictionary<SquadFormation, Position[]> {
            {
                SquadFormation.FourFourTwo, new[] { Position.GK, Position.LB, Position.LCB, Position.RCB, Position.RB, Position.LM, Position.LCM, Position.RCM, Position.RM, Position.LST, Position.RST }
            }
        };
    }
}
