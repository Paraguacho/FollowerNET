using Follower.Models;

namespace Follower.Services
{
    public interface ISolicitudesIngresoService
    {
        Task<ProgramaEducativo?> ObtenerProgramaAsignadoAsync(int idResponsable);
        Task<List<Egresado>> ObtenerSolicitudesPendientesAsync(int idPrograma);
        Task<bool> AprobarSolicitudAsync(int idEgresado, int idResponsable);
        Task<bool> RechazarSolicitudAsync(int idEgresado, int idResponsable, string? motivoRechazo);
    }
}
