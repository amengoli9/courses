namespace SantasWorkshop.Interfaces.Delivery;

/// <summary>
/// [I] Interfaccia specifica per consegne con drone
/// Separata perché solo il drone usa GPS e batterie
/// </summary>
public interface IDroneDelivery : IDeliveryStrategy
{
    void ChargeBattery();
    void UpdateGPS();
}
