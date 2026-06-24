using api.Enums;

namespace api.Model.Dtos.Todo;

public class UpdateTodoDto
{
    public TODO_TIPO? Tipo { get; set; }
    public string? Descricao { get; set; }
    public DateOnly? DataLimite { get; set; }
    public int? PlantioId { get; set; }
    public int? MembroId { get; set; }
    public DateOnly? CompletedAt { get; set; }
    public TODO_STATUS? Status { get; set; }
}