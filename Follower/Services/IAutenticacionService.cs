using Follower.DTOs;

namespace Follower.Services
{
    public interface IAutenticacionService
    {
        Task<AutenticacionResultadoDto> ValidarLoginAsync(LoginDto modelo);
    }
}
