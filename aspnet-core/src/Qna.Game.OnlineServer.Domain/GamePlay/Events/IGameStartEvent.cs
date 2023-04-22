using System;

namespace Qna.Game.OnlineServer.GamePlay.Events;

public interface IGameStartEvent
{
    Guid GamePlayId { get; set; }
    string RoomName { get; set; }
}