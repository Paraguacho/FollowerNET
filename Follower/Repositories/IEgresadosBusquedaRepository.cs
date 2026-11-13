using System.Data.Common;
using Follower.Models;

namespace Follower.Repositories
{
    public interface IEgresadosBusquedaRepository
    {
        // HU3.2 / HU3.3
        // Flujo: ejecuta una consulta escalar con los parámetros entregados.
        // Función: obtiene el conteo de resultados.
        Task<long> EjecutarEscalarAsync(
            string sql,
            IReadOnlyDictionary<string, object?> parametros,
            CancellationToken cancellationToken = default);

        // HU3.2 / HU3.3
        // Flujo: ejecuta SQL parametrizado y transforma las filas con el delegado recibido.
        // Función: recupera resultados tipados para el servicio.
        Task<IReadOnlyList<T>> EjecutarConsultaAsync<T>(
            string sql,
            IReadOnlyDictionary<string, object?> parametros,
            Func<DbDataReader, T> mapear,
            CancellationToken cancellationToken = default);

        // HU3.3
        // Flujo: busca el egresado aprobado dentro del programa autorizado y carga sus relaciones.
        // Función: suministra el perfil seleccionado para la vista de detalle.
        Task<Egresado?> ObtenerPerfilAutorizadoAsync(
            int idEgresado,
            int idPrograma,
            CancellationToken cancellationToken = default);
    }
}
