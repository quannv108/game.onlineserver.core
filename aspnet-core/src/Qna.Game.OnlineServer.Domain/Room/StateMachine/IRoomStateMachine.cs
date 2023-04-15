using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.Room.StateMachine;

public interface IRoomStateMachine : ITransientDependency
{
    bool ProcessState(Room match);
}