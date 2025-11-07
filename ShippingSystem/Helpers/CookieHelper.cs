namespace ShippingSystem.Helpers
{
    public static class CookieHelper
    {
        public static void SetRefreshTokenInCookie(HttpResponse response, string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}