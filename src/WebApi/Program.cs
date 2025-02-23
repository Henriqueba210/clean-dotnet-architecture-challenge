using System.Diagnostics;

using Infrastructure;
using Infrastructure.Services.Postcodes.Io;

using Location.Api.Endpoints.Location;
using Location.Application;

using Microsoft.AspNetCore.Http.Features;

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Scalar.AspNetCore;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<PostcodesIoSettings>()
    .BindConfiguration(PostcodesIoSettings.ConfigurationSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddOpenApi();

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("Location"))
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
        metrics.AddHttpClientInstrumentation();

        metrics.AddOtlpExporter();
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddHttpClientInstrumentation();

        tracing.AddOtlpExporter();
    });

builder.Logging.AddOpenTelemetry();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

WebApplication app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Location API")
        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.MapLocationEndpoints();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

await app.RunAsync();

public abstract partial class Program;