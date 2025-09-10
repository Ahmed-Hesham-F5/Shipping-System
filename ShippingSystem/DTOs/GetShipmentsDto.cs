namespace ShippingSystem.DTOs
{
    public class GetShipmentsDTO
    {
        public int Id { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string? ReceiverAdditionalPhone { get; set; }
        public AddressDTO ReceiverAddress { get; set; } = null!;
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ShipmentTrackingNumber { get; set; } = null!;
        public ICollection<ShipmentStatusDTO> ShipmentStatuses { get; set; } = new List<ShipmentStatusDTO>();
    }
}
