using System;
using System.Collections.Generic;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Engine.Actions;
using Pitch.Match.API.ApplicationCore.Engine.Providers;
using Pitch.Match.API.ApplicationCore.Engine.Services;
using Pitch.Match.API.ApplicationCore.Models;

namespace Pitch.Match.API.Tests.Engine
{
    public abstract class MatchTestBase
    {
        protected Card StubAwayPlayer;
        protected Card StubAwaySub;

        protected Card StubHomePlayer;

        protected Squad StubHomeSquad;
        protected Squad StubAwaySquad;
        protected Card StubHomeSub;

        protected Guid StubHomeUserId;

        protected TeamDetails StubHomeTeamDetails;

        protected ApplicationCore.Models.Match StubMatch;
        protected MatchEngine StubMatchEngine;

        protected MatchTestBase()
        {
            var randomnessProvider = new ThreadSafeRandomnessProvider();
            var actions = new IAction[] {new Foul(randomnessProvider), new Shot(randomnessProvider) };
            var actionService = new ActionService(actions, randomnessProvider);
            StubMatchEngine = new MatchEngine(actionService);

            StubHomePlayer = new Card
            {
                Id = Guid.NewGuid(),
                Name = "GK",
                Rating = 80,
                Fitness = 100
            };

            StubHomeSub = new Card
            {
                Id = Guid.NewGuid(),
                Name = "ST",
                Rating = 40,
                Fitness = 100
            };

            StubAwayPlayer = new Card
            {
                Id = Guid.NewGuid(),
                Name = "ST",
                Rating = 40,
                Fitness = 100
            };

            StubAwaySub = new Card
            {
                Id = Guid.NewGuid(),
                Name = "ST",
                Rating = 40,
                Fitness = 100
            };

            var stubHomeTeamLineup = new Dictionary<string, IEnumerable<Card>>
            {
                {
                    "GK", new List<Card>
                    {
                        StubHomePlayer
                    }
                },
                {
                    "DEF", new List<Card>
                    {
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "LB",
                            Rating = 80,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CB",
                            Rating = 80,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CB",
                            Rating = 80,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "RB",
                            Rating = 80,
                            Fitness = 100
                        }
                    }
                },
                {
                    "MID", new List<Card>
                    {
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "LM",
                            Rating = 80,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CM",
                            Rating = 80,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CM",
                            Rating = 80,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "RM",
                            Rating = 80,
                            Fitness = 100
                        }
                    }
                },
                {
                    "ATT", new List<Card>
                    {
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "ST",
                            Rating = 80,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "ST",
                            Rating = 80,
                            Fitness = 100
                        }
                    }
                }
            };

            var stubAwayTeamLineup = new Dictionary<string, IEnumerable<Card>>
            {
                {
                    "GK", new List<Card>
                    {
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "GK",
                            Rating = 40,
                            Fitness = 100
                        }
                    }
                },
                {
                    "DEF", new List<Card>
                    {
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "LB",
                            Rating = 40,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CB",
                            Rating = 40,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CB",
                            Rating = 40,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "RB",
                            Rating = 40,
                            Fitness = 100
                        }
                    }
                },
                {
                    "MID", new List<Card>
                    {
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "LM",
                            Rating = 40,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CM",
                            Rating = 40,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "CM",
                            Rating = 40,
                            Fitness = 100
                        },
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "RM",
                            Rating = 40,
                            Fitness = 100
                        }
                    }
                },
                {
                    "ATT", new List<Card>
                    {
                        StubAwayPlayer,
                        new Card
                        {
                            Id = Guid.NewGuid(),
                            Name = "ST",
                            Rating = 40,
                            Fitness = 100
                        }
                    }
                }
            };

            StubHomeSquad = new Squad
            {
                Id = Guid.NewGuid(),
                Lineup = stubHomeTeamLineup,
                Subs = new[]
                {
                    StubHomeSub
                },
                Name = "Good FC"
            };

            StubAwaySquad = new Squad
            {
                Id = Guid.NewGuid(),
                Lineup = stubAwayTeamLineup,
                Subs = new[]
                {
                    StubAwaySub
                },
                Name = "Shitty FC"
            };

            StubHomeUserId = Guid.NewGuid();
            var stubAwayUserId = Guid.NewGuid();

            StubHomeTeamDetails = new TeamDetails
            {
                UserId = StubHomeUserId,
                Squad = StubHomeSquad
            };

            StubMatch = new ApplicationCore.Models.Match
            {
                Id = Guid.NewGuid(),
                KickOff = DateTime.Now,
                HomeTeam = StubHomeTeamDetails,
                AwayTeam = new TeamDetails
                {
                    UserId = stubAwayUserId,
                    Squad = StubAwaySquad
                }
            };
        }

        protected void SimulateStubMatch()
        {
            StubMatch = StubMatchEngine.SimulateReentrant(StubMatch);
        }
    }
}