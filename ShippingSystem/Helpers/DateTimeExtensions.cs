namespace ShippingSystem.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime UtcNowTrimmedToSeconds()
        {
            var now = DateTime.UtcNow;
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
        }
    }
}
