namespace LoginExercise.Middlewares
{
    public class CheckHttpMethod
    {
        public readonly RequestDelegate _next;

        public CheckHttpMethod(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 200;
            }
        }
    }

    public static class CheckHttpMethodExtension
    {
        public static IApplicationBuilder UseCheckHttpMethod(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CheckHttpMethod>();
        }
    }
}
