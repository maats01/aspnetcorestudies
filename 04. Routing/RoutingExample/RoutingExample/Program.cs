using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options => {
    options.ConstraintMap.Add("months", typeof(CustomMonthConstraint));
    });
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints => {
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        
        await context.Response.WriteAsync($"In files - {fileName} - {extension}");
    });

    endpoints.Map("employee/profile/{employeename:length(3, 7):alpha?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("employeename"))
        {
            string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
            await context.Response.WriteAsync($"In employee profile - {employeeName}");
        }
        else
        {
            await context.Response.WriteAsync("In employee profile - employee name not supplied");
        }
    });

    endpoints.Map("products/details/{id:int:range(1, 1000)?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Products details - {id}");
        }
        else
        {
            await context.Response.WriteAsync("Products details - id not supplied");
        }
    });

    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
    });

    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"City information - {cityId}");
    });

    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context => {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"sales report - {year} - {month}");
    });

    endpoints.Map("sales-report/2024/jan", async context =>
    {
        await context.Response.WriteAsync("sales report exclusively for 2024 - jan");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});

app.Run();
