namespace ShippingSystem.Enums
{
    public enum ShipmentStatusEnum
    {
        Pending,
        Canceled,
        WaitingForReturnPickup,
        PickedUp,
        InWarehouse,
        OnHold,
        WaitingForDelivery,
        OutForDelivery,
        FailedDelivery,
        WaitingForPickup,
        ReturningToWarehouse,
        ReturningToShipper,
        Delivered,
        Returned,
        Lost,
        Damaged
    }
}