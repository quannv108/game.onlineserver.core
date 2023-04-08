using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace Qna.Game.OnlineServer.SignalR;

[DependsOn(typeof(AbpAspNetCoreSignalRModule))]
public class OnlineServerSignalRModule : AbpModule
{
    
}