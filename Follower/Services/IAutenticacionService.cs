using Follower.DTOs;

namespace Follower.Services
{
    public interface IAutenticacionService
    {
        // Función: valida las credenciales y el estado de la solicitud del usuario.
        // Flujo: recibe el formulario, consulta la cuenta y devuelve los datos de sesión o el mensaje de error.
        Task<AutenticacionResultadoDto> IniciarSesionAsync(LoginDto modelo);
    }
}
