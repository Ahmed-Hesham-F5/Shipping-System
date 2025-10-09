namespace ShippingSystem.DTOs
{
    public class ShipmentStatusStatisticsDto
    {
        public int PendingShipmentsCount { get; set; }
        public int CanceledShipmentsCount { get; set; }
        public int WaitingForPickupShipmentsCount { get; set; }
        public int PickedUpShipmentsCount { get; set; }
        public int InWarehouseShipmentsCount { get; set; }
        public int OnHoldShipmentsCount { get; set; }
        public int OutForDeliveryShipmentsCount { get; set; }
        public int FailedDeliveryShipmentsCount { get; set; }
        public int ReturningToWarehouseShipmentsCount { get; set; }
        public int ReturningToShipperShipmentsCount { get; set; }
        public int DeliveredShipmentsCount { get; set; }
        public int ReturnedShipmentsCount { get; set; }
        public int LostShipmentsCount { get; set; }
        public int DamagedShipmentsCount { get; set; }
    }
}
