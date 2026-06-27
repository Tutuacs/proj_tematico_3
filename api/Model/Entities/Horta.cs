using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums;

namespace api.Model.Entities;

[Table("horta")]
public class HortaDb(string nome, string local, decimal largura, decimal profundidade)
{
    [Key]
    public int Id { get; private set; }
    public string Nome { get; set; } = nome;
    public string? Descricao { get; set; }
    public string Local { get; set; } = local;
    public decimal Largura { get; set; } = largura;
    public decimal Profundidade { get; set; } = profundidade;
    public STATUS Status { get; set; } = STATUS.Ativa;
    public DateOnly CreatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
}
