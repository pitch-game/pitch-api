using System.Collections.Generic;
using Pitch.Squad.API.Exceptions;
using Pitch.Squad.API.Models;

namespace Pitch.Squad.API.Services
{
    public interface IChemistryService
    {
        void SetChemistryOnLineup(IDictionary<string, Card> lineup);
    }

    public class ChemistryService : IChemistryService
    {
        private static readonly IDictionary<string, string> PositionOverrides = new Dictionary<string, string>
        {
            {"LCB", "CB"},
            {"RCB", "CB"},
            {"LCM", "CM"},
            {"RCM", "CM"},
            {"LST", "ST"},
            {"RST", "ST"}
        };

        private static readonly IDictionary<string, Dictionary<string, int>> ChemistryMap =
            new Dictionary<string, Dictionary<string, int>>
            {
                {
                    "GK", new Dictionary<string, int>
                    {
                        {"GK", 100}
                    }
                },
                {
                    "CB", new Dictionary<string, int>
                    {
                        {"CB", 100},
                        {"LB", 75},
                        {"RB", 75}
                    }
                },
                {
                    "LB", new Dictionary<string, int>
                    {
                        {"LB", 100},
                        {"RB", 90},
                        {"CB", 75},
                        {"LM", 50},
                        {"RM", 50}
                    }
                },
                {
                    "RB", new Dictionary<string, int>
                    {
                        {"RB", 100},
                        {"LB", 90},
                        {"CB", 75},
                        {"LM", 50},
                        {"RM", 50}
                    }
                },
                {
                    "CM", new Dictionary<string, int>
                    {
                        {"CM", 100},
                        {"LM", 75},
                        {"RM", 75}
                    }
                },
                {
                    "LM", new Dictionary<string, int>
                    {
                        {"LM", 100},
                        {"RM", 90},
                        {"CM", 75},
                        {"LB", 50},
                        {"RB", 50}
                    }
                },
                {
                    "RM", new Dictionary<string, int>
                    {
                        {"RM", 100},
                        {"LM", 90},
                        {"CM", 75},
                        {"LB", 50},
                        {"RB", 50}
                    }
                },
                {
                    "ST", new Dictionary<string, int>
                    {
                        {"ST", 100},
                        {"LM", 75},
                        {"RM", 75},
                        {"CM", 50}
                    }
                }
            };

        public void SetChemistryOnLineup(IDictionary<string, Card> lineup)
        {
            foreach (var (position, card) in lineup)
            {
                if (card == null) continue;

                var actualPosition = position;

                var hasCorePosition = PositionOverrides.TryGetValue(actualPosition, out var corePosition);
                if (hasCorePosition)
                {
                    actualPosition = corePosition;
                }

                var hasChemistryLookup = ChemistryMap.TryGetValue(actualPosition, out var chemistryLookup);
                if (!hasChemistryLookup) throw new ChemistryException($"Could not find {actualPosition} in chemistry map.");

                chemistryLookup.TryGetValue(card.Position, out var chemistryValue);

                card.Chemistry = chemistryValue;
            }
        }
    }
}