namespace PrestamosDeLibros.Models;

public record PrestamoRecord
{
    public int IdPrestamo { get; init; }
    public int IdEstudiante { get; init; }
    public int IdLibro { get; init; }
    public DateTime FechaPrestamo { get; init; }
    public DateTime FechaVencimiento { get; init; }
}