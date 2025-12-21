using Microsoft.Extensions.DependencyInjection;
using SantasWorkshop.Interfaces.Delivery;

namespace SantasWorkshop.Delivery;

/// <summary>
/// Factory per creare strategie di consegna
/// [O] Per aggiungere nuove strategie, basta registrarle nel DI e aggiungere qui
/// </summary>
public class DeliveryStrategyFactory : IDeliveryStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DeliveryStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDeliveryStrategy Create(string deliveryType)
    {
        return deliveryType switch
        {
            "Slitta" => _serviceProvider.GetRequiredService<ISleighDelivery>(),
            "Drone" => _serviceProvider.GetRequiredService<IDroneDelivery>(),
            "Teletrasporto" => _serviceProvider.GetRequiredService<ITeleportDelivery>(),
            _ => _serviceProvider.GetRequiredService<PostalDeliveryStrategy>()
        };
    }
}
