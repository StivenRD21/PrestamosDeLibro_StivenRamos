using System.Data;
using Dapper;
using PrestamosDeLibros.Services;
using Scalar.AspNetCore;

// Registra el convertidor para que Dapper mapee TEXT de SQLite a DateTime sin errores
SqlMapper.AddTypeHandler(new SqliteDateTimeHandler());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<EstudianteService>();
builder.Services.AddScoped<LibroService>();
builder.Services.AddScoped<PrestamoService>();
builder.Services.AddScoped<DevolucionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.GetRequiredService<EstudianteService>().InitializeAsync();
    await services.GetRequiredService<LibroService>().InitializeAsync();
    await services.GetRequiredService<PrestamoService>().InitializeAsync();
    await services.GetRequiredService<DevolucionService>().InitializeAsync();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseCors("PermitirTodo");
app.UseAuthorization();
app.MapControllers();

app.Run();

public class SqliteDateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value)
        => parameter.Value = value.ToString("yyyy-MM-ddTHH:mm:ss");

    public override DateTime Parse(object value)
        => DateTime.Parse(value.ToString()!);
}