using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.GamePlay.Players.Managers;

public interface IGamePlayerManager : IDomainService
{
    Task<List<GamePlayer>> GetAllAsync(Guid userId, long gameId);
    Task<GamePlayer> GetNearestPlayingAsync(Guid userId, long gameId);
    Task CreateAsync(Guid userId, long gameId);
    Task CreateOneIfRequiredAsync(Guid userId, Game.Game game);
}