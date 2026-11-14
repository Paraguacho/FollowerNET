using Follower.Data;
using Follower.DTOs;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Services
{
    public class RegistroEgresadoService : IRegistroEgresadoService
    {
        private readonly ApplicationDbContext _context;

        public RegistroEgresadoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProgramaEducativo>> ObtenerProgramasDisponiblesAsync()
        {
            return await _context.ProgramasEducativos.AsNoTracking().ToListAsync();
        }

        public async Task<RegistroResultadoDto> RegistrarEgresadoAsync(RegistroEgresadoDto modelo)
        {
            var resultado = new RegistroResultadoDto { Exito = false };

            try
            {
                var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Correo == modelo.Correo);
                if (existeUsuario)
                {
                    resultado.MensajeError = "Este correo electrónico ya se encuentra registrado.";
                    return resultado;
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // BCrypt.Net.BCrypt.HashPassword(modelo.Contrasena)
                    var nuevoUsuario = new Usuario
                    {
                        Correo = modelo.Correo,
                        ContrasenaHash = modelo.Contrasena,
                        IdRol = 2 // Rol Egresado
                    };

                    _context.Usuarios.Add(nuevoUsuario);
                    await _context.SaveChangesAsync();

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

                    _context.Egresados.Add(nuevoEgresado);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    resultado.Exito = true;
                    return resultado;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    resultado.MensajeError = "Ocurrió un error al procesar tu registro. Inténtalo de nuevo.";
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                resultado.MensajeError = "Error de conexión con la base de datos.";
                return resultado;
            }
        }
    }
}