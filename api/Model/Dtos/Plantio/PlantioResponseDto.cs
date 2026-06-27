namespace api.Model.Dtos.Plantio;

public class PlantioResponseDto
{
    public int Id { get; set; }
    public int EspecieId { get; set; }
    public int HortaId { get; set; }
    public DateOnly DataPlantio { get; set; }
    public int? Quantidade { get; set; }
    public string Status { get; set; } = string.Empty;
    public EspecieResponseDto? Especie { get; set; }
}

public class EspecieResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? ImageLink { get; set; }
    public int DiasParaRegar { get; set; }
    public int? DiasParaColher { get; set; }
    public string? MesesPlantio { get; set; }
    public string? MesesColheita { get; set; }
}
