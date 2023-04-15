using System;

namespace Qna.Game.OnlineServer.Room.Events;

public class RoomDestroyedEvent
{
    public Guid RoomId { get; set; }
}