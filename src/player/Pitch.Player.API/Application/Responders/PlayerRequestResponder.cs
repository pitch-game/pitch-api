using AutoMapper;
using EasyNetQ;
using Pitch.Player.API.Application.Requests;
using Pitch.Player.API.Application.Responses;
using Pitch.Player.API.Models;
using Pitch.Player.API.Services;

namespace Pitch.Player.API.Application.Responders
{
    public interface IPlayerRequestResponder : IResponder
    {
        PlayerResponse Response(PlayerRequest request);
    }

    public class PlayerRequestResponder : IPlayerRequestResponder
    {
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        public PlayerRequestResponder(IPlayerService playerService, IMapper mapper, IBus bus)
        {
            _playerService = playerService;
            _mapper = mapper;
            _bus = bus;
        }

        public void Register()
        {
            _bus.Respond<PlayerRequest, PlayerResponse>(Response);
        }

        public PlayerResponse Response(PlayerRequest @request)
        {
            PlayerRequestModel req = _mapper.Map<PlayerRequestModel>(request);
            var player = _playerService.GetRandom(req);
            return _mapper.Map<PlayerResponse>(player);
        }
    }
}
