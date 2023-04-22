using System;

namespace Qna.Game.OnlineServer.GamePlay;

public interface IGameLoop<T, TS> : IGameLoop
    where T : class, IGamePlayData
    where TS : struct, Enum
{
    TS State { get; }
    T GamePlayData { get; }
}

public interface IGameLoop
{
    Guid Id { get; }
    Room.Room Room { get; }
    bool IsFinished { get; }
    void Setup(Room.Room room);
    void Start();
    void AbortGame();
}