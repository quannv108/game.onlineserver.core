using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Game;

[Table("Game", Schema = SchemaNames.Game)]
[Index(nameof(IsActive))]
public class Game : Entity<long>, IEquatable<Game>
{
    [MaxLength(GameConsts.NameMaxLength)]
    public string Name { get; set; }
    
    public GameType Type { get; set; }
    public bool IsActive { get; set; }

    public bool Equals(Game other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name && Type == other.Type && IsActive == other.IsActive;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((Game)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, (int)Type, IsActive);
    }
}