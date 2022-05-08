namespace MessengerApi.Middlewares
{
    public class SignalRAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SignalRAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers["Connection"] == "Upgrade" &&
                context.Request.Query.TryGetValue("access_token", out var token))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + token.First());
            }
            
            await _next.Invoke(context);
        }
    }
}