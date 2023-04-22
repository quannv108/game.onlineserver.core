using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.GamePlay.Managers;

public interface IGamePlayManager : IDomainService
{
    void CreateAndStartGameLoop(Game.Game game, Room.Room room);
    void StopGameLoop(IGameLoop gameLoop);
}