using ShippingSystem.Validators;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs
{
    public class UpdateShipmentDto
    {
        [Required, MaxLength(100)]
        public string ReceiverName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string ReceiverPhone { get; set; } = null!;
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string? ReceiverAdditionalPhone { get; set; } = null;
        [Required, MaxLength(255), EmailAddress]
        public string ReceiverEmail { get; set; } = null!;
        [Required, MaxLength(256)]
        public string Street { get; set; } = null!;
        [Required, MaxLength(50)]
        public string City { get; set; } = null!;
        [Required, MaxLength(50)]
        public string Governorate { get; set; } = null!;
        [MaxLength(500)]
        public string? AddressDetails { get; set; }
        [MaxLength(2083), Url]
        public string? GoogleMapAddressLink { get; set; }

        [Required, MaxLength(500)]
        public string ShipmentDescription { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment weight must be greater than 0.")]
        public decimal ShipmentWeight { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment length must be greater than 0.")]
        public decimal ShipmentLength { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment width must be greater than 0.")]
        public decimal ShipmentWidth { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment height must be greater than 0.")]
        public decimal ShipmentHeight { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [MaxLength(500)]
        public string? ShipmentNotes { get; set; }
        public bool CashOnDeliveryEnabled { get; set; }
        public bool OpenPackageOnDeliveryEnabled { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }

        [ValueRequiredIfCod("CashOnDeliveryEnabled", ErrorMessage = "CollectionAmount is required when CashOnDeliveryEnabled is true and must be greater than 0.")]
        public decimal CollectionAmount { get; set; }
    }
}
