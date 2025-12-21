using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PaymentGateway;

/// <summary>
/// Real-World Example: Multi-Gateway Payment Processing
///
/// This example demonstrates how to use KeyedServices to support multiple
/// payment gateways (Stripe, PayPal, Square, etc.) in a single application.
/// </summary>

// ========================================
// DOMAIN MODELS
// ========================================

public record PaymentRequest(
    decimal Amount,
    string Currency,
    string CustomerId,
    string? PaymentMethodId = null,
    Dictionary<string, string>? Metadata = null
);

public record PaymentResult(
    bool Success,
    string TransactionId,
    string Gateway,
    decimal Amount,
    string? ErrorMessage = null,
    decimal? ProcessingFee = null
);

public record RefundRequest(
    string TransactionId,
    decimal Amount,
    string Reason
);

public record RefundResult(
    bool Success,
    string RefundId,
    string? ErrorMessage = null
);

// ========================================
// PAYMENT GATEWAY ABSTRACTION
// ========================================

public interface IPaymentGateway
{
    string GatewayName { get; }
    decimal ProcessingFeePercentage { get; }
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<RefundResult> RefundPaymentAsync(RefundRequest request);
    Task<bool> VerifyPaymentAsync(string transactionId);
}

// ========================================
// CONCRETE PAYMENT GATEWAYS
// ========================================

public class StripePaymentGateway : IPaymentGateway
{
    public string GatewayName => "Stripe";
    public decimal ProcessingFeePercentage => 2.9m;

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        Console.WriteLine($"💳 [STRIPE] Processing payment");
        Console.WriteLine($"   Amount: {request.Amount:C} {request.Currency}");
        Console.WriteLine($"   Customer: {request.CustomerId}");

        // Simulate Stripe API call
        await Task.Delay(150);

        var fee = request.Amount * (ProcessingFeePercentage / 100);
        var transactionId = $"stripe_{Guid.NewGuid():N}";

        Console.WriteLine($"   ✓ Payment successful (Fee: {fee:C})");
        Console.WriteLine($"   Transaction ID: {transactionId}");

        return new PaymentResult(
            true,
            transactionId,
            GatewayName,
            request.Amount,
            ProcessingFee: fee
        );
    }

    public async Task<RefundResult> RefundPaymentAsync(RefundRequest request)
    {
        Console.WriteLine($"💳 [STRIPE] Processing refund");
        Console.WriteLine($"   Transaction: {request.TransactionId}");
        Console.WriteLine($"   Amount: {request.Amount:C}");

        await Task.Delay(100);

        var refundId = $"refund_stripe_{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Refund successful (ID: {refundId})");

        return new RefundResult(true, refundId);
    }

    public async Task<bool> VerifyPaymentAsync(string transactionId)
    {
        await Task.Delay(50);
        return true;
    }
}

public class PayPalPaymentGateway : IPaymentGateway
{
    public string GatewayName => "PayPal";
    public decimal ProcessingFeePercentage => 3.5m;

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        Console.WriteLine($"🅿️ [PAYPAL] Processing payment");
        Console.WriteLine($"   Amount: {request.Amount:C} {request.Currency}");
        Console.WriteLine($"   Customer: {request.CustomerId}");

        // Simulate PayPal API call
        await Task.Delay(200);

        var fee = request.Amount * (ProcessingFeePercentage / 100);
        var transactionId = $"paypal_{Guid.NewGuid():N}";

        Console.WriteLine($"   ✓ Payment successful (Fee: {fee:C})");
        Console.WriteLine($"   Transaction ID: {transactionId}");

        return new PaymentResult(
            true,
            transactionId,
            GatewayName,
            request.Amount,
            ProcessingFee: fee
        );
    }

    public async Task<RefundResult> RefundPaymentAsync(RefundRequest request)
    {
        Console.WriteLine($"🅿️ [PAYPAL] Processing refund");
        Console.WriteLine($"   Transaction: {request.TransactionId}");

        await Task.Delay(150);

        var refundId = $"refund_paypal_{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Refund successful (ID: {refundId})");

        return new RefundResult(true, refundId);
    }

    public async Task<bool> VerifyPaymentAsync(string transactionId)
    {
        await Task.Delay(75);
        return true;
    }
}

public class SquarePaymentGateway : IPaymentGateway
{
    public string GatewayName => "Square";
    public decimal ProcessingFeePercentage => 2.6m;

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        Console.WriteLine($"⬛ [SQUARE] Processing payment");
        Console.WriteLine($"   Amount: {request.Amount:C} {request.Currency}");
        Console.WriteLine($"   Customer: {request.CustomerId}");

        // Simulate Square API call
        await Task.Delay(120);

        var fee = request.Amount * (ProcessingFeePercentage / 100);
        var transactionId = $"square_{Guid.NewGuid():N}";

        Console.WriteLine($"   ✓ Payment successful (Fee: {fee:C})");
        Console.WriteLine($"   Transaction ID: {transactionId}");

