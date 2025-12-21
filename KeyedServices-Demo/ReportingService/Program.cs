using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ReportingService;

/// <summary>
/// Real-World Example: Report Generation with Multiple Formats
///
/// Demonstrates using KeyedServices for generating reports in different formats
/// (PDF, Excel, CSV, JSON, HTML) from the same data.
/// </summary>

// ========================================
// DOMAIN MODELS
// ========================================

public record ReportData(
    string Title,
    DateTime GeneratedAt,
    List<Dictionary<string, object>> Rows
);

public record ReportOptions(
    bool IncludeHeader = true,
    bool IncludeFooter = true,
    string? Styling = null
);

// ========================================
// REPORT GENERATOR ABSTRACTION
// ========================================

public interface IReportGenerator
{
    string FormatName { get; }
    string FileExtension { get; }
    Task<byte[]> GenerateAsync(ReportData data, ReportOptions? options = null);
}

// ========================================
// CONCRETE REPORT GENERATORS
// ========================================

public class PdfReportGenerator : IReportGenerator
{
    public string FormatName => "PDF";
    public string FileExtension => ".pdf";

    public async Task<byte[]> GenerateAsync(ReportData data, ReportOptions? options = null)
    {
        Console.WriteLine($"📄 [PDF] Generating report: {data.Title}");
        Console.WriteLine($"   Rows: {data.Rows.Count}");
        Console.WriteLine($"   Creating PDF document...");

        await Task.Delay(150);

        Console.WriteLine($"   ✓ PDF generated ({data.Rows.Count * 1024} bytes)");
        return new byte[data.Rows.Count * 1024];
    }
}

public class ExcelReportGenerator : IReportGenerator
{
    public string FormatName => "Excel (XLSX)";
    public string FileExtension => ".xlsx";

    public async Task<byte[]> GenerateAsync(ReportData data, ReportOptions? options = null)
    {
        Console.WriteLine($"📊 [EXCEL] Generating spreadsheet: {data.Title}");
        Console.WriteLine($"   Rows: {data.Rows.Count}");
        Console.WriteLine($"   Creating workbook with formulas and formatting...");

        await Task.Delay(120);

        Console.WriteLine($"   ✓ Excel file generated ({data.Rows.Count * 2048} bytes)");
        return new byte[data.Rows.Count * 2048];
    }
}

public class CsvReportGenerator : IReportGenerator
{
    public string FormatName => "CSV";
    public string FileExtension => ".csv";

    public async Task<byte[]> GenerateAsync(ReportData data, ReportOptions? options = null)
    {
        Console.WriteLine($"📝 [CSV] Generating CSV file: {data.Title}");
        Console.WriteLine($"   Rows: {data.Rows.Count}");
        Console.WriteLine($"   Writing comma-separated values...");

        await Task.Delay(50);

        Console.WriteLine($"   ✓ CSV generated ({data.Rows.Count * 256} bytes)");
        return new byte[data.Rows.Count * 256];
    }
}

public class JsonReportGenerator : IReportGenerator
{
    public string FormatName => "JSON";
    public string FileExtension => ".json";

    public async Task<byte[]> GenerateAsync(ReportData data, ReportOptions? options = null)
    {
        Console.WriteLine($"🔤 [JSON] Generating JSON report: {data.Title}");
        Console.WriteLine($"   Rows: {data.Rows.Count}");
        Console.WriteLine($"   Serializing to JSON...");

        await Task.Delay(40);

        Console.WriteLine($"   ✓ JSON generated ({data.Rows.Count * 512} bytes)");
        return new byte[data.Rows.Count * 512];
    }
}

public class HtmlReportGenerator : IReportGenerator
{
    public string FormatName => "HTML";
    public string FileExtension => ".html";

    public async Task<byte[]> GenerateAsync(ReportData data, ReportOptions? options = null)
    {
        Console.WriteLine($"🌐 [HTML] Generating HTML report: {data.Title}");
        Console.WriteLine($"   Rows: {data.Rows.Count}");
        Console.WriteLine($"   Creating responsive HTML table...");

        await Task.Delay(80);

        Console.WriteLine($"   ✓ HTML generated ({data.Rows.Count * 768} bytes)");
        return new byte[data.Rows.Count * 768];
    }
}

// ========================================
// REPORT SERVICE
// ========================================

public class ReportService
{
    private readonly IServiceProvider _serviceProvider;

    public ReportService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<byte[]> GenerateReportAsync(string format, ReportData data, ReportOptions? options = null)
    {
        var generator = _serviceProvider.GetRequiredKeyedService<IReportGenerator>(format);
        return await generator.GenerateAsync(data, options);
    }

    public async Task GenerateAllFormatsAsync(ReportData data)
    {
        Console.WriteLine($"\n📑 Generating report in ALL formats:");
        Console.WriteLine(new string('=', 70));

        var formats = new[] { "pdf", "excel", "csv", "json", "html" };

        foreach (var format in formats)
        {
            var generator = _serviceProvider.GetRequiredKeyedService<IReportGenerator>(format);
            await generator.GenerateAsync(data);
            Console.WriteLine();
        }
    }
}

