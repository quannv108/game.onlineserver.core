using System;
using Qna.Game.OnlineServer.Game;

namespace Qna.Game.OnlineServer.Room.Events;

public class RoomStateChangedEvent
{
    public Guid RoomId { get; set; }
    public string RoomName { get; set; }
    public RoomState NewState { get; set; }
}