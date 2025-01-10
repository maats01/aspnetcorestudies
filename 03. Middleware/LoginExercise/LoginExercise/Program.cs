using LoginExercise.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseCheckHttpMethod();
app.UseCheckLogin();

app.Run();
