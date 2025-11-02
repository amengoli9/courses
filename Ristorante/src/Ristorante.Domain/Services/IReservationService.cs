using Ristorante.Domain.Entities;
using Ristorante.Domain.Enums;

namespace Ristorante.Domain.Services;

public interface IReservationService
{
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Reservation>> GetByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task<Reservation?> UpdateAsync(Guid id, Reservation reservation, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
