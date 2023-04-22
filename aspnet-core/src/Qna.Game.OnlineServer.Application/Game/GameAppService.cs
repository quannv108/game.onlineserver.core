using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Game.Dto;
using Qna.Game.OnlineServer.Game.Managers;

namespace Qna.Game.OnlineServer.Game;

public class GameAppService : OnlineServerAppService, IGameAppService
{
    private readonly IGameManager _gameManager;

    public GameAppService(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    public async Task<List<GameDto>> GetAllAsync()
    {
        var games = await _gameManager.GetAllAsync();
        return ObjectMapper.Map<List<Game>, List<GameDto>>(games);
    }
}