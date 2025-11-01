using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class UserPhone
    {
        public string PhoneNumber { get; set; } = null!;
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User{ get; set; } = null!;
    }
}