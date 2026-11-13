using Follower.DTOs;
using Follower.Models;

namespace Follower.Services
{
    public interface IRegistroEgresadoService
    {
        // Función: obtiene los programas para el formulario de registro.
        // Flujo: solicita al repositorio la lista de programas disponibles.
        Task<List<ProgramaEducativo>> ObtenerProgramasDisponiblesAsync();

        // Función: procesa la solicitud de registro de un egresado.
        // Flujo: comprueba que el correo no exista, construye las entidades iniciales y delega su persistencia.
        Task<RegistroResultadoDto> RegistrarAsync(RegistroEgresadoDto modelo);
    }
}
