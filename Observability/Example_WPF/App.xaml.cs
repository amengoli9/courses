using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Windows;

namespace Example_WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
   public static IHost AppHost { get; private set; }

   protected override void OnStartup(StartupEventArgs e)
   {
      AppHost = Host.CreateDefaultBuilder()
         .ConfigureServices(services =>
         {
            services.AddHttpClient();
            services.AddOpenTelemetry()
                .ConfigureResource(r => r.AddService(
                    serviceName: "MyWpfApp",
                    serviceVersion: "1.0.0"))
                .WithTracing(cfg =>
                {
                   cfg.AddSource("MyWpfApp.Activities") // ActivitySource custom

                     .AddHttpClientInstrumentation()
                      .AddOtlpExporter();
                })
                .WithMetrics(b =>
                {
                   b.AddMeter("MyWpfApp.Metrics")
                            .AddRuntimeInstrumentation()
                            .AddProcessInstrumentation()
                            .AddOtlpExporter();
                });
         })
         .ConfigureLogging(logging =>
         {
            logging.ClearProviders();
            // Invia i log via OTLP (oltre a Debug/EventLog se vuoi)
            logging.AddOpenTelemetry(o =>
            {
               o.IncludeFormattedMessage = true;
               o.IncludeScopes = true;
               o.AddOtlpExporter();
            });
         })
         .Build();

      AppHost.Start();
      base.OnStartup(e);
   }

   protected override void OnExit(ExitEventArgs e)
   {
      AppHost?.StopAsync().GetAwaiter().GetResult();
      AppHost?.Dispose();
      base.OnExit(e);
   }

}
