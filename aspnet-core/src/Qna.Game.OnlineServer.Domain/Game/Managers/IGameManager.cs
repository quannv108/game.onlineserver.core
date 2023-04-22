using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Game.Managers;

public interface IGameManager : IDomainService
{
    Task<Game> GetByTypeAsync(GameType gameType);
    Task<Game> GetAsync(long gameId);
    Task<List<Game>> GetAllAsync();
}