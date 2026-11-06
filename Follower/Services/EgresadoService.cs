using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Services
{
    public class EgresadoService : IEgresadoService
    {
        private readonly ApplicationDbContext _context;

        public EgresadoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Egresado?> ObtenerPorUsuarioIdAsync(int idUsuario)

        {
            var egresado = await _context.Egresados.Include(e => e.PerfilAcademico).FirstOrDefaultAsync(e=> e.IdUsuario == idUsuario);
            if (egresado != null && egresado.PerfilAcademico == null)
            {
                egresado.PerfilAcademico = new PerfilAcademico { EgresadoId = egresado.IdEgresado };
            }
            return egresado;
        }

        public async Task ActualizarAsync(Egresado egresado)
        {
            try
            {
                if (egresado.PerfilAcademico != null)
                {
                    egresado.PerfilAcademico.EgresadoId = egresado.IdEgresado;
                    if (egresado.PerfilAcademico.Id == 0)
                    {
                        _context.Entry(egresado.PerfilAcademico).State = EntityState.Added;
                    }
                    else
                    {
                        _context.Entry(egresado.PerfilAcademico).State = EntityState.Modified;
                    }
                }
                _context.Egresados.Update(egresado);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"DETALLE: {ex.InnerException.Message}");
                }
            }
        }
    }
}
