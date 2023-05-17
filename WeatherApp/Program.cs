using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection.PortableExecutable;
using WeatherApp;

var serviceName = "Microlise.OpenTelemetry.OtelApi";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithTracing(builder => OpenTelemetryUtil.BuildOpenTelemetryWithTracer(builder))
    .WithMetrics( builder => OpenTelemetryUtil.BuildOpenTelemetryWithMetrics(builder));

builder.Logging.ClearProviders();

builder.Logging.AddOpenTelemetry(options => OpenTelemetryUtil.BuildOpenTelemetryWithLogger(options));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
