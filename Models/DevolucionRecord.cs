namespace PrestamosDeLibros.Models;

public record DevolucionRecord
{
    public int IdDevolucion { get; init; }
    public int IdPrestamo { get; init; }
    public DateTime FechaDevolucion { get; init; }
    public string Observaciones { get; init; } = string.Empty;
}