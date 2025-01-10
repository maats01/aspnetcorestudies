var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    context.Response.StatusCode = 400;
    string[] operators = new string[] {"add", "subtract", "divide", "multiply", "rest"};

    string op = string.Empty;
    bool isFirstNum = false;
    bool isSecondNum = false;
    float n = 0;
    float n2 = 0;

    if (context.Request.Query.ContainsKey("firstNumber"))
    {
        isFirstNum = float.TryParse(context.Request.Query["firstNumber"][0], out n);
    }

    if (context.Request.Query.ContainsKey("secondNumber"))
    {
        isSecondNum = float.TryParse(context.Request.Query["secondNumber"][0], out n2);
    }

    if (context.Request.Query.ContainsKey("operation") && operators.Contains(context.Request.Query["operation"][0]))
    {
        op = context.Request.Query["operation"][0];
    }

    if (!isFirstNum)
    {
        await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
    }

    if (!isSecondNum)
    {
        await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
    }

    if (String.IsNullOrEmpty(op))
    {
        await context.Response.WriteAsync("Invalid input for 'operation'");
    }

    if (isFirstNum && isSecondNum && !String.IsNullOrEmpty(op))
    {
        float result = 0;
        switch (op)
        {
            case "add":
                result = n + n2;
                break;
            case "subtract":
                result = n - n2;
                break;
            case "divide":
                result = n / n2;
                break;
            case "rest":
                result = n % n2;
                break;
            case "multiply":
                result = n * n2;
                break;
        }

        context.Response.StatusCode = 200;
        await context.Response.WriteAsync(Convert.ToString(result));
    }
}
);

app.Run();
