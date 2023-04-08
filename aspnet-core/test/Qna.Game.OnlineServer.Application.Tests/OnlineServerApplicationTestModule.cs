using Volo.Abp.Modularity;

namespace Qna.Game.OnlineServer;

[DependsOn(
    typeof(OnlineServerApplicationModule),
    typeof(OnlineServerDomainTestModule)
    )]
public class OnlineServerApplicationTestModule : AbpModule
{

}
