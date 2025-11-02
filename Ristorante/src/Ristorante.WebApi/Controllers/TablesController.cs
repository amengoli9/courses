using Microsoft.AspNetCore.Mvc;
using Ristorante.WebApi.DTOs;
using Ristorante.Domain.Services;
using Ristorante.Domain.Entities;
using Ristorante.Domain.Enums;
using System.Diagnostics;

namespace Ristorante.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController : ControllerBase
{
    private readonly ITableService _tableService;
    private readonly ILogger<TablesController> _logger;
    private static readonly ActivitySource ActivitySource = new("Ristorante.WebApi", "1.0.0");

    public TablesController(ITableService tableService, ILogger<TablesController> logger)
    {
        _tableService = tableService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TableDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetAllTables");
        _logger.LogInformation("Fetching all tables");

        var tables = await _tableService.GetAllAsync(cancellationToken);
        var result = tables.Select(MapToDto).ToList();

        _logger.LogInformation("Retrieved {Count} tables", result.Count);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TableDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("GetTableById");
        activity?.SetTag("table.id", id.ToString());

        _logger.LogInformation("Fetching table {TableId}", id);

        var table = await _tableService.GetByIdAsync(id, cancellationToken);
        if (table is null)
        {
            _logger.LogWarning("Table {TableId} not found", id);
            return NotFound();
        }

        return Ok(MapToDto(table));
    }

    [HttpPost]
    [ProducesResponseType(typeof(TableDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTableRequest request, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("CreateTable");
        _logger.LogInformation("Creating new table {TableNumber}", request.TableNumber);

        var table = new Table
        {
            TableNumber = request.TableNumber,
            Capacity = request.Capacity,
            Status = request.Status,
            Location = request.Location,
            Notes = request.Notes
        };

        var created = await _tableService.CreateAsync(table, cancellationToken);
        _logger.LogInformation("Table created with ID {TableId}", created.Id);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    private static TableDto MapToDto(Table entity)
    {
        return new TableDto(
            entity.Id,
            entity.TableNumber,
            entity.Capacity,
            entity.Status,
            entity.Location,
            entity.Notes,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }
}
