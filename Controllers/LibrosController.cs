using Microsoft.AspNetCore.Mvc;
using PrestamosDeLibros.Models;
using PrestamosDeLibros.Services;

namespace PrestamosDeLibros.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController(LibroService service) : ControllerBase
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
        var libro = await service.GetByIdAsync(id);
        return libro is not null ? Ok(libro) : NotFound($"No se encontró el libro con ID {id}");
    }

    [HttpPost]
    public async Task<IActionResult> Create(LibroRecord record)
    {
        var nuevoId = await service.SaveAsync(record);
        var creado = record with { IdLibro = nuevoId };
        return CreatedAtAction(nameof(GetById), new { id = nuevoId }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, LibroRecord record)
    {
        var actualizado = record with { IdLibro = id };
        var filas = await service.UpdateAsync(actualizado);
        return filas > 0 ? NoContent() : NotFound($"No se encontró el libro con ID {id}");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var filas = await service.DeleteAsync(id);
        return filas > 0 ? NoContent() : NotFound($"No se encontró el libro con ID {id}");
    }
}