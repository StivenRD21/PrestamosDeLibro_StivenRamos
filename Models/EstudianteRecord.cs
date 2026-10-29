namespace PrestamosDeLibros.Models;

public record EstudianteRecord
{
    public int IdEstudiante { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string Matricula { get; init; } = string.Empty;
    public string Celular { get; init; } = string.Empty;
}