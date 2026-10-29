using Dapper;
using Microsoft.Data.Sqlite;
using PrestamosDeLibros.Models;

namespace PrestamosDeLibros.Services;

public class PrestamoService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            CREATE TABLE IF NOT EXISTS Prestamos (
                IdPrestamo INTEGER PRIMARY KEY AUTOINCREMENT,
                IdEstudiante INTEGER NOT NULL,
                IdLibro INTEGER NOT NULL,
                FechaPrestamo TEXT NOT NULL,
                FechaVencimiento TEXT NOT NULL,
                FOREIGN KEY (IdEstudiante) REFERENCES Estudiantes(IdEstudiante),
                FOREIGN KEY (IdLibro) REFERENCES Libros(IdLibro)
            );";
        await connection.ExecuteAsync(sql);
    }

    public async Task<int> SaveAsync(PrestamoRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            INSERT INTO Prestamos (IdEstudiante, IdLibro, FechaPrestamo, FechaVencimiento) 
            VALUES (@IdEstudiante, @IdLibro, @FechaPrestamo, @FechaVencimiento);
            
            UPDATE Libros 
            SET CantidadTotal = CantidadTotal - 1 
            WHERE IdLibro = @IdLibro AND CantidadTotal > 0;

            SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<int>(sql, record);
    }

    public async Task<int> UpdateAsync(PrestamoRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            UPDATE Prestamos 
            SET IdEstudiante = @IdEstudiante, 
                IdLibro = @IdLibro, 
                FechaPrestamo = @FechaPrestamo, 
                FechaVencimiento = @FechaVencimiento 
            WHERE IdPrestamo = @IdPrestamo;";
        return await connection.ExecuteAsync(sql, record);
    }

    public async Task<PrestamoRecord?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Prestamos WHERE IdPrestamo = @Id;";
        return await connection.QuerySingleOrDefaultAsync<PrestamoRecord>(sql, new { Id = id });
    }

    public async Task<IEnumerable<PrestamoRecord>> GetListAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Prestamos ORDER BY FechaPrestamo DESC;";
        return await connection.QueryAsync<PrestamoRecord>(sql);
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "DELETE FROM Prestamos WHERE IdPrestamo = @Id;";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }
}