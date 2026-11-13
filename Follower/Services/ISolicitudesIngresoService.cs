using Follower.DTOs;

namespace Follower.Services
{
    public interface ISolicitudesIngresoService
    {
        // Función: obtiene el programa asignado al responsable y sus solicitudes pendientes.
        // Flujo: recibe el identificador del responsable y delega la carga al repositorio.
        Task<SolicitudesIngresoDto?> ObtenerSolicitudesAsync(int idResponsable);

        // Función: autoriza la solicitud seleccionada.
        // Flujo: valida los identificadores y delega la autorización al repositorio.
        Task<bool> AutorizarAsync(int idEgresado, int idResponsable);

        // Función: rechaza la solicitud seleccionada.
        // Flujo: valida los identificadores, normaliza el motivo vacío y delega el rechazo al repositorio.
        Task<bool> RechazarAsync(int idEgresado, int idResponsable, string? motivo);
    }
}
