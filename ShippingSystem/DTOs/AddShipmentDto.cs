using ShippingSystem.Validators;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs
{
    public class AddShipmentDto
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
        public decimal? CollectionAmount { get; set; }
    }
}
