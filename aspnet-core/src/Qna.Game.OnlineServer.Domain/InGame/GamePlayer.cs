using System;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.InGame;

public class GamePlayer : Entity<Guid>
{
    public Guid UserId { get; set; }
    
    public int CurrentLevel { get; set; }

    public GamePlayer(Guid userId)
    {
        UserId = userId;
        Id = Guid.NewGuid();
    }
}