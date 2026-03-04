using System.ComponentModel.DataAnnotations;
using api.Enums;

namespace api.Model.Dtos.Membro;

public class AddMembroDto
{
    [Required(ErrorMessage = "HortaId é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "HortaId deve ser um número positivo")]
    public int HortaId { get; set; }
    
    [Required(ErrorMessage = "PerfilId é obrigatório")]
    public required string PerfilId { get; set; }
    
    [Required(ErrorMessage = "Role é obrigatória")]
    public ROLE Role { get; set; } = ROLE.Membro;
}
