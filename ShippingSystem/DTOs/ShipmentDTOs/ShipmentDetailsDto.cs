using ShippingSystem.DTOs.AddressDTOs;

namespace ShippingSystem.DTOs.ShipmentDTOs
{
    public class ShipmentDetailsDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string? CustomerAdditionalPhone { get; set; }
        public string CustomerEmail { get; set; } = null!;
        public AddressDto CustomerAddress { get; set; } = null!;
        public string ShipmentDescription { get; set; } = null!;
        public decimal ShipmentWeight { get; set; }
        public decimal ShipmentLength { get; set; }
        public decimal ShipmentWidth { get; set; }
        public decimal ShipmentHeight { get; set; }
        public decimal ShipmentVolume { get; set; }
        public int Quantity { get; set; }
        public string? ShipmentNotes { get; set; }
        public bool CashOnDeliveryEnabled { get; set; }
        public bool OpenPackageOnDeliveryEnabled { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }
        public decimal CollectionAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal AdditionalWeight { get; set; }
        public decimal AdditionalWeightCost { get; set; }
        public decimal CollectionFee { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal NetPayout { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ShipmentTrackingNumber { get; set; } = null!;
        public ICollection<ShipmentStatusHistoryDto> ShipmentStatuses { get; set; } = new List<ShipmentStatusHistoryDto>();
    }
}
