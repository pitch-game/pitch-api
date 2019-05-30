using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pitch.Squad.Api.Models
{
    public class Squad
    {
        public Squad(SquadFormation formation = SquadFormation.FourFourTwo)
        {
            Formation = formation;
            Lineup = FormationLookup.AllowedPositions[Formation].ToDictionary(x => x.ToString(), x => (Guid?)null);
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public SquadFormation Formation;
        public IDictionary<string, Guid?> Lineup { get; set; }
        public IEnumerable<Guid> Subs { get; set; }
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
                SquadFormation.FourFourTwo, new Position[] { Position.GK, Position.LB, Position.LCB, Position.RCB, Position.RB, Position.LM, Position.LCM, Position.RCM, Position.RM, Position.LST, Position.RST }
            }
        };
    }
}
