using Follower.Models;

namespace Follower.Repositories
{
    public interface IRegistroEgresadoRepository
    {
        // Función: recupera los programas disponibles para registro.
        // Flujo: consulta los programas educativos y devuelve la lista al servicio.
        Task<List<ProgramaEducativo>> ObtenerProgramasDisponiblesAsync();

        // Función: verifica si una cuenta ya usa el correo proporcionado.
        // Flujo: consulta la existencia del correo y devuelve el resultado al servicio.
        Task<bool> ExisteCorreoAsync(string correo);

        // Función: persiste el usuario y el perfil inicial de egresado de forma atómica.
        // Flujo: inicia una transacción, guarda primero el usuario y luego el egresado, y confirma o revierte la operación.
        Task RegistrarAsync(Usuario usuario, Egresado egresado);
    }
}
