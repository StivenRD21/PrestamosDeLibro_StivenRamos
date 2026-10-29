using Dapper;
using Microsoft.Data.Sqlite;
using PrestamosDeLibros.Models;

namespace PrestamosDeLibros.Services;

public class LibroService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            CREATE TABLE IF NOT EXISTS Libros (
                IdLibro INTEGER PRIMARY KEY AUTOINCREMENT,
                Titulo TEXT NOT NULL,
                Autor TEXT NOT NULL,
                ISBN TEXT NOT NULL,
                CantidadTotal INTEGER NOT NULL
            );";
        await connection.ExecuteAsync(sql);
    }

    public async Task<int> SaveAsync(LibroRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            INSERT INTO Libros (Titulo, Autor, ISBN, CantidadTotal) 
            VALUES (@Titulo, @Autor, @ISBN, @CantidadTotal); 
            SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<int>(sql, record);
    }

    public async Task<int> UpdateAsync(LibroRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            UPDATE Libros 
            SET Titulo = @Titulo, 
                Autor = @Autor, 
                ISBN = @ISBN, 
                CantidadTotal = @CantidadTotal 
            WHERE IdLibro = @IdLibro;";
        return await connection.ExecuteAsync(sql, record);
    }

    public async Task<LibroRecord?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Libros WHERE IdLibro = @Id;";
        return await connection.QuerySingleOrDefaultAsync<LibroRecord>(sql, new { Id = id });
    }

    public async Task<IEnumerable<LibroRecord>> GetListAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Libros ORDER BY Titulo ASC;";
        return await connection.QueryAsync<LibroRecord>(sql);
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "DELETE FROM Libros WHERE IdLibro = @Id;";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }
}