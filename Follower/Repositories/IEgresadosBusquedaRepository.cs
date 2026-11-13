using System.Data.Common;
using Follower.Models;

namespace Follower.Repositories
{
    public interface IEgresadosBusquedaRepository
    {
        Task<long> EjecutarEscalarAsync(
            string sql,
            IReadOnlyDictionary<string, object?> parametros,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> EjecutarConsultaAsync<T>(
            string sql,
            IReadOnlyDictionary<string, object?> parametros,
            Func<DbDataReader, T> mapear,
            CancellationToken cancellationToken = default);

        Task<Egresado?> ObtenerPerfilAutorizadoAsync(
            int idEgresado,
            int idPrograma,
            CancellationToken cancellationToken = default);
    }
}
