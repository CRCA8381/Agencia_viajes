using AgenciaViajes.Api.Data;
using AgenciaViajes.Api.DTOs;
using AgenciaViajes.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaViajes.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController(AgenciaDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await context.Clientes.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.ClienteId == id);
        return item is null ? NotFound("El cliente no existe.") : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ClienteRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var documento = request.Documento.Trim();

        if (await context.Clientes.AnyAsync(x => x.Documento == documento))
            return Conflict("Ya existe un cliente con ese documento.");
        if (await context.Clientes.AnyAsync(x => x.Email == email))
            return Conflict("Ya existe un cliente con ese correo.");

        var item = new Cliente
        {
            Nombre = request.Nombre.Trim(),
            Documento = documento,
            Telefono = request.Telefono.Trim(),
            Email = email
        };
        context.Clientes.Add(item);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.ClienteId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, ClienteRequest request)
    {
        var item = await context.Clientes.FindAsync(id);
        if (item is null) return NotFound("El cliente no existe.");

        var email = request.Email.Trim().ToLowerInvariant();
        var documento = request.Documento.Trim();

        if (await context.Clientes.AnyAsync(x => x.Documento == documento && x.ClienteId != id))
            return Conflict("Ya existe otro cliente con ese documento.");
        if (await context.Clientes.AnyAsync(x => x.Email == email && x.ClienteId != id))
            return Conflict("Ya existe otro cliente con ese correo.");

        item.Nombre = request.Nombre.Trim();
        item.Documento = documento;
        item.Telefono = request.Telefono.Trim();
        item.Email = email;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await context.Clientes.FindAsync(id);
        if (item is null) return NotFound("El cliente no existe.");
        if (await context.Reservas.AnyAsync(x => x.ClienteId == id))
            return Conflict("No se puede eliminar: el cliente tiene reservas registradas.");

        context.Clientes.Remove(item);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
