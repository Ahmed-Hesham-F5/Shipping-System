namespace ShippingSystem.Midleware
{
    public static class SecurityHeaderExtensionsMiddlware
    {
        //This stops browsers from second-guessing the MIME type and treating JSON as HTML or JavaScript.
        public static IApplicationBuilder UseNoSniffHeader(this IApplicationBuilder app) =>
              app.Use((ctx, next) =>
              {
                  ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                  return next();
              });
       
        
        //triggers a pre-flight OPTIONS request (“non-simple" request ).
        public static IApplicationBuilder HeaderChecker(this IApplicationBuilder app) =>
       app.Use(async (context, next) =>
       {
           // Let the browser’s CORS pre-flight through; it will NOT have the custom header.
           if (context.Request.Method == HttpMethods.Options)
           {
               await next();
               return;
           }

           // Check for the required header (name is case-insensitive).
           if (!context.Request.Headers.ContainsKey("X-Client-Key"))
           {
               context.Response.StatusCode = StatusCodes.Status400BadRequest;
               await context.Response.WriteAsync("Missing X-Client-Key header.");
               return; // stop pipeline
           }

           // Optionally: validate the header value here (length, HMAC, etc.)

           await next();
       });

    }
}
