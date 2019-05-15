using AutoMapper;
using Pitch.Player.Api.Application.Requests;
using Pitch.Player.Api.Application.Responses;
using Pitch.Player.Api.Models;
using Pitch.Player.Api.Services;

namespace Pitch.Player.Api.Application.Responders
{
    public interface IPlayerRequestResponder
    {
        PlayerResponse Response(PlayerRequest request);
    }

    public class PlayerRequestResponder : IPlayerRequestResponder
    {
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;
        public PlayerRequestResponder(IPlayerService playerService, IMapper mapper)
        {
            _playerService = playerService;
            _mapper = mapper;
        }

        public PlayerResponse Response(PlayerRequest @request)
        {
            PlayerRequestModel req = _mapper.Map<PlayerRequestModel>(request);
            var player = _playerService.GetRandom(req);
            return _mapper.Map<PlayerResponse>(player);
        }
    }
}
