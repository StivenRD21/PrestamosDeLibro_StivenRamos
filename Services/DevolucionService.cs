using Dapper;
using Microsoft.Data.Sqlite;
using PrestamosDeLibros.Models;

namespace PrestamosDeLibros.Services;

public class DevolucionService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            CREATE TABLE IF NOT EXISTS Devoluciones (
                IdDevolucion INTEGER PRIMARY KEY AUTOINCREMENT,
                IdPrestamo INTEGER NOT NULL,
                FechaDevolucion TEXT NOT NULL,
                Observaciones TEXT,
                FOREIGN KEY (IdPrestamo) REFERENCES Prestamos(IdPrestamo)
            );";
        await connection.ExecuteAsync(sql);
    }

    public async Task<int> SaveAsync(DevolucionRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            INSERT INTO Devoluciones (IdPrestamo, FechaDevolucion, Observaciones) 
            VALUES (@IdPrestamo, @FechaDevolucion, @Observaciones);

            UPDATE Libros 
            SET CantidadTotal = CantidadTotal + 1 
            WHERE IdLibro = (SELECT IdLibro FROM Prestamos WHERE IdPrestamo = @IdPrestamo);

            SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<int>(sql, record);
    }

    public async Task<int> UpdateAsync(DevolucionRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            UPDATE Devoluciones 
            SET IdPrestamo = @IdPrestamo, 
                FechaDevolucion = @FechaDevolucion, 
                Observaciones = @Observaciones 
            WHERE IdDevolucion = @IdDevolucion;";
        return await connection.ExecuteAsync(sql, record);
    }

    public async Task<DevolucionRecord?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Devoluciones WHERE IdDevolucion = @Id;";
        return await connection.QuerySingleOrDefaultAsync<DevolucionRecord>(sql, new { Id = id });
    }

    public async Task<IEnumerable<DevolucionRecord>> GetListAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Devoluciones ORDER BY FechaDevolucion DESC;";
        return await connection.QueryAsync<DevolucionRecord>(sql);
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "DELETE FROM Devoluciones WHERE IdDevolucion = @Id;";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }
}