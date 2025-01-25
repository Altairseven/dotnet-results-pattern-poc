using FluentValidation;
using POC.ResultsPattern.Behaviors;
using POC.ResultsPattern.Endpoints;
using POC.ResultsPattern.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var assemblyToScan = typeof(Program).Assembly;
builder.Services.AddValidatorsFromAssembly(assemblyToScan);
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(assemblyToScan);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/", _ => _.Servers = []);
}

app.UseHttpsRedirection();

app.UseStatusCodePages();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

var apiGroup = app.MapGroup("/api");
apiGroup.MapPocEndpoints();

app.Run();
