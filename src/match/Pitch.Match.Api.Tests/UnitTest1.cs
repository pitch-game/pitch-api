using System;
using Xunit;
using Pitch.Match.Api.Application.Engine.Action;
using Pitch.Match.Api.Services;
using Pitch.Match.Api.Models;
using System.Collections.Generic;

namespace Pitch.Match.Api.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var actions = new IAction[] { new Application.Engine.Action.RedCard(), new Shot(), new Application.Engine.Action.YellowCard() };
            var engine = new MatchEngine(actions);

            var lineup = new Dictionary<PositionalArea, IEnumerable<Card>>()
                {
                    { PositionalArea.GK, new List<Card>()
                        {
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "GK",
                                Rating = 80,
                                Fitness = 100
                            }
                        }
                    },
                { PositionalArea.DEF, new List<Card>()
                        {
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "LB",
                                Rating = 80,
                                Fitness = 100
                            },
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "CB",
                                Rating = 80,
                                Fitness = 100
                            },
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "CB",
                                Rating = 80,
                                Fitness = 100
                            },
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "RB",
                                Rating = 80,
                                Fitness = 100
                            }
                        }
                    },
                { PositionalArea.MID, new List<Card>()
                        {
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "LM",
                                Rating = 80,
                                Fitness = 100
                            },
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "CM",
                                Rating = 80,
                                Fitness = 100
                            },
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "CM",
                                Rating = 80,
                                Fitness = 100
                            },
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "RM",
                                Rating = 80,
                                Fitness = 100
                            }
                        }
                    },
                { PositionalArea.ATT, new List<Card>()
                        {
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "ST",
                                Rating = 80,
                                Fitness = 100
                            },
                            new Card()
                            {
                                Id = Guid.NewGuid(),
                                Name = "ST",
                                Rating = 80,
                                Fitness = 100
                            }
                        }
                    }
                };

            var squad1 = new Squad()
            {
                Id = Guid.NewGuid(),
                Lineup = lineup
            };

            var squad2 = new Squad()
            {
                Id = Guid.NewGuid(),
                Lineup = lineup
            };

            var user1 = Guid.NewGuid();
            var user2 = Guid.NewGuid();

            var match = new Models.Match()
            {
                Id = Guid.NewGuid(),
                KickOff = DateTime.Now,
                User1Id = user1,
                User2Id = user2,
                Team1 = squad1,
                Team2 = squad2
            };
            var result = engine.SimulateReentrant(match);
            var matchResult = new MatchResult(result);
        }
    }
}
