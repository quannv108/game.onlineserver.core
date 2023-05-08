using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Qna.Game.OnlineServer.Core.Repository;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Qna.Game.OnlineServer.GamePlay.Players.Managers;

public class GamePlayerManager : DomainService, IGamePlayerManager
{
    private readonly IRepository<GamePlayer, Guid> _gamePlayerRepository;
    private readonly IDistributedCache<GamePlayer, Guid> _nearestPlayerCache;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public GamePlayerManager(IRepository<GamePlayer, Guid> gamePlayerRepository,
        IDistributedCache<GamePlayer, Guid> nearestPlayerCache,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _gamePlayerRepository = gamePlayerRepository;
        _nearestPlayerCache = nearestPlayerCache;
        _unitOfWorkManager = unitOfWorkManager;
    }
    
    public Task<List<GamePlayer>> GetAllAsync(Guid userId, long gameId)
    {
        return _gamePlayerRepository.GetAll().AsNoTracking()
            .Where(g => g.UserId == userId && g.GameId == gameId)
            .OrderBy(g => g.LastModificationTime ?? g.CreationTime)
            .ToListAsync();
    }

    public Task<GamePlayer> GetNearestPlayingAsync(Guid userId, long gameId)
    {
        return _nearestPlayerCache.GetOrAddAsync(userId,
            () => _gamePlayerRepository.GetAll().AsNoTracking()
                .Where(g => g.UserId == userId && g.GameId == gameId)
                .OrderBy(g => g.LastModificationTime ?? g.CreationTime)
                .FirstOrDefaultAsync(),
            () => new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            }, considerUow:true);
    }

    public async Task CreateAsync(Guid userId, long gameId)
    {
        var gamePlayer = new GamePlayer
        {
            UserId = userId,
            GameId = gameId,
            CurrentLevel = 1    
        };
        await _gamePlayerRepository.InsertAsync(gamePlayer);
        await _unitOfWorkManager.Current.SaveChangesAsync();
    }

    public Task CreateOneIfRequiredAsync(Guid userId, Game.Game game)
    {
        return game.MinPlayer == 0 ? Task.CompletedTask : CreateAsync(userId, game.Id);
    }
}