using Follower.DTOs;
using Follower.Repositories;

namespace Follower.Services
{
    public sealed class AutenticacionService : IAutenticacionService
    {
        private readonly IAutenticacionRepository _repository;

        // Función: prepara el servicio de autenticación.
        // Flujo: recibe por inyección el repositorio que consulta cuentas.
        public AutenticacionService(IAutenticacionRepository repository)
        {
            _repository = repository;
        }

        // Función: valida las credenciales y determina el acceso y los datos de sesión.
        // Flujo: busca la cuenta, comprueba la contraseña y el estado de la solicitud, y arma el resultado de autenticación.
        public async Task<AutenticacionResultadoDto> IniciarSesionAsync(LoginDto modelo)
        {
            var usuario = await _repository.ObtenerUsuarioPorCorreoAsync(modelo.Correo);
            if (usuario == null || usuario.ContrasenaHash != modelo.Contrasena)
            {
                return new AutenticacionResultadoDto
                {
                    MensajeError = "El correo electrónico o la contraseña son incorrectos. Verifica tus datos e inténtalo de nuevo."
                };
            }

            if (usuario.Egresado?.EstadoSolicitud == "Pendiente")
            {
                return new AutenticacionResultadoDto
                {
                    MensajeError = "Tu cuenta de egresado aún está en proceso de revisión. Te notificaremos por correo cuando sea aprobada."
                };
            }

            if (usuario.Egresado?.EstadoSolicitud == "Rechazado")
            {
                return new AutenticacionResultadoDto
                {
                    MensajeError = $"Tu intento de registro fue rechado por el motivo siguiente: {usuario.Egresado.MotivoRechazo} porfavor comunicate con tu responsable para mas información."
                };
            }

            var rol = usuario.Rol.NombreRol;
            var nombreCompleto = usuario.Egresado != null
                ? $"{usuario.Egresado.Nombres} {usuario.Egresado.ApellidoPaterno} {usuario.Egresado.ApellidoMaterno}".ToUpper().Trim()
                : rol == "ResponsablePrograma" ? "Responsable" : "Usuario";

            return new AutenticacionResultadoDto
            {
                Exitoso = true,
                IdUsuario = usuario.IdUsuario,
                Rol = rol,
                NombreCompleto = nombreCompleto
            };
        }
    }
}
