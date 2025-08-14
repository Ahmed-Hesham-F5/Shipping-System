namespace ShippingSystem.Enums
{
    [Flags]
    public enum RolesEnum
    {
        Shipper = 1,
        Courier = 2,
        Storekeeper = 4,
        TechnicalSupport = 8,
        WarehouseManager = 16,
        Accountant = 32,
        Admin = 64,
        MainAdmin= 128
    }

}
