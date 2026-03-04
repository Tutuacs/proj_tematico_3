using System.ComponentModel.DataAnnotations;

namespace api.Model.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [MinLength(3)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}