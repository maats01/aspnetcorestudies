
using System.Runtime.CompilerServices;

namespace MiddlewareExample.CustomMiddlewares
{
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("My custom middleware - Starts\n");
            await next(context);
            await context.Response.WriteAsync("My custom middleware - End\n");
        }
    }

    public static class MyCustomMiddlewareExtension
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomMiddleware>();
        }
    }
}
