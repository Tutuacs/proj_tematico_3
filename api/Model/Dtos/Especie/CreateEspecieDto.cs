using System.ComponentModel.DataAnnotations;

namespace api.Model.Dtos.Especie;

public class CreateEspecieDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public required string Nome { get; set; }

    public string? ImageLink { get; set; } = "https://static.vecteezy.com/system/resources/thumbnails/019/534/015/small/realistic-foliage-isolated-on-transparent-background-3d-rendering-illustration-png.png";

    [Required(ErrorMessage = "DiasParaRegar é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "DiasParaRegar deve ser maior que zero")]
    public int DiasParaRegar { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "DiasParaColher deve ser maior que zero")]
    public int? DiasParaColher { get; set; }

    public string? MesesPlantio { get; set; }
    public string? MesesColheita { get; set; }
    public string? Descricao { get; set; }
}