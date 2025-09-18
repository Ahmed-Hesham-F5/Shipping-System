namespace ShippingSystem.DTOs
{
    public class ShipmentDetailsDto
    {
        public int Id { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string? ReceiverAdditionalPhone { get; set; }
        public ReceiverAddressDto ReceiverAddress { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ShipmentTrackingNumber { get; set; } = null!;
        public ICollection<ShipmentStatusDto> ShipmentStatuses { get; set; } = new List<ShipmentStatusDto>();
    }
}
