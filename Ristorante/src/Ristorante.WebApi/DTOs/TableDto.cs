using Ristorante.Domain.Enums;

namespace Ristorante.WebApi.DTOs;

public record TableDto(
    Guid Id,
    int TableNumber,
    int Capacity,
    TableStatus Status,
    string Location,
    string? Notes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateTableRequest(
    int TableNumber,
    int Capacity,
    TableStatus Status,
    string Location,
    string? Notes
);

public record UpdateTableRequest(
    int TableNumber,
    int Capacity,
    TableStatus Status,
    string Location,
    string? Notes
);
