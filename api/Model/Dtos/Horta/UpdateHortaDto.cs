using api.Enums;

namespace api.Model.Dtos.Horta;

public class UpdateHortaDto
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public string? Local { get; set; }
    public decimal? Largura { get; set; }
    public decimal? Profundidade { get; set; }
    public STATUS? Status { get; set; }
}
