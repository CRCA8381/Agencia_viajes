using AgenciaViajes.Api.Data;
using AgenciaViajes.Api.DTOs;
using AgenciaViajes.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajes.Api.Controllers;

[ApiController]
[Route("api/reservas")]
public class ReservasController(AgenciaDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await context.Reservas.AsNoTracking().Include(x => x.Cliente).Include(x => x.Paquete)
            .OrderByDescending(x => x.FechaReserva)
            .Select(x => new
            {
                x.ReservaId, x.ClienteId, x.PaqueteId, x.FechaReserva, x.Estado,
                ClienteNombre = x.Cliente!.Nombre,
                PaqueteNombre = x.Paquete!.Nombre
            }).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await context.Reservas.AsNoTracking().FirstOrDefaultAsync(x => x.ReservaId == id);
        return item is null ? NotFound("La reserva no existe.") : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ReservaRequest request)
    {
        var error = await ValidateAsync(request, null);
        if (error is not null) return error;

        var item = new Reserva
        {
            ClienteId = request.ClienteId,
            PaqueteId = request.PaqueteId,
            FechaReserva = request.FechaReserva.ToUniversalTime(),
            Estado = request.Estado
        };
        context.Reservas.Add(item);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.ReservaId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, ReservaRequest request)
    {
        var item = await context.Reservas.FindAsync(id);
        if (item is null) return NotFound("La reserva no existe.");

        var error = await ValidateAsync(request, id);
        if (error is not null) return error;

        item.ClienteId = request.ClienteId;
        item.PaqueteId = request.PaqueteId;
        item.FechaReserva = request.FechaReserva.ToUniversalTime();
        item.Estado = request.Estado;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await context.Reservas.FindAsync(id);
        if (item is null) return NotFound("La reserva no existe.");
        context.Reservas.Remove(item);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private async Task<IActionResult?> ValidateAsync(ReservaRequest request, int? currentId)
    {
        if (!await context.Clientes.AnyAsync(x => x.ClienteId == request.ClienteId))
            return BadRequest("El cliente indicado no existe.");
        if (!await context.Paquetes.AnyAsync(x => x.PaqueteId == request.PaqueteId))
            return BadRequest("El paquete indicado no existe.");
        if (request.FechaReserva.ToUniversalTime().Date < DateTime.UtcNow.Date)
            return BadRequest("La fecha de reserva no puede ser anterior a la fecha actual.");

        var duplicate = await context.Reservas.AnyAsync(x =>
            x.ClienteId == request.ClienteId &&
            x.PaqueteId == request.PaqueteId &&
            x.FechaReserva.Date == request.FechaReserva.ToUniversalTime().Date &&
            (!currentId.HasValue || x.ReservaId != currentId.Value));

        return duplicate
            ? Conflict("El cliente ya tiene una reserva para ese paquete en la fecha indicada.")
            : null;
    }
}
