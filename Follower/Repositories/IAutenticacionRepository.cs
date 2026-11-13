using Follower.Models;

namespace Follower.Repositories
{
    public interface IAutenticacionRepository
    {
        // Función: obtiene la cuenta asociada al correo proporcionado.
        // Flujo: consulta el usuario e incluye su rol y perfil de egresado; devuelve null si no existe.
        Task<Usuario?> ObtenerUsuarioPorCorreoAsync(string correo);
    }
}
