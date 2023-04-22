using Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Hub.TicTacToe;
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

        #region TicTacToe

        context.Services.AddSingleton<MatchService<Hub.TicTacToe.TicTacToeHub, ITicTacToeClientAction>>();
        context.Services.AddSingleton<SessionService<Hub.TicTacToe.TicTacToeHub, ITicTacToeClientAction>>();
        context.Services.AddSingleton<GamePlay.TicTacToe.IGamePlayService, GamePlay.TicTacToe.GamePlayService>();

        #endregion
    }
    
    
}