using Dapper;
using Microsoft.Data.Sqlite;
using PrestamosDeLibros.Models;

namespace PrestamosDeLibros.Services;

public class EstudianteService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            CREATE TABLE IF NOT EXISTS Estudiantes (
                IdEstudiante INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL,
                Apellido TEXT NOT NULL,
                Matricula TEXT NOT NULL,
                Celular TEXT
            );";
        await connection.ExecuteAsync(sql);
    }

    public async Task<int> SaveAsync(EstudianteRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            INSERT INTO Estudiantes (Nombre, Apellido, Matricula, Celular) 
            VALUES (@Nombre, @Apellido, @Matricula, @Celular); 
            SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<int>(sql, record);
    }

    public async Task<int> UpdateAsync(EstudianteRecord record)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            UPDATE Estudiantes 
            SET Nombre = @Nombre, 
                Apellido = @Apellido, 
                Matricula = @Matricula, 
                Celular = @Celular 
            WHERE IdEstudiante = @IdEstudiante;";
        return await connection.ExecuteAsync(sql, record);
    }

    public async Task<EstudianteRecord?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Estudiantes WHERE IdEstudiante = @Id;";
        return await connection.QuerySingleOrDefaultAsync<EstudianteRecord>(sql, new { Id = id });
    }

    public async Task<IEnumerable<EstudianteRecord>> GetListAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Estudiantes ORDER BY Nombre ASC, Apellido ASC;";
        return await connection.QueryAsync<EstudianteRecord>(sql);
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "DELETE FROM Estudiantes WHERE IdEstudiante = @Id;";
        return await connection.ExecuteAsync(sql, new { Id = id });
    }
}