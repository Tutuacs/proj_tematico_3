using api.Enums;

namespace api.Model.Dtos.Plantio;

public class UpdatePlantioDto
{
    public int? EspecieId { get; set; }
    public int? HortaId { get; set; }
    public DateOnly? DataPlantio { get; set; }
    public int? Quantidade { get; set; }
    public PLANTIO_STATUS? Status { get; set; }
}