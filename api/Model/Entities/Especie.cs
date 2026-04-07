using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model.Entities;

[Table("especie")]
public class EspecieDb(string nome, int diasParaRegar)
{
    [Key]
    public int Id { get; private set; }

    public string Nome { get; set; } = nome;
    public string? ImageLink { get; set; }
    public int DiasParaRegar { get; set; } = diasParaRegar;
    public int? DiasParaColher { get; set; }
    public string? MesesPlantio { get; set; }
    public string? MesesColheita { get; set; }
    public string? Descricao { get; set; }
}