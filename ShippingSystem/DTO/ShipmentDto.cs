using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTO
{
    public class ShipmentDto
    {
        [Required, MaxLength(100)]
        public string ReceiverName { get; set; } = null!;
        [Required, MaxLength(11)]
        public string ReceiverPhone { get; set; } = null!;
        [Required, MaxLength(255), EmailAddress]
        public string ReceiverEmail { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Street { get; set; } = null!;
        [Required, MaxLength(50)]
        public string City { get; set; } = null!;
        [MaxLength(50)]
        public string Country { get; set; } = "Egypt";
        [MaxLength(500)]
        public string? AddressDetails { get; set; }

        [Required, MaxLength(500)]
        public string ShipmentDescription { get; set; } = null!;
        [Required]
        public decimal ShipmentWeight { get; set; }
        [Required]
        public decimal ShipmentLength { get; set; }
        [Required]
        public decimal ShipmentWidth { get; set; }
        [Required]
        public decimal ShipmentHeight { get; set; }
        [MaxLength(500)]
        public string? ShipmentNotes { get; set; }
    }
}
