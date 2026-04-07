namespace api.Model.Dtos.Especie;

public class UpdateEspecieDto
{
    public string? Nome { get; set; }
    public string? ImageLink { get; set; }
    public int? DiasParaRegar { get; set; }
    public int? DiasParaColher { get; set; }
    public string? MesesPlantio { get; set; }
    public string? MesesColheita { get; set; }
    public string? Descricao { get; set; }
}