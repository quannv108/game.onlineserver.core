using Qna.Game.OnlineServer.GamePlay.TicTacToe;
using Qna.Game.OnlineServer.Session;
using Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.SignalR.GamePlay.TicTacToe;

public interface IGamePlayService : IApplicationService
{
    GameLoop GetGameLoop(Guid roomId);
    public Task PutMarkAsync(UserConnectionSession session, PutMarkToBoardInput toBoardInput);
}