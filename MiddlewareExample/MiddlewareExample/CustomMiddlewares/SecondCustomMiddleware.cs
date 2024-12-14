using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewareExample.CustomMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SecondCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public SecondCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Query.ContainsKey("firstname") &&
                httpContext.Request.Query.ContainsKey("lastname"))
            {
                string fullName = httpContext.Request.Query["firstname"] + " " + httpContext.Request.Query["lastname"];
                await httpContext.Response.WriteAsync(fullName);
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SecondCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecondCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecondCustomMiddleware>();
        }
    }
}
