using Follower.DTOs;

namespace Follower.Repositories
{
    public interface ISolicitudesIngresoRepository
    {
        // Función: obtiene el programa asignado al responsable y sus solicitudes pendientes.
        // Flujo: recibe el identificador de usuario y devuelve los datos del programa junto con la lista pendiente.
        Task<SolicitudesIngresoDto?> ObtenerSolicitudesAsync(int idResponsable);

        // Función: autoriza una solicitud pendiente del programa del responsable.
        // Flujo: valida la asignación y el estado pendiente, registra la resolución y persiste el cambio.
        Task<bool> AutorizarAsync(int idEgresado, int idResponsable);

        // Función: rechaza una solicitud pendiente del programa del responsable.
        // Flujo: valida la asignación y el estado pendiente, guarda el motivo y los datos de resolución.
        Task<bool> RechazarAsync(int idEgresado, int idResponsable, string? motivo);
    }
}
