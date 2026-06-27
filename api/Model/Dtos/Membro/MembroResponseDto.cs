namespace api.Model.Dtos.Membro;

public class MembroResponseDto
{
    public int Id { get; set; }
    public int HortaId { get; set; }
    public string PerfilId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public ProfileResponseDto? Profile { get; set; }
    public HortaResponseDto? Horta { get; set; }
}

public class ProfileResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class HortaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Local { get; set; } = string.Empty;
}
