using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Qna.Game.OnlineServer;

[Dependency(ReplaceServices = true)]
public class OnlineServerBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OnlineServer";
}
