using Qna.Game.OnlineServer.Room.Storage;
using Qna.Game.OnlineServer.SignalR.Match;
using Qna.Game.OnlineServer.SignalR.Session;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Qna.Game.OnlineServer.SignalR;

[DependsOn(
    typeof(OnlineServerDomainModule),
    typeof(AbpAspNetCoreSignalRModule)
    )]
public class OnlineServerSignalRModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OnlineServerSignalRModule>();
        });
        
        // context.Services.AddSingleton<IMatchService, MatchService>();
        // context.Services.AddSingleton<ISessionService, SessionService>();
    }
    
    
}