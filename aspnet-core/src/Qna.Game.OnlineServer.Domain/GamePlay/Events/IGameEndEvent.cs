using System;

namespace Qna.Game.OnlineServer.GamePlay.Events;

public interface IGameEndEvent
{
    Guid RoomId { get; set; }
    string RoomName { get; set; }
}