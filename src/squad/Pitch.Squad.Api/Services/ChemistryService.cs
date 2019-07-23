using Pitch.Squad.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Squad.Api.Services
{
    public interface IChemistryService
    {
        void SetChemistry(IDictionary<string, Card> lineup);
    }

    public class ChemistryService : IChemistryService
    {
        private static readonly IDictionary<string, string> _groupPositions = new Dictionary<string, string>
        {
            { "LCB", "CB" },
            { "RCB", "CB" },
            { "LCM", "CM" },
            { "RCM", "CM" },
            { "LST", "ST" },
            { "RST", "ST" }
        };

        private static readonly IDictionary<string, IEnumerable<(string position, int chemistry)>> _chemistryMap = new Dictionary<string, IEnumerable<(string position, int chemistry)>> {
            {
                "GK", new List<(string position, int chemistry)>() {
                    ("GK", 100)
                }
            },
            {
                "CB", new List<(string position, int chemistry)>() {
                    ("CB", 100),
                    ("LB", 75),
                    ("RB", 75)
                }
            },
            {
                "LB", new List<(string position, int chemistry)>() {
                    ("LB", 100),
                    ("RB", 90),
                    ("CB", 75)
                }
            },
            {
                "RB", new List<(string position, int chemistry)>() {
                    ("RB", 100),
                    ("LB", 90),
                    ("CB", 75)
                }
            },
            {
                "CM", new List<(string position, int chemistry)>() {
                    ("CM", 100),
                    ("LM", 75),
                    ("RM", 75)
                }
            },
            {
                "LM", new List<(string position, int chemistry)>() {
                    ("LM", 100),
                    ("RM", 90),
                    ("CM", 75)
                }
            },
            {
                "RM", new List<(string position, int chemistry)>() {
                    ("RM", 100),
                    ("LM", 90),
                    ("CM", 75)
                }
            },
            {
                "ST", new List<(string position, int chemistry)>() {
                    ("ST", 100),
                    ("LM", 75),
                    ("RM", 75),
                    ("CM", 50)
                }
            }
        };

        public void SetChemistry(IDictionary<string, Card> lineup)
        {
            foreach (var position in lineup)
            {
                var realPosition = _groupPositions.ContainsKey(position.Key) ? _groupPositions[position.Key] : position.Key;
                var chemistryLookup = _chemistryMap[realPosition];
                if (position.Value != null)
                {
                    var card = position.Value;
                    var chemistry = chemistryLookup.FirstOrDefault(x => x.position == card.Position).chemistry; //TODO handle nulls
                    card.Chemistry = chemistry;
                }
            }
        }
    }
}
