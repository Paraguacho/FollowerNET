using Follower.DTOs;
using Follower.Repositories;

namespace Follower.Services
{
    public sealed class SolicitudesIngresoService : ISolicitudesIngresoService
    {
        private readonly ISolicitudesIngresoRepository _repository;

        // Función: prepara el servicio de solicitudes de ingreso.
        // Flujo: recibe por inyección el repositorio que consulta y resuelve solicitudes.
        public SolicitudesIngresoService(ISolicitudesIngresoRepository repository)
        {
            _repository = repository;
        }

        // Función: obtiene el programa asignado al responsable y sus solicitudes pendientes.
        // Flujo: valida el identificador de usuario y delega la carga al repositorio.
        public Task<SolicitudesIngresoDto?> ObtenerSolicitudesAsync(int idResponsable)
        {
            return idResponsable > 0
                ? _repository.ObtenerSolicitudesAsync(idResponsable)
                : Task.FromResult<SolicitudesIngresoDto?>(null);
        }

        // Función: autoriza una solicitud pendiente.
        // Flujo: valida los identificadores y delega al repositorio la verificación de alcance y persistencia.
        public Task<bool> AutorizarAsync(int idEgresado, int idResponsable)
        {
            return idEgresado > 0 && idResponsable > 0
                ? _repository.AutorizarAsync(idEgresado, idResponsable)
                : Task.FromResult(false);
        }

        // Función: rechaza una solicitud pendiente.
        // Flujo: valida los identificadores, convierte un motivo vacío en null y delega al repositorio.
        public Task<bool> RechazarAsync(int idEgresado, int idResponsable, string? motivo)
        {
            return idEgresado > 0 && idResponsable > 0
                ? _repository.RechazarAsync(idEgresado, idResponsable, string.IsNullOrEmpty(motivo) ? null : motivo)
                : Task.FromResult(false);
        }
    }
}
