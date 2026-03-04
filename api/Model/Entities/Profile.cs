using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model.Entities;

[Table("profile")]
public class ProfileDb(string name, string email, string password)
{
    [Key]
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public DateOnly CreatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
}