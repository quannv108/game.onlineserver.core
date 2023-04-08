using Qna.Game.OnlineServer.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Qna.Game.OnlineServer.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OnlineServerController : AbpControllerBase
{
    protected OnlineServerController()
    {
        LocalizationResource = typeof(OnlineServerResource);
    }
}
