using AgenciaViajes.Api.Data;
using AgenciaViajes.Api.DTOs;
using AgenciaViajes.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajes.Api.Controllers;

[ApiController]
[Route("api/paquetes")]
public class PaquetesController(AgenciaDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await context.Paquetes.AsNoTracking().Include(x => x.Destino)
            .OrderBy(x => x.Nombre)
            .Select(x => new
            {
                x.PaqueteId, x.DestinoId, x.Nombre, x.Precio, x.Duracion,
                DestinoNombre = x.Destino!.Ciudad + ", " + x.Destino.Pais
            }).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await context.Paquetes.AsNoTracking().FirstOrDefaultAsync(x => x.PaqueteId == id);
        return item is null ? NotFound("El paquete no existe.") : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PaqueteRequest request)
    {
        if (!await context.Destinos.AnyAsync(x => x.DestinoId == request.DestinoId))
            return BadRequest("El destino indicado no existe.");

        var item = new Paquete
        {
            DestinoId = request.DestinoId,
            Nombre = request.Nombre.Trim(),
            Precio = request.Precio,
            Duracion = request.Duracion
        };
        context.Paquetes.Add(item);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.PaqueteId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, PaqueteRequest request)
    {
        var item = await context.Paquetes.FindAsync(id);
        if (item is null) return NotFound("El paquete no existe.");
        if (!await context.Destinos.AnyAsync(x => x.DestinoId == request.DestinoId))
            return BadRequest("El destino indicado no existe.");

        item.DestinoId = request.DestinoId;
        item.Nombre = request.Nombre.Trim();
        item.Precio = request.Precio;
        item.Duracion = request.Duracion;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await context.Paquetes.FindAsync(id);
        if (item is null) return NotFound("El paquete no existe.");
        if (await context.Reservas.AnyAsync(x => x.PaqueteId == id))
            return Conflict("No se puede eliminar: el paquete tiene reservas registradas.");

        context.Paquetes.Remove(item);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
