using Microsoft.AspNetCore.Mvc;
using PrestamosDeLibros.Models;
using PrestamosDeLibros.Services;

namespace PrestamosDeLibros.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController(PrestamoService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lista = await service.GetListAsync();
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var prestamo = await service.GetByIdAsync(id);
        return prestamo is not null ? Ok(prestamo) : NotFound($"No se encontró el préstamo con ID {id}");
    }

    [HttpPost]
    public async Task<IActionResult> Create(PrestamoRecord record)
    {
        var nuevoId = await service.SaveAsync(record);
        var creado = record with { IdPrestamo = nuevoId };
        return CreatedAtAction(nameof(GetById), new { id = nuevoId }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PrestamoRecord record)
    {
        var actualizado = record with { IdPrestamo = id };
        var filas = await service.UpdateAsync(actualizado);
        return filas > 0 ? NoContent() : NotFound($"No se encontró el préstamo con ID {id}");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var filas = await service.DeleteAsync(id);
        return filas > 0 ? NoContent() : NotFound($"No se encontró el préstamo con ID {id}");
    }
}