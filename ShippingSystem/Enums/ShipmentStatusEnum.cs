namespace ShippingSystem.Enums
{
    public enum ShipmentStatusEnum
    {
        Pending,
        Canceled,
        WaitingForPickup,
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