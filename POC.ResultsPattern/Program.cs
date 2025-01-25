
using FluentValidation;
using POC.ResultsPattern.Behaviors;
using POC.ResultsPattern.Endpoints;
using POC.ResultsPattern.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

var assemblyToScan = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(assemblyToScan);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    
});

builder.Services.AddValidatorsFromAssembly(assemblyToScan);




var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
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
