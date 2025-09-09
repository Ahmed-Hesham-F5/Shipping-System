namespace ShippingSystem.Settings
{
    public class JWT
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public double DurationInMinutesForAccessToken { get; set; }
        public double DurationInMinutesForRefreshToken { get; set; }
    }
}
