using Follower.Data;
using Follower.DTOs;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Services
{
    public class RegistroEgresadoService : IRegistroEgresadoService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RegistroEgresadoService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<ProgramaEducativo>> ObtenerProgramasDisponiblesAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.ProgramasEducativos.AsNoTracking().ToListAsync();
        }

        public async Task<RegistroResultadoDto> RegistrarEgresadoAsync(RegistroEgresadoDto modelo)
        {
            var resultado = new RegistroResultadoDto { Exito = false };

            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();

                var existeUsuario = await context.Usuarios
                    .AsNoTracking()
                    .AnyAsync(u => u.Correo == modelo.Correo);

                if (existeUsuario)
                {
                    resultado.MensajeError = "Este correo electrónico ya se encuentra registrado.";
                    return resultado;
                }

                var strategy = context.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await context.Database.BeginTransactionAsync();
                    try
                    {
                        // BCrypt.Net.BCrypt.HashPassword(modelo.Contrasena)
                        var nuevoUsuario = new Usuario
                        {
                            Correo = modelo.Correo,
                            ContrasenaHash = modelo.Contrasena,
                            IdRol = 2 // Rol Egresado
                        };

                        context.Usuarios.Add(nuevoUsuario);
                        await context.SaveChangesAsync();

                        var nuevoEgresado = new Egresado
                        {
                            IdUsuario = nuevoUsuario.IdUsuario,
                            IdPrograma = modelo.IdPrograma,
                            Nombres = modelo.Nombres,
                            ApellidoPaterno = modelo.ApellidoPaterno,
                            ApellidoMaterno = modelo.ApellidoMaterno,
                            FechaEgreso = modelo.AnoEgreso,
                            EstadoSolicitud = "Pendiente",
                            PrivacidadPerfil = "Privado",
                            FechaSolicitud = DateTime.UtcNow,
                            FechaAceptacionPrivacidad = DateTime.UtcNow,
                            FechaUltimaActualizacion = DateTime.UtcNow
                        };

                        context.Egresados.Add(nuevoEgresado);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        context.ChangeTracker.Clear();
                        throw;
                    }
                });

                resultado.Exito = true;
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en RegistroEgresadoService: {ex.Message}");
                resultado.MensajeError = "Ocurrió un error al procesar tu registro. Verifica tu conexión e inténtalo de nuevo.";
                return resultado;
            }
        }
    }
}