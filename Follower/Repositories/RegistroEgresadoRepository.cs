using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Repositories
{
    public sealed class RegistroEgresadoRepository : IRegistroEgresadoRepository
    {
        private readonly ApplicationDbContext _context;

        // Función: prepara el repositorio de registro de egresados.
        // Flujo: recibe por inyección el contexto de EF Core.
        public RegistroEgresadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Función: carga los programas educativos disponibles.
        // Flujo: consulta el conjunto de programas y devuelve todos los resultados.
        public Task<List<ProgramaEducativo>> ObtenerProgramasDisponiblesAsync()
        {
            return _context.ProgramasEducativos.ToListAsync();
        }

        // Función: comprueba si el correo está registrado.
        // Flujo: ejecuta una consulta de existencia sobre los usuarios.
        public Task<bool> ExisteCorreoAsync(string correo)
        {
            return _context.Usuarios.AnyAsync(usuario => usuario.Correo == correo);
        }

        // Función: guarda de forma atómica la cuenta y el perfil inicial del egresado.
        // Flujo: abre una transacción, guarda la cuenta para obtener su clave, guarda el perfil y confirma; ante error revierte y propaga la excepción.
        public async Task RegistrarAsync(Usuario usuario, Egresado egresado)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                egresado.IdUsuario = usuario.IdUsuario;
                _context.Egresados.Add(egresado);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
