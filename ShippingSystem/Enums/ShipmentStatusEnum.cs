namespace ShippingSystem.Enums
{
    public enum ShipmentStatusEnum
    {
        NewShipment,
        InWarehouse,
        PickedUp,
        Returned,
        Exchanged,
        Delivered,
        Canceled,
        InReviewForPickup,
        InReviewForReturn,
        InReviewForExchange,
        WaitingForPickup,
        WaitingForReturn,
        WaitingForExchange,
        WaitingForDelivery,
        OutForDelivery,
        ReturningToWarehouse,
        ReturningToShipper,
        FailedDelivery,
        Lost,
        Damaged
    }
}