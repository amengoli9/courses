using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Domain.Repositories;

namespace Restaurant.Domain.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;

    public ReservationService(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByTableIdAsync(tableId, cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByStatusAsync(status, cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByDateRangeAsync(startDate, endDate, cancellationToken);
    }

    public async Task<Reservation> CreateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        return await _repository.AddAsync(reservation, cancellationToken);
    }

    public async Task<Reservation?> UpdateAsync(Guid id, Reservation reservation, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
            return null;

        existing.TableId = reservation.TableId;
        existing.CustomerName = reservation.CustomerName;
        existing.CustomerEmail = reservation.CustomerEmail;
        existing.CustomerPhone = reservation.CustomerPhone;
        existing.NumberOfGuests = reservation.NumberOfGuests;
        existing.ReservationDateTime = reservation.ReservationDateTime;
        existing.Status = reservation.Status;
        existing.SpecialRequests = reservation.SpecialRequests;
        existing.UpdatedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(existing, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}
