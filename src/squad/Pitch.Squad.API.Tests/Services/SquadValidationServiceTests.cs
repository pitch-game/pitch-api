using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using FluentAssertions;
using Moq;
using Pitch.Squad.API.Application.Requests;
using Pitch.Squad.API.Application.Response;
using Pitch.Squad.API.Services;
using Pitch.Squad.API.Tests.Builders;
using Xunit;

namespace Pitch.Squad.API.Tests.Services
{
    public class SquadValidationServiceTests
    {
        private readonly Mock<IBus> _busMock;

        public SquadValidationServiceTests()
        {
            _busMock = new Mock<IBus>();
        }

        [Fact]
        public async Task Return_True_When_Squad_Is_Valid()
        {
            var squadBuilder = new SquadBuilder();
            var squad = squadBuilder.WithDefaults().Build();

            var getCardsResponseBuilder = new GetCardsResponseBuilder();
            var getCardsResponse = getCardsResponseBuilder
                .WithDefaultCard(squadBuilder.DefaultGkId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRcbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLstId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRstId, squad.UserId)
                .Build();

            _busMock.Setup(x => x.RequestAsync<GetCardsRequest, GetCardsResponse>(It.IsAny<GetCardsRequest>()))
                .ReturnsAsync(getCardsResponse);

            var squadValidationService = new SquadValidationService(_busMock.Object);
            var isValid = await squadValidationService.Validate(squad, squad.Id, squad.UserId);

            isValid.Should().BeTrue();
        }

        [Fact]
        public async Task Return_False_When_Squad_Ids_Dont_Match()
        {
            var squadBuilder = new SquadBuilder();
            var squad = squadBuilder
                .WithId(Guid.NewGuid())
                .Build();

            var squadValidationService = new SquadValidationService(_busMock.Object);
            var isValid = await squadValidationService.Validate(squad, Guid.NewGuid(), "1");

            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Return_False_When_Squad_Contains_Duplicates()
        {
            var duplicateId = Guid.NewGuid();

            var squadBuilder = new SquadBuilder();
            var squad = squadBuilder
                .WithDefaults()
                .SetPosition("LST", duplicateId)
                .SetPosition("RST", duplicateId)
                .Build();

            var getCardsResponseBuilder = new GetCardsResponseBuilder();
            var getCardsResponse = getCardsResponseBuilder
                .WithDefaultCard(squadBuilder.DefaultGkId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRcbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLstId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRstId, squad.UserId)
                .WithDefaultCard(duplicateId, squad.UserId)
                .Build();

            _busMock.Setup(x => x.RequestAsync<GetCardsRequest, GetCardsResponse>(It.IsAny<GetCardsRequest>()))
               .ReturnsAsync(getCardsResponse);
            
            var squadValidationService = new SquadValidationService(_busMock.Object);
            var isValid = await squadValidationService.Validate(squad, squad.Id, squad.UserId);

            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Return_False_When_Card_Doesnt_Belong_To_User()
        {
            var squadBuilder = new SquadBuilder();
            var squad = squadBuilder
                .WithDefaults()
                .Build();

            var getCardsResponseBuilder = new GetCardsResponseBuilder();
            var getCardsResponse = getCardsResponseBuilder
                .WithDefaultCard(squadBuilder.DefaultGkId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRcbId, Guid.NewGuid().ToString())
                .WithDefaultCard(squadBuilder.DefaultRbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLstId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRstId, squad.UserId)
                .Build();

            _busMock.Setup(x => x.RequestAsync<GetCardsRequest, GetCardsResponse>(It.IsAny<GetCardsRequest>()))
                .ReturnsAsync(getCardsResponse);

            var squadValidationService = new SquadValidationService(_busMock.Object);
            var isValid = await squadValidationService.Validate(squad, squad.Id, squad.UserId);

            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Return_False_When_Card_Doesnt_Exist()
        {
            var squadBuilder = new SquadBuilder();
            var squad = squadBuilder
                .WithDefaults()
                .Build();

            var getCardsResponseBuilder = new GetCardsResponseBuilder();
            var getCardsResponse = getCardsResponseBuilder
                .WithDefaultCard(squadBuilder.DefaultGkId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRbId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRcmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRmId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultLstId, squad.UserId)
                .WithDefaultCard(squadBuilder.DefaultRstId, squad.UserId)
                .Build();

            _busMock.Setup(x => x.RequestAsync<GetCardsRequest, GetCardsResponse>(It.IsAny<GetCardsRequest>()))
                .ReturnsAsync(getCardsResponse);

            var squadValidationService = new SquadValidationService(_busMock.Object);
            var isValid = await squadValidationService.Validate(squad, squad.Id, squad.UserId);

            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Return_False_When_Squad_Is_Incomplete()
        {
            var squadBuilder = new SquadBuilder();
            var cardId = Guid.NewGuid();

            var squad = squadBuilder
                .WithDefaults()
                .WithLineup(new Dictionary<string, Guid?>()
                {
                    {"INV", cardId}
                })
                .Build();

            var getCardsResponseBuilder = new GetCardsResponseBuilder();
            var getCardsResponse = getCardsResponseBuilder
                .WithDefaultCard(cardId, squad.UserId)
                .Build();

            _busMock.Setup(x => x.RequestAsync<GetCardsRequest, GetCardsResponse>(It.IsAny<GetCardsRequest>()))
                .ReturnsAsync(getCardsResponse);

            var squadValidationService = new SquadValidationService(_busMock.Object);
            var isValid = await squadValidationService.Validate(squad, squad.Id, squad.UserId);

            isValid.Should().BeFalse();
        }
    }
}