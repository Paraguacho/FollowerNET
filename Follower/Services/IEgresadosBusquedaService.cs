using Follower.DTOs;
using Follower.Models;

namespace Follower.Services
{
    public interface IEgresadosBusquedaService
    {
        Task<EgresadosBusquedaOpcionesDto> ObtenerOpcionesAsync(CancellationToken cancellationToken = default);

        Task<EgresadosBusquedaPaginaDto> BuscarAsync(
            EgresadosBusquedaFiltrosDto filtros,
            CancellationToken cancellationToken = default);

        Task<Egresado?> ObtenerPerfilAutorizadoAsync(
            int idEgresado,
            CancellationToken cancellationToken = default);
    }
}
