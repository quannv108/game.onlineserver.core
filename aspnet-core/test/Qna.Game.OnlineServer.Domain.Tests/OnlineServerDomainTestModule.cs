using Qna.Game.OnlineServer.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Qna.Game.OnlineServer;

[DependsOn(
    typeof(OnlineServerEntityFrameworkCoreTestModule)
    )]
public class OnlineServerDomainTestModule : AbpModule
{

}
