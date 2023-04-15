using System;
using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.Room.Helpers;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Room.StateMachine;

public class RoomStateMachine : DomainService, IRoomStateMachine
{
    public bool ProcessState(Room match)
    {
        switch (match.State)
        {
            case RoomState.Matching:
                if (match.IsEnoughForStartGame())
                {
                    match.State = RoomState.ReadyForPlay;
                    return true;
                }
                break;
            case RoomState.Playing:
                break;
            case RoomState.Ended:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}