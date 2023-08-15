using System.Threading.Tasks;
using Qna.Game.OnlineServer.Version.Dto;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.Version;

public interface IVersionAppService : IApplicationService
{
    Task<VersionDto> GetAsync();
}