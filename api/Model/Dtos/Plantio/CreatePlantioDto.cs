using System.ComponentModel.DataAnnotations;

namespace api.Model.Dtos.Plantio;

public class CreatePlantioDto
{
    [Required(ErrorMessage = "EspecieId é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "EspecieId deve ser um número positivo")]
    public int EspecieId { get; set; }

    [Required(ErrorMessage = "HortaId é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "HortaId deve ser um número positivo")]
    public int HortaId { get; set; }

    [Required(ErrorMessage = "DataPlantio é obrigatória")]
    public DateOnly DataPlantio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public int? Quantidade { get; set; }
}