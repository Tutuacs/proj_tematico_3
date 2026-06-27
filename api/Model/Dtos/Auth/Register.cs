using System.ComponentModel.DataAnnotations;

namespace api.Model.Dtos.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }
        
        [Required]
        [MinLength(3)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}