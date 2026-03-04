using System.ComponentModel.DataAnnotations;

namespace api.Model.Dtos.Horta;

public class CreateHortaDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public required string Nome { get; set; }
    
    public string? Descricao { get; set; }
    
    [Required(ErrorMessage = "Local é obrigatório")]
    public required string Local { get; set; }
    
    [Required(ErrorMessage = "Largura é obrigatória")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Largura deve ser maior que zero")]
    public decimal Largura { get; set; }
    
    [Required(ErrorMessage = "Profundidade é obrigatória")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Profundidade deve ser maior que zero")]
    public decimal Profundidade { get; set; }
}
