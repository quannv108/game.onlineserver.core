using Qna.Game.OnlineServer.Localization;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer;

/* Inherit your application services from this class.
 */
public abstract class OnlineServerAppService : ApplicationService
{
    protected OnlineServerAppService()
    {
        LocalizationResource = typeof(OnlineServerResource);
    }
}
