using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Schema;
using Volo.Abp.Domain.Entities;

namespace Qna.Game.OnlineServer.Game;

[Table("Game", Schema = SchemaNames.Game)]
[Index(nameof(IsActive))]
public class Game : Entity<long>
{
    [MaxLength(GameConsts.NameMaxLength)]
    public string Name { get; set; }
    
    public GameType Type { get; set; }
    public bool IsActive { get; set; }
    
    public short MinPlayer { get; set; }
    public short MaxPlayer { get; set; }
}