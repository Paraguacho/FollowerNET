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
                .Include(e => e.ResumenProductividad)
                .Include(e => e.ProductividadesCientificas)
                    .ThenInclude(p => p.Coautores)
                .Include(e => e.ReconocimientosAcademicos)
                .Include(e => e.ContribucionesSociales)
                .FirstOrDefaultAsync(e=> e.IdUsuario == idUsuario);

            if (egresado != null)
            {
                if (egresado.ResumenProductividad == null)
                {
                    egresado.ResumenProductividad = new ResumenProductividad { EgresadoId = egresado.IdEgresado };
                }
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
            var egresado = await _context.Egresados.FindAsync(experiencia.EgresadoId);
            if (egresado != null)
            {
                egresado.FechaUltimaActualizacion = DateTime.UtcNow;
            }

            _context.ExperienciasLaborales.Remove(experiencia);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarFormacionAcademicaAsync (FormacionAcademica formacion)
        {
            var egresado = await _context.Egresados.FindAsync(formacion.EgresadoId);
            if (egresado != null)
            {
                egresado.FechaUltimaActualizacion = DateTime.UtcNow;
            }

            _context.FormacionesAcademicas.Remove(formacion);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarProductividadCientificaAsync (ProductividadCientifica productividad)
        {
            _context.ProductividadesCientificas.Remove(productividad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarCoautorAsync(CoautorProducto coautor)
        {
            _context.CoautoresProductos.Remove(coautor);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarProyectoInvestigacionAsync(ProyectoInvestigacion proyecto)
        {
            _context.ProyectosInvestigacion.Remove(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarReconocimientoAcademicoAsync(ReconocimientoAcademico reconocimiento)
        {
            _context.ReconocimientosAcademicos.Remove(reconocimiento);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarContribucionSocialAsync(ContribucionSocial contribucion)
        {
            _context.ContribucionesSociales.Remove(contribucion);
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

                if (egresado.ResumenProductividad != null)
                {
                    egresado.ResumenProductividad.EgresadoId = egresado.IdEgresado;
                    if (egresado.ResumenProductividad.IdResumen == 0)
                        _context.Entry(egresado.ResumenProductividad).State = EntityState.Added;
                    else
                        _context.Entry(egresado.ResumenProductividad).State = EntityState.Modified;
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
