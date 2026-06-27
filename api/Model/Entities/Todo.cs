using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums;

namespace api.Model.Entities;

[Table("todo")]
public class TodoDb(TODO_TIPO tipo, DateOnly dataLimite, int hortaId)
{
    [Key]
    public int Id { get; private set; }

    public TODO_TIPO Tipo { get; set; } = tipo;
    public string? Descricao { get; set; }
    public DateOnly DataLimite { get; set; } = dataLimite;

    [ForeignKey("Plantio")]
    public int? PlantioId { get; set; }

    [ForeignKey("Horta")]
    public int HortaId { get; set; } = hortaId;

    [ForeignKey("Membro")]
    public int? MembroId { get; set; }

    public DateOnly? CompletedAt { get; set; }
    public TODO_STATUS Status { get; set; } = TODO_STATUS.Aberto;
    public DateOnly CreatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

    // Navigation properties
    public PlantioDb? Plantio { get; set; }
    public HortaDb? Horta { get; set; }
    public MembroDb? Membro { get; set; }
}