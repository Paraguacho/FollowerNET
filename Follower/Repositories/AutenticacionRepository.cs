using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Repositories
{
    public sealed class AutenticacionRepository : IAutenticacionRepository
    {
        private readonly ApplicationDbContext _context;

        // Función: prepara el repositorio de acceso a cuentas.
        // Flujo: recibe por inyección el contexto de EF Core.
        public AutenticacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Función: carga una cuenta a partir de su correo electrónico.
        // Flujo: consulta el usuario e incluye el rol y el perfil asociados para el proceso de inicio de sesión.
        public Task<Usuario?> ObtenerUsuarioPorCorreoAsync(string correo)
        {
            return _context.Usuarios
                .Include(usuario => usuario.Rol)
                .Include(usuario => usuario.Egresado)
                .FirstOrDefaultAsync(usuario => usuario.Correo == correo);
        }
    }
}
