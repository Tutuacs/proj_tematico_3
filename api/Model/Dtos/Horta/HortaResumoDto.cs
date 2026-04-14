using api.Enums;

namespace api.Model.Dtos.Horta;

public class HortaResumoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Local { get; set; } = string.Empty;
    public decimal Largura { get; set; }
    public decimal Profundidade { get; set; }
    public STATUS Status { get; set; }
    public DateOnly CreatedAt { get; set; }
    public int CountMembros { get; set; }
    public int CountPlantios { get; set; }
}