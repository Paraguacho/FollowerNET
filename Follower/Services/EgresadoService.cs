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
            var egresado = await _context.Egresados
                .Include(e => e.PerfilAcademico)
                .Include(e => e.SituacionLaboral)
                .Include(e => e.ExperienciasLaborales)
                .Include(e => e.FormacionesAcademicas)
                .FirstOrDefaultAsync(e=> e.IdUsuario == idUsuario);

            if (egresado != null)
            {
                if (egresado.PerfilAcademico == null)
                {
                    egresado.PerfilAcademico = new PerfilAcademico { EgresadoId = egresado.IdEgresado };
                }
                if (egresado.SituacionLaboral == null)
                {
                    egresado.SituacionLaboral = new SituacionLaboral { EgresadoId = egresado.IdEgresado };
                }
            }

            return egresado;
        }

        public async Task EliminarExperienciaLaboralAsync (ExperienciaLaboral experiencia)
        {
            _context.ExperienciasLaborales.Remove(experiencia);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarFormacionAcademicaAsync (FormacionAcademica formacion)
        {
            _context.FormacionesAcademicas.Remove(formacion);
            await _context.SaveChangesAsync();
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

                if (egresado.SituacionLaboral != null)
                {
                    egresado.SituacionLaboral.EgresadoId = egresado.IdEgresado;

                    if (egresado.SituacionLaboral.IdSituacionLaboral == 0)
                        _context.Entry(egresado.SituacionLaboral).State = EntityState.Added;
                    else
                        _context.Entry(egresado.SituacionLaboral).State = EntityState.Modified;
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