// ========================================
// MULTI-FORMAT REPORT EXPORTER
// ========================================

public class MultiFormatReportExporter
{
    private readonly IReportGenerator _pdfGenerator;
    private readonly IReportGenerator _excelGenerator;
    private readonly IReportGenerator _csvGenerator;
    private readonly IReportGenerator _jsonGenerator;
    private readonly IReportGenerator _htmlGenerator;

    public MultiFormatReportExporter(
        [FromKeyedServices("pdf")] IReportGenerator pdfGenerator,
        [FromKeyedServices("excel")] IReportGenerator excelGenerator,
        [FromKeyedServices("csv")] IReportGenerator csvGenerator,
        [FromKeyedServices("json")] IReportGenerator jsonGenerator,
        [FromKeyedServices("html")] IReportGenerator htmlGenerator)
    {
        _pdfGenerator = pdfGenerator;
        _excelGenerator = excelGenerator;
        _csvGenerator = csvGenerator;
        _jsonGenerator = jsonGenerator;
        _htmlGenerator = htmlGenerator;
    }

    public async Task ExportReportPackageAsync(ReportData data)
    {
        Console.WriteLine($"\n📦 Creating report package with all formats:");
        Console.WriteLine(new string('=', 70));

        var generators = new[] { _pdfGenerator, _excelGenerator, _csvGenerator, _jsonGenerator, _htmlGenerator };

        var tasks = generators.Select(g => g.GenerateAsync(data)).ToArray();
        var results = await Task.WhenAll(tasks);

        Console.WriteLine($"\n✓ Report package created:");
        for (int i = 0; i < generators.Length; i++)
        {
            Console.WriteLine($"   - {data.Title}{generators[i].FileExtension} ({results[i].Length} bytes)");
        }
    }
}

// ========================================
// MAIN PROGRAM
// ========================================

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine("KEYED SERVICES: MULTI-FORMAT REPORT GENERATION");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register all report generators as keyed services
                services.AddKeyedSingleton<IReportGenerator, PdfReportGenerator>("pdf");
                services.AddKeyedSingleton<IReportGenerator, ExcelReportGenerator>("excel");
                services.AddKeyedSingleton<IReportGenerator, CsvReportGenerator>("csv");
                services.AddKeyedSingleton<IReportGenerator, JsonReportGenerator>("json");
                services.AddKeyedSingleton<IReportGenerator, HtmlReportGenerator>("html");

                // Register services
                services.AddSingleton<ReportService>();
                services.AddSingleton<MultiFormatReportExporter>();
            })
            .Build();

        // Sample report data
        var salesData = new ReportData(
            Title: "Monthly Sales Report - December 2024",
            GeneratedAt: DateTime.Now,
            Rows: new List<Dictionary<string, object>>
            {
                new() { ["Product"] = "Laptop", ["Quantity"] = 50, ["Revenue"] = 50000 },
                new() { ["Product"] = "Mouse", ["Quantity"] = 200, ["Revenue"] = 4000 },
                new() { ["Product"] = "Keyboard", ["Quantity"] = 150, ["Revenue"] = 7500 },
                new() { ["Product"] = "Monitor", ["Quantity"] = 75, ["Revenue"] = 22500 }
            }
        );

        var reportService = host.Services.GetRequiredService<ReportService>();
        var multiFormatExporter = host.Services.GetRequiredService<MultiFormatReportExporter>();

        // ========================================
        // DEMO 1: Generate specific format
        // ========================================
        Console.WriteLine("DEMO 1: Generate Report in Specific Format");
        Console.WriteLine("-" .PadRight(70, '-'));

        await reportService.GenerateReportAsync("pdf", salesData);

        // ========================================
        // DEMO 2: Generate all formats
        // ========================================
        Console.WriteLine("\n\nDEMO 2: Generate Report in All Formats");
        Console.WriteLine("-" .PadRight(70, '-'));

        await reportService.GenerateAllFormatsAsync(salesData);

        // ========================================
        // DEMO 3: Export package
        // ========================================
        Console.WriteLine("\n\nDEMO 3: Export Complete Report Package");
        Console.WriteLine("-" .PadRight(70, '-'));

        await multiFormatExporter.ExportReportPackageAsync(salesData);

        // ========================================
        // SUMMARY
        // ========================================
        Console.WriteLine("\n\n" + "=" .PadRight(70, '='));
        Console.WriteLine("BENEFITS:");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine("✓ Support multiple report formats from same data");
        Console.WriteLine("✓ Easy to add new export formats");
        Console.WriteLine("✓ User can choose preferred format");
        Console.WriteLine("✓ Generate multiple formats simultaneously");
        Console.WriteLine("✓ Clean, maintainable architecture");
        Console.WriteLine();
    }
}
