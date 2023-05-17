using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace WeatherApp
{
     static class OpenTelemetryUtil
    {

        private static readonly string serviceName = "Microlise.OpenTelemetry.OtelApi";
        private static readonly string serviceVersion = "1.0.0";
        static public TracerProviderBuilder BuildOpenTelemetryWithTracer(TracerProviderBuilder builder) {
            builder
                .AddSource(serviceName)
                .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                .AddAspNetCoreInstrumentation()
#if DEBUG
                .AddConsoleExporter(options => {
                    options.Targets = ConsoleExporterOutputTargets.Debug;
                });
#else
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri("http://otel-collector:4317");
                });
#endif
            return builder;
        }

        static public MeterProviderBuilder BuildOpenTelemetryWithMetrics(MeterProviderBuilder builder)
        {
            builder.AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddMeter("MyApplicationMetrics")
                .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
#if DEBUG
                .AddConsoleExporter(options => {
                    options.Targets = ConsoleExporterOutputTargets.Debug;
                });
#else
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri("http://otel-collector:4317");
                });
#endif
            return builder;
        }

        static public OpenTelemetryLoggerOptions BuildOpenTelemetryWithLogger(OpenTelemetryLoggerOptions options)
        {
            options.SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion));
#if DEBUG
            options.AddConsoleExporter(opt => {
                opt.Targets = ConsoleExporterOutputTargets.Debug;
            });
#else
            options.AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri("http://otel-collector:4317");
            });
#endif
            return options;
        }
    }
}
