var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    Dictionary<int, String> countries = new Dictionary<int, string>() {
        {1, "United States"},
        {2, "Canada"},
        {3, "United Kingdom"},
        {4, "India"},
        {5, "Japan"}
    };

    endpoints.MapGet("countries", async context =>
    {
        foreach (var item in countries)
        {
            await context.Response.WriteAsync($"{item.Key}, {item.Value}\n");
        }
    });

    endpoints.MapGet("countries/{countryID:int}", async context =>
    {
        int countryID = Convert.ToInt32(context.Request.RouteValues["countryID"]);

        if (countryID > 0 && countryID < 6)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync($"{countries[countryID]}");
        }
        else if (countryID > 5 && countryID < 101)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("[No country]");
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("The CountryID should be between 1 and 100");
        }
    });
});

app.Run();