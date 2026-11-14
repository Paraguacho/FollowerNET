using Follower.DTOs;
using Follower.Models;

namespace Follower.Services
{
    public interface IRegistroEgresadoService
    {
        Task<List<ProgramaEducativo>> ObtenerProgramasDisponiblesAsync();
        Task<RegistroResultadoDto> RegistrarEgresadoAsync(RegistroEgresadoDto modelo);
    }
}
