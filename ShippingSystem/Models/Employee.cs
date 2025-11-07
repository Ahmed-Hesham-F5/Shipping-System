using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class Employee
    {
        [Key, ForeignKey("User")]
        public string EmployeeId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public bool FirstLogin { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public int? HubId { get; set; }
        [ForeignKey("HubId")]
        public Hub? Hub { get; set; }
    }
}
