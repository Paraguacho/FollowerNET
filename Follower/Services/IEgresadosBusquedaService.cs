using Follower.DTOs;
using Follower.Models;

namespace Follower.Services
{
    public interface IEgresadosBusquedaService
    {
        // HU3.2
        // Flujo: obtiene opciones cerradas y valores laborales registrados para el programa autorizado.
        // Función: provee las opciones de los filtros de la página.
        Task<EgresadosBusquedaOpcionesDto> ObtenerOpcionesAsync(CancellationToken cancellationToken = default);

        // HU3.2 / HU3.3
        // Flujo: recibe criterios, orden y paginación, y devuelve filas junto con el total coincidente.
        // Función: realiza la búsqueda multicriterio y alimenta la tabla paginada.
        Task<EgresadosBusquedaPaginaDto> BuscarAsync(
            EgresadosBusquedaFiltrosDto filtros,
            CancellationToken cancellationToken = default);

        // HU3.3
        // Flujo: recibe el identificador seleccionado y devuelve el perfil solo si pertenece al programa autorizado.
        // Función: permite navegar al detalle del egresado desde la tabla.
        Task<Egresado?> ObtenerPerfilAutorizadoAsync(
            int idEgresado,
            CancellationToken cancellationToken = default);
    }
}
