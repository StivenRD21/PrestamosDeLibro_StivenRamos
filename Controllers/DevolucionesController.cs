using Microsoft.AspNetCore.Mvc;
using PrestamosDeLibros.Models;
using PrestamosDeLibros.Services;

namespace PrestamosDeLibros.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevolucionesController(DevolucionService service) : ControllerBase
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
        var devolucion = await service.GetByIdAsync(id);
        return devolucion is not null ? Ok(devolucion) : NotFound($"No se encontró la devolución con ID {id}");
    }

    [HttpPost]
    public async Task<IActionResult> Create(DevolucionRecord record)
    {
        var nuevoId = await service.SaveAsync(record);
        var creado = record with { IdDevolucion = nuevoId };
        return CreatedAtAction(nameof(GetById), new { id = nuevoId }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DevolucionRecord record)
    {
        var actualizado = record with { IdDevolucion = id };
        var filas = await service.UpdateAsync(actualizado);
        return filas > 0 ? NoContent() : NotFound($"No se encontró la devolución con ID {id}");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var filas = await service.DeleteAsync(id);
        return filas > 0 ? NoContent() : NotFound($"No se encontró la devolución con ID {id}");
    }
}