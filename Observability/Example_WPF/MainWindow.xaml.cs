using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example_WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
   private static readonly ActivitySource ActivitySource = new("MyWpfApp.Activities");
   private static readonly Meter Meter = new("MyWpfApp.Metrics", "1.0.0");
   private static readonly Counter<long> Clicks = Meter.CreateCounter<long>("ui.clicks");


   private readonly ILogger<MainWindow> _logger;
   private readonly IHttpClientFactory _httpClientFactory;


   public MainWindow()
   {
      InitializeComponent();
      _logger = App.AppHost!.Services.GetRequiredService<ILogger<MainWindow>>();
      _httpClientFactory = App.AppHost!.Services.GetRequiredService<IHttpClientFactory>();


      // Se l'utente inserisce un endpoint OTLP, impostalo in variabile d'ambiente (gRPC)
      EndpointBox.TextChanged += (_, __) =>
      {
         var v = EndpointBox.Text?.Trim();
      };
   }


   private async void BtnTrace_Click(object sender, RoutedEventArgs e)
   {
      using var activity = ActivitySource.StartActivity("UI.DoWork");
      activity?.SetTag("ui.action", "trace_button");


      try
      {
         var client = _httpClientFactory.CreateClient();
         // chiamata innocua per generare una span HTTP
         _ = await client.GetAsync("https://www.example.com/");
         StatusText.Text = "Traccia inviata (con span HTTP)";
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Errore durante la chiamata HTTP");
         StatusText.Text = "Errore HTTP (vedi log)";
      }
   }


   private void BtnMetric_Click(object sender, RoutedEventArgs e)
   {
      Clicks.Add(1, new KeyValuePair<string, object?>("button", "metric"));
      StatusText.Text = "Metrica incrementata";
   }


   private void BtnLog_Click(object sender, RoutedEventArgs e)
   {
      _logger.LogInformation("Clic su {Button} alle {Time}", "Invia Log", DateTimeOffset.Now);
      StatusText.Text = "Log inviato";
   }
}