namespace ShippingSystem.DTOs.ShipmentDTOs
{
    public class ShipmentFiltrationParams
    {
        public string? StatusFilter { get; set; } = null;
        public string? SearchBy { get; set; } = null;
        public string? SearchValue { get; set; } = null;
        public bool? CashOnDeliveryEnabled { get; set; } = null;
        public bool? OpenPackageOnDeliveryEnabled { get; set; } = null;
        public bool? ExpressDeliveryEnabled { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public string? SortDirection { get; set; } = null;
    }
}
