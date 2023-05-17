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
        private static string OTELCollectorURLPath() {
            var enviromentPath = Environment.GetEnvironmentVariable("OTEL_COLLECTOR_URL");
            if (enviromentPath != null) {
                return enviromentPath;
            }
            return "http://";
        }

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
                })
# endif
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(OTELCollectorURLPath());
                });
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
                })
#endif
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(OTELCollectorURLPath());
                });
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
#endif
            options.AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(OTELCollectorURLPath());
            });
            return options;
        }
    }
}
