using Follower.Data;
using Follower.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Follower.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly ApplicationDbContext _context;

        public AutenticacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AutenticacionResultadoDto> ValidarLoginAsync(LoginDto modelo)
        {
            var resultado = new AutenticacionResultadoDto { Exitoso = false };

            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .Include(u => u.Egresado)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Correo == modelo.Correo);

                // Validacin 
                // !BCrypt.Net.BCrypt.Verify(modelo.Contrasena, usuario.ContrasenaHash)
                if (usuario == null || usuario.ContrasenaHash != modelo.Contrasena)
                {
                    resultado.MensajeError = "El correo electrónico o la contraseña son incorrectos. Verifica tus datos e inténtalo de nuevo.";
                    return resultado;
                }

                // Reglas de negocio para Egresados
                if (usuario.Egresado != null)
                {
                    if (usuario.Egresado.EstadoSolicitud == "Pendiente")
                    {
                        resultado.MensajeError = "Tu cuenta de egresado aún está en proceso de revisión. Te notificaremos por correo cuando sea aprobada.";
                        return resultado;
                    }

                    if (usuario.Egresado.EstadoSolicitud == "Rechazado")
                    {
                        resultado.MensajeError = $"Tu intento de registro fue rechazado por el motivo siguiente: {usuario.Egresado.MotivoRechazo}. Por favor comunícate con tu responsable para más información.";
                        return resultado;
                    }
                }

                // Lógica de formateo del nombre
                string nombreCompleto = "Usuario";
                if (usuario.Egresado != null)
                {
                    nombreCompleto = $"{usuario.Egresado.Nombres} {usuario.Egresado.ApellidoPaterno} {usuario.Egresado.ApellidoMaterno}".ToUpper().Trim();
                }
                else if (usuario.Rol?.NombreRol == "ResponsablePrograma")
                {
                    nombreCompleto = "Responsable";
                }

                // Si pasa todo, es exitoso
                resultado.Exitoso = true;
                resultado.IdUsuario = usuario.IdUsuario;
                resultado.Rol = usuario.Rol?.NombreRol ?? "SinRol";
                resultado.NombreCompleto = nombreCompleto;

                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AutenticacionService: {ex.Message}");
                resultado.MensajeError = "Error al intentar iniciar sesión. Inténtalo más tarde.";
                return resultado;
            }
        }
    }
}