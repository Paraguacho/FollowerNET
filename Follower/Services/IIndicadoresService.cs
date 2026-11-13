using Follower.DTOs;

namespace Follower.Services
{
    public interface IIndicadoresService
    {
        // Función: obtiene los datos autorizados y prepara las opciones de generación.
        // Flujo: consulta el repositorio, deriva las generaciones y devuelve el origen de datos para el panel.
        Task<IndicadoresDatosDto?> CargarDatosAsync(int idResponsable);

        // Función: calcula los indicadores del panel para una generación seleccionada.
        // Flujo: filtra el conjunto de egresados y agrupa la población por generación, empleo, sector, doctorado y becas.
        IndicadoresPanelDto CalcularIndicadores(IndicadoresDatosDto datos, string generacionSeleccionada);
    }
}
