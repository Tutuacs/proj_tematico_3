using System.ComponentModel.DataAnnotations;
using api.Enums;

namespace api.Model.Dtos.Todo;

public class CreateTodoDto
{
    [Required(ErrorMessage = "Tipo é obrigatório")]
    public required TODO_TIPO Tipo { get; set; }

    public string? Descricao { get; set; }

    [Required(ErrorMessage = "DataLimite é obrigatório")]
    public required DateOnly DataLimite { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PlantioId deve ser maior que zero")]
    public int? PlantioId { get; set; }

    [Required(ErrorMessage = "HortaId é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "HortaId deve ser maior que zero")]
    public int HortaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "MembroId deve ser maior que zero")]
    public int? MembroId { get; set; }
}