using Qna.Game.OnlineServer.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Qna.Game.OnlineServer.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OnlineServerEntityFrameworkCoreModule),
    typeof(OnlineServerApplicationContractsModule)
    )]
public class OnlineServerDbMigratorModule : AbpModule
{

}
