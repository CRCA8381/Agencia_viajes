using AgenciaViajes.Api.Data;
using AgenciaViajes.Api.DTOs;
using AgenciaViajes.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajes.Api.Controllers;

[ApiController]
[Route("api/destinos")]
public class DestinosController(AgenciaDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await context.Destinos.AsNoTracking().OrderBy(x => x.Pais).ThenBy(x => x.Ciudad).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await context.Destinos.AsNoTracking().FirstOrDefaultAsync(x => x.DestinoId == id);
        return item is null ? NotFound("El destino no existe.") : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post(DestinoRequest request)
    {
        var item = new Destino
        {
            Pais = request.Pais.Trim(),
            Ciudad = request.Ciudad.Trim(),
            Descripcion = request.Descripcion.Trim(),
            Temporada = request.Temporada.Trim()
        };
        context.Destinos.Add(item);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.DestinoId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, DestinoRequest request)
    {
        var item = await context.Destinos.FindAsync(id);
        if (item is null) return NotFound("El destino no existe.");

        item.Pais = request.Pais.Trim();
        item.Ciudad = request.Ciudad.Trim();
        item.Descripcion = request.Descripcion.Trim();
        item.Temporada = request.Temporada.Trim();
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await context.Destinos.FindAsync(id);
        if (item is null) return NotFound("El destino no existe.");
        if (await context.Paquetes.AnyAsync(x => x.DestinoId == id))
            return Conflict("No se puede eliminar: el destino tiene paquetes registrados.");

        context.Destinos.Remove(item);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
