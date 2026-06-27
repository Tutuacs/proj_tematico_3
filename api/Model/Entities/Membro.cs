using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums;

namespace api.Model.Entities;

[Table("membro")]
public class MembroDb(int hortaId, string perfilId, ROLE role)
{
    [Key]
    public int Id { get; private set; }
    
    [ForeignKey("Horta")]
    public int HortaId { get; set; } = hortaId;
    
    [ForeignKey("Profile")]
    public string PerfilId { get; set; } = perfilId;
    
    public ROLE Role { get; set; } = role;
    
    // Navigation properties
    public HortaDb? Horta { get; set; }
    public ProfileDb? Profile { get; set; }
}