        return new PaymentResult(
            true,
            transactionId,
            GatewayName,
            request.Amount,
            ProcessingFee: fee
        );
    }

    public async Task<RefundResult> RefundPaymentAsync(RefundRequest request)
    {
        Console.WriteLine($"⬛ [SQUARE] Processing refund");
        Console.WriteLine($"   Transaction: {request.TransactionId}");

        await Task.Delay(90);

        var refundId = $"refund_square_{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Refund successful (ID: {refundId})");

        return new RefundResult(true, refundId);
    }

    public async Task<bool> VerifyPaymentAsync(string transactionId)
    {
        await Task.Delay(60);
        return true;
    }
}

public class CryptoPaymentGateway : IPaymentGateway
{
    public string GatewayName => "Crypto (Bitcoin/Ethereum)";
    public decimal ProcessingFeePercentage => 1.0m;

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        Console.WriteLine($"₿ [CRYPTO] Processing payment");
        Console.WriteLine($"   Amount: {request.Amount:C} {request.Currency}");
        Console.WriteLine($"   Converting to cryptocurrency...");

        // Simulate blockchain transaction
        await Task.Delay(300); // Blockchain can be slower

        var fee = request.Amount * (ProcessingFeePercentage / 100);
        var transactionId = $"crypto_{Guid.NewGuid():N}";

        Console.WriteLine($"   ✓ Payment successful (Fee: {fee:C})");
        Console.WriteLine($"   Transaction ID: {transactionId}");
        Console.WriteLine($"   Blockchain confirmations: Pending...");

        return new PaymentResult(
            true,
            transactionId,
            GatewayName,
            request.Amount,
            ProcessingFee: fee
        );
    }

    public async Task<RefundResult> RefundPaymentAsync(RefundRequest request)
    {
        Console.WriteLine($"₿ [CRYPTO] Processing refund (blockchain transaction)");
        Console.WriteLine($"   Transaction: {request.TransactionId}");

        await Task.Delay(250);

        var refundId = $"refund_crypto_{Guid.NewGuid():N}";
        Console.WriteLine($"   ✓ Refund initiated (ID: {refundId})");

        return new RefundResult(true, refundId);
    }

    public async Task<bool> VerifyPaymentAsync(string transactionId)
    {
        Console.WriteLine($"   Waiting for blockchain confirmations...");
        await Task.Delay(200);
        return true;
    }
}

// ========================================
// PAYMENT SERVICE - Smart Gateway Selection
// ========================================

public class PaymentService
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(string gatewayKey, PaymentRequest request)
    {
        var gateway = _serviceProvider.GetRequiredKeyedService<IPaymentGateway>(gatewayKey);
        return await gateway.ProcessPaymentAsync(request);
    }

    public async Task<PaymentResult> ProcessWithLowestFeeAsync(PaymentRequest request)
    {
        Console.WriteLine($"\n💰 Finding gateway with lowest processing fee:");
        Console.WriteLine(new string('=', 70));

        var gateways = new[]
        {
            ("stripe", _serviceProvider.GetRequiredKeyedService<IPaymentGateway>("stripe")),
            ("paypal", _serviceProvider.GetRequiredKeyedService<IPaymentGateway>("paypal")),
            ("square", _serviceProvider.GetRequiredKeyedService<IPaymentGateway>("square")),
            ("crypto", _serviceProvider.GetRequiredKeyedService<IPaymentGateway>("crypto"))
        };

        foreach (var (key, gateway) in gateways)
        {
            var estimatedFee = request.Amount * (gateway.ProcessingFeePercentage / 100);
            Console.WriteLine($"   {gateway.GatewayName}: {gateway.ProcessingFeePercentage}% = {estimatedFee:C}");
        }

        var cheapest = gateways.MinBy(g => g.Item2.ProcessingFeePercentage);
        Console.WriteLine($"   → Selected: {cheapest.Item2.GatewayName}");
        Console.WriteLine();

        return await cheapest.Item2.ProcessPaymentAsync(request);
    }

    public async Task<List<PaymentResult>> ProcessWithFallbackAsync(
        PaymentRequest request,
        params string[] gatewayKeys)
    {
        Console.WriteLine($"\n🔄 Processing with fallback strategy:");
        Console.WriteLine(new string('=', 70));

        var results = new List<PaymentResult>();

        foreach (var gatewayKey in gatewayKeys)
        {
            try
            {
                var gateway = _serviceProvider.GetRequiredKeyedService<IPaymentGateway>(gatewayKey);
                var result = await gateway.ProcessPaymentAsync(request);

                results.Add(result);

                if (result.Success)
                {
                    Console.WriteLine($"   ✓ Payment successful via {gateway.GatewayName}");
                    return results;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ {gatewayKey} failed: {ex.Message}");
                Console.WriteLine($"   → Trying next gateway...");
            }
        }

        Console.WriteLine($"   ✗ All gateways failed!");
        return results;
    }
}

// ========================================
// PAYMENT PROCESSOR WITH ALL GATEWAYS
// ========================================

public class MultiGatewayPaymentProcessor
{
    private readonly IPaymentGateway _stripeGateway;
    private readonly IPaymentGateway _paypalGateway;
    private readonly IPaymentGateway _squareGateway;
    private readonly IPaymentGateway _cryptoGateway;

