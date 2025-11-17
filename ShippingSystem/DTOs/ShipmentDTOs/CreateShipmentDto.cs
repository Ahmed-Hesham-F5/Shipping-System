using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.Validators;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.ShipmentDTOs
{
    public class CreateShipmentDto
    {
        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string CustomerPhone { get; set; } = null!;
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string? CustomerAdditionalPhone { get; set; } = null;
        [MaxLength(255), EmailAddress]
        public string? CustomerEmail { get; set; } = null!;
        [Required]
        public AddressDto CustomerAddress { get; set; } = null!;

        [Required, MaxLength(500)]
        public string ShipmentDescription { get; set; } = null!;

        [Range(0.0, double.MaxValue, ErrorMessage = "Shipment weight must be greater than 0.")]
        public decimal ShipmentWeight { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [MaxLength(500)]
        public string? ShipmentNotes { get; set; }

        public bool CashOnDeliveryEnabled { get; set; }
        public bool OpenPackageOnDeliveryEnabled { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }

        [ValueRequiredIfCod("CashOnDeliveryEnabled", ErrorMessage = "CollectionAmount is required when CashOnDeliveryEnabled is true and must be greater than 0.")]
        public decimal CollectionAmount { get; set; }
        public bool IsDelivered { get; set; }
    }
}
