using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums;

namespace api.Model.Entities;

[Table("plantio")]
public class PlantioDb(int especieId, int hortaId, DateOnly dataPlantio)
{
    [Key]
    public int Id { get; private set; }

    [ForeignKey("Especie")]
    public int EspecieId { get; set; } = especieId;

    [ForeignKey("Horta")]
    public int HortaId { get; set; } = hortaId;

    public DateOnly DataPlantio { get; set; } = dataPlantio;
    public int? Quantidade { get; set; }
    public PLANTIO_STATUS Status { get; set; } = PLANTIO_STATUS.Ativo;
    public DateOnly CreatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

    public EspecieDb? Especie { get; set; }
    public HortaDb? Horta { get; set; }
}