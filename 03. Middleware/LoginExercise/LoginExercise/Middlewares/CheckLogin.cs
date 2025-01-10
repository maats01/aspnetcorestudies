using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace LoginExercise.Middlewares
{
    public class CheckLogin
    {
        private readonly RequestDelegate _next;

        public CheckLogin(RequestDelegate next)
        {
            _next = next;   
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string email = string.Empty;
            string password = string.Empty;
            StreamReader reader = new StreamReader(context.Request.Body);
            string body = await reader.ReadToEndAsync();
            Dictionary<string, StringValues> bodyDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
            
            if (bodyDict.ContainsKey("email"))
            {
                email = bodyDict["email"]; 
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'email'\n");
            }

            if (bodyDict.ContainsKey("password"))
            {
                password = bodyDict["password"];
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for 'password'\n");
            }

            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {
                if (email == "admin@example.com" && password == "admin1234")
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Successful login");
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid Login");
                }
            }
        }
    }

    public static class CheckLoginExtension
    {
        public static IApplicationBuilder UseCheckLogin(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CheckLogin>();
        }
    }
}
