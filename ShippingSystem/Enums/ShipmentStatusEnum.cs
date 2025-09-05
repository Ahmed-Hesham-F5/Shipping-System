namespace ShippingSystem.Enums
{
    public enum ShipmentStatusEnum
    {
        Pending,
        Canceled,
        WatingForPickup,
        PickedUp,
        InWarehouse,
        OnHold,
        OutForDelivery,
        FailedDelivery,
        ReturningToWarehouse,
        ReturningToShipper,
        Delivered,
        Returned,
        Lost,
        Damaged
    }
}