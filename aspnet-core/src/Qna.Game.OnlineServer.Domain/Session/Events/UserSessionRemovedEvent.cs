using System;

namespace Qna.Game.OnlineServer.Session.Events;

public class UserSessionRemovedEvent
{
    public Guid UserId { get; set; }
    public string ConnectionId { get; set; }
    public ConnectionSessionDestroyReason Reason { get; set; }
}