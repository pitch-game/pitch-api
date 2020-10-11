using System;
using System.Collections.Generic;
using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.MatchResult;
using Pitch.Match.API.Models;
using Lineup = Pitch.Match.API.ApplicationCore.Models.MatchResult.Lineup;
using Squad = Pitch.Match.API.ApplicationCore.Models.MatchResult.Squad;

namespace Pitch.Match.API.Tests.Integration
{
    public class MatchResponseFixtures
    {
        public MatchResultModel DefaultMatchResultModel => new MatchResultModel
        {
            Minute = 34,
            HomeResult = new Result
            {
                Name = "Default FC",
                Score = 1,
                Scorers = new[]
                {
                    "Jimmy Johnson 20'"
                }
            },
            AwayResult = new Result
            {
                Name = "Evil FC",
                Score = 0,
                Scorers = new string[0]
            },
            HomeStats = new Stats
            {
                Shots = 2,
                ShotsOnTarget = 2,
                Fouls = 0,
                Possession = 100,
                RedCards = 0,
                YellowCards = 0
            },
            AwayStats = new Stats
            {
                Shots = 0,
                ShotsOnTarget = 0,
                Fouls = 0,
                Possession = 0,
                RedCards = 0,
                YellowCards = 0
            },
            Expired = false,
            ExpiredOn = null,
            Lineup = new Lineup
            {
                Home = new Squad
                {
                    Lineup = new Dictionary<string, IEnumerable<Guid>>
                    {
                        {"ST", new[] {TestConstants.DefaultHomeActiveCardId}}
                    },
                    Subs = new[]
                    {
                        TestConstants.DefaultHomeSubCardId
                    }
                },
                Away = new Squad
                {
                    Lineup = new Dictionary<string, IEnumerable<Guid>>
                    {
                        {"ST", new[] {TestConstants.DefaultAwayActiveCardId}}
                    },
                    Subs = new[]
                    {
                        TestConstants.DefaultAwaySubCardId
                    }
                }
            },
            Timeline = new List<Event>
            {
                new Event
                {
                    CardId = TestConstants.DefaultHomeActiveCardId,
                    Minute = 20,
                    Name = "Goal",
                    SquadName = "Default FC"
                },
                new Event
                {
                    CardId = TestConstants.DefaultHomeActiveCardId,
                    Minute = 0,
                    Name = "Shot Saved",
                    SquadName = "Default FC"
                }
            },
            CardLookup = new Dictionary<string, Card>
            {
                {
                    TestConstants.DefaultHomeActiveCardId.ToString(), new Card
                    {
                        Id = TestConstants.DefaultHomeActiveCardId,
                        Name = "Jimmy Johnson",
                        Rating = 50,
                        Fitness = 100,
                        Goals = 1
                    }
                },
                {
                    TestConstants.DefaultHomeSubCardId.ToString(), new Card
                    {
                        Id = TestConstants.DefaultHomeSubCardId,
                        Rating = 50,
                        Fitness = 100
                    }
                },
                {
                    TestConstants.DefaultAwayActiveCardId.ToString(), new Card
                    {
                        Id = TestConstants.DefaultAwayActiveCardId,
                        Rating = 50,
                        Fitness = 100
                    }
                },
                {
                    TestConstants.DefaultAwaySubCardId.ToString(), new Card
                    {
                        Id = TestConstants.DefaultAwaySubCardId,
                        Rating = 50,
                        Fitness = 100
                    }
                }
            }
        };

        public LineupModel DefaultLineupModel => new LineupModel
        {
            Active = new List<Card>
            {
                new Card
                {
                    Id = TestConstants.DefaultHomeActiveCardId,
                    Name = "Jimmy Johnson",
                    Rating = 50,
                    Fitness = 100,
                }
            },
            Subs = new[]
            {
                new Card
                {
                    Id = TestConstants.DefaultHomeSubCardId,
                    Rating = 50,
                    Fitness = 100
                }
            }
        };
    }
}