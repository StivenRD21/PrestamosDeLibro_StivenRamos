namespace PrestamosDeLibros.Models;

public record LibroRecord
{
    public int IdLibro { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Autor { get; init; } = string.Empty;
    public string ISBN { get; init; } = string.Empty;
    public int CantidadTotal { get; init; }
}