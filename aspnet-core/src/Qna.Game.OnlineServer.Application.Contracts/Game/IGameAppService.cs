using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Game.Dto;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.Game;

public interface IGameAppService : IApplicationService
{
    Task<List<GameDto>> GetAllAsync();
}