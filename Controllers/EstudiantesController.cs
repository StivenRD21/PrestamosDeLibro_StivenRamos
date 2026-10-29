using Microsoft.AspNetCore.Mvc;
using PrestamosDeLibros.Models;
using PrestamosDeLibros.Services;

namespace PrestamosDeLibros.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController(EstudianteService service) : ControllerBase
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
        var estudiante = await service.GetByIdAsync(id);
        return estudiante is not null ? Ok(estudiante) : NotFound($"No se encontró el estudiante con ID {id}");
    }

    [HttpPost]
    public async Task<IActionResult> Create(EstudianteRecord record)
    {
        var nuevoId = await service.SaveAsync(record);
        var creado = record with { IdEstudiante = nuevoId };
        return CreatedAtAction(nameof(GetById), new { id = nuevoId }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EstudianteRecord record)
    {
        var actualizado = record with { IdEstudiante = id };
        var filas = await service.UpdateAsync(actualizado);
        return filas > 0 ? NoContent() : NotFound($"No se encontró el estudiante con ID {id}");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var filas = await service.DeleteAsync(id);
        return filas > 0 ? NoContent() : NotFound($"No se encontró el estudiante con ID {id}");
    }
}