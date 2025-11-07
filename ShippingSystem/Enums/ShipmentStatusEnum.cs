namespace ShippingSystem.Enums
{
    public enum ShipmentStatusEnum
    {
        Pending,
        InWarehouse,
        PickedUp,
        Returned,
        Delivered,
        Canceled,
        InReviewForPickup,
        InReviewForReturn,
        InReviewForDelivery,
        InReviewForCancellation,
        InReviewForReschedule,
        InReviewForExchange,
        WaitingForPickup,
        WaitingForReturn,
        WaitingForDelivery,
        OutForDelivery,
        ReturningToWarehouse,
        ReturningToShipper,
        OnHold,
        FailedDelivery,
        Lost,
        Damaged
    }
}