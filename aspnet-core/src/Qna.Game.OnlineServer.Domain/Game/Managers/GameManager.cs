using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Repository;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Game.Managers;

public class GameManager : DomainService, IGameManager
{
    private readonly IRepository<Game, long> _gameRepository;

    public GameManager(IRepository<Game, long> gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public Task<Game> GetByTypeAsync(GameType gameType)
    {
        return _gameRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Type == gameType);
    }

    public Task<Game> GetAsync(long gameId)
    {
        return _gameRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.Id == gameId);
    }

    public Task<List<Game>> GetAllAsync()
    {
        return _gameRepository.GetAll().AsNoTracking().Where(x => x.IsActive).ToListAsync();
    }
}