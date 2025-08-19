using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTO
{
    public class LoginDto
    {
        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password), MinLength(8), MaxLength(50)]
        public string Password { get; set; } = null!;
    }
}