    public MultiGatewayPaymentProcessor(
        [FromKeyedServices("stripe")] IPaymentGateway stripeGateway,
        [FromKeyedServices("paypal")] IPaymentGateway paypalGateway,
        [FromKeyedServices("square")] IPaymentGateway squareGateway,
        [FromKeyedServices("crypto")] IPaymentGateway cryptoGateway)
    {
        _stripeGateway = stripeGateway;
        _paypalGateway = paypalGateway;
        _squareGateway = squareGateway;
        _cryptoGateway = cryptoGateway;
    }

    public async Task CompareGatewayFeesAsync(decimal amount)
    {
        Console.WriteLine($"\n💵 Comparing fees for {amount:C} payment:");
        Console.WriteLine(new string('=', 70));

        var gateways = new[] { _stripeGateway, _paypalGateway, _squareGateway, _cryptoGateway };

        foreach (var gateway in gateways)
        {
            var fee = amount * (gateway.ProcessingFeePercentage / 100);
            var netAmount = amount - fee;

            Console.WriteLine($"{gateway.GatewayName,-30} Fee: {gateway.ProcessingFeePercentage}% = {fee:C}  (Net: {netAmount:C})");
        }
    }

    public async Task ProcessThroughAllGatewaysAsync(PaymentRequest request)
    {
        Console.WriteLine($"\n🌐 Processing through all gateways (demo):");
        Console.WriteLine(new string('=', 70));

        var tasks = new[]
        {
            _stripeGateway.ProcessPaymentAsync(request),
            _paypalGateway.ProcessPaymentAsync(request),
            _squareGateway.ProcessPaymentAsync(request),
            _cryptoGateway.ProcessPaymentAsync(request)
        };

        var results = await Task.WhenAll(tasks);

        Console.WriteLine($"\n📊 Summary:");
        foreach (var result in results)
        {
            Console.WriteLine($"   {result.Gateway}: {(result.Success ? "✓" : "✗")} {result.TransactionId}");
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
        Console.WriteLine("REAL-WORLD KEYED SERVICES: PAYMENT GATEWAY INTEGRATION");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register all payment gateways as keyed services
                services.AddKeyedSingleton<IPaymentGateway, StripePaymentGateway>("stripe");
                services.AddKeyedSingleton<IPaymentGateway, PayPalPaymentGateway>("paypal");
                services.AddKeyedSingleton<IPaymentGateway, SquarePaymentGateway>("square");
                services.AddKeyedSingleton<IPaymentGateway, CryptoPaymentGateway>("crypto");

                // Register payment services
                services.AddSingleton<PaymentService>();
                services.AddSingleton<MultiGatewayPaymentProcessor>();
            })
            .Build();

        var paymentService = host.Services.GetRequiredService<PaymentService>();
        var multiGatewayProcessor = host.Services.GetRequiredService<MultiGatewayPaymentProcessor>();

        // ========================================
        // SCENARIO 1: Pay via specific gateway
        // ========================================
        Console.WriteLine("SCENARIO 1: Process payment via Stripe");
        Console.WriteLine("-" .PadRight(70, '-'));

        var payment1 = new PaymentRequest(
            Amount: 99.99m,
            Currency: "USD",
            CustomerId: "cust_123",
            PaymentMethodId: "pm_card_visa"
        );

        await paymentService.ProcessPaymentAsync("stripe", payment1);

        // ========================================
        // SCENARIO 2: Find lowest fee
        // ========================================
        var payment2 = new PaymentRequest(
            Amount: 500.00m,
            Currency: "USD",
            CustomerId: "cust_456"
        );

        await paymentService.ProcessWithLowestFeeAsync(payment2);

        // ========================================
        // SCENARIO 3: Fallback strategy
        // ========================================
        var payment3 = new PaymentRequest(
            Amount: 250.00m,
            Currency: "USD",
            CustomerId: "cust_789"
        );

        await paymentService.ProcessWithFallbackAsync(
            payment3,
            "stripe",   // Try Stripe first
            "square",   // Then Square
            "paypal"    // Finally PayPal
        );

        // ========================================
        // SCENARIO 4: Compare fees
        // ========================================
        await multiGatewayProcessor.CompareGatewayFeesAsync(1000.00m);

        // ========================================
        // SUMMARY
        // ========================================
        Console.WriteLine("\n\n" + "=" .PadRight(70, '='));
        Console.WriteLine("BENEFITS OF KEYED SERVICES FOR PAYMENT GATEWAYS:");
        Console.WriteLine("=" .PadRight(70, '='));
        Console.WriteLine();
        Console.WriteLine("✓ Support multiple payment providers seamlessly");
        Console.WriteLine("✓ Easy to add new payment gateways");
        Console.WriteLine("✓ Dynamic gateway selection based on fees/preferences");
        Console.WriteLine("✓ Fallback strategies for reliability");
        Console.WriteLine("✓ A/B testing different payment providers");
        Console.WriteLine("✓ Region-specific gateway selection");
        Console.WriteLine("✓ Clean, testable architecture");
        Console.WriteLine();
    }
}
