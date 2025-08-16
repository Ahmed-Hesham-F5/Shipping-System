using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [PersonalData]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

      
    }
}
