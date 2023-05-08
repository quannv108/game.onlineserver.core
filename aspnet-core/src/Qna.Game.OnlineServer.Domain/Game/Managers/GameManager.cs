using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Core.Repository;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Game.Managers;

public class GameManager : DomainService, IGameManager
{
    private readonly IRepository<Game, long> _gameRepository;
    private readonly IDistributedCache<Game, GameType> _gameTypeCache;
    private readonly IEntityCache<Game, long> _gameEntityCache;

    public GameManager(IRepository<Game, long> gameRepository,
        IDistributedCache<Game, GameType> gameTypeCache,
        IEntityCache<Game, long> gameEntityCache)
    {
        _gameRepository = gameRepository;
        _gameTypeCache = gameTypeCache;
        _gameEntityCache = gameEntityCache;
    }

    public Task<Game> GetByTypeAsync(GameType gameType)
    {
        return _gameTypeCache.GetOrAddAsync(gameType,
            () => _gameRepository.GetAll().AsNoTracking().SingleOrDefaultAsync(x => x.Type == gameType));
    }

    public Task<Game> GetAsync(long gameId)
    {
        return _gameEntityCache.GetAsync(gameId);
    }

    public Task<List<Game>> GetAllAsync()
    {
        return _gameRepository.GetAll().AsNoTracking().Where(x => x.IsActive).ToListAsync();
    }
}