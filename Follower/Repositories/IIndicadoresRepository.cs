using Follower.DTOs;

namespace Follower.Repositories
{
    public interface IIndicadoresRepository
    {
        // Función: obtiene los egresados aprobados de los programas asignados al responsable.
        // Flujo: consulta las asignaciones autorizadas, carga sus egresados y relaciones y devuelve el conjunto para los cálculos.
        Task<IndicadoresDatosDto?> ObtenerDatosAsync(int idResponsable);
    }
}
