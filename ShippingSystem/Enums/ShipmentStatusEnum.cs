namespace ShippingSystem.Enums
{
    public enum ShipmentStatusEnum
    {
        // جديد
        NewShipment,
        InReviewForPickup,
        WaitingForPickup,
        // الأوردرات قيد التنفيذ
        PickedUp,
        InWarehouse,
        InReviewForReturn,
        InReviewForExchange,
        WaitingForReturn,
        WaitingForExchange,
        WaitingForDelivery,
        OutForDelivery,
        ReturningToShipper,
        // المتوقف حاليا
        WaitingForShipperAction,
        RejectedReturns,
        // تم بنجاح
        Exchanged,
        Delivered,
        // غير ناجح
        Returned,
        Canceled,
        FailedDelivery,
        ReturningToWarehouse,
        Lost,
        Damaged
    }
}