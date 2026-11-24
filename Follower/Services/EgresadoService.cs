using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Services
{
    public class EgresadoService : IEgresadoService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public EgresadoService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Egresado?> ObtenerPorUsuarioIdAsync(int idUsuario)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var egresado = await context.Egresados
                .Include(e => e.PerfilAcademico)
                .Include(e => e.SituacionLaboral)
                .Include(e => e.ExperienciasLaborales)
                .Include(e => e.FormacionesAcademicas)
                .Include(e => e.ResumenProductividad)
                .Include(e => e.ProductividadesCientificas)
                    .ThenInclude(p => p.Coautores)
                .Include(e => e.ReconocimientosAcademicos)
                .Include(e => e.ProyectosInvestigacion)
                .Include(e => e.ContribucionesSociales)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdUsuario == idUsuario);

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

        public Task EliminarExperienciaLaboralAsync(ExperienciaLaboral experiencia)
            => EliminarEntidadAsync(experiencia);

        public Task EliminarFormacionAcademicaAsync(FormacionAcademica formacion)
            => EliminarEntidadAsync(formacion);

        public async Task EliminarProductividadCientificaAsync(ProductividadCientifica productividad)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                var entry = context.Entry(productividad);
                if (!entry.IsKeySet) return;

                if (productividad.Coautores != null)
                {
                    foreach (var coautor in productividad.Coautores)
                    {
                        var coautorEntry = context.Entry(coautor);
                        if (coautorEntry.IsKeySet)
                        {
                            coautorEntry.State = EntityState.Deleted;
                        }
                    }
                }

                entry.State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // El registro ya había sido eliminado (por ejemplo, doble clic rápido)
            }
        }

        public Task EliminarCoautorAsync(CoautorProducto coautor)
            => EliminarEntidadAsync(coautor);

        public Task EliminarProyectoInvestigacionAsync(ProyectoInvestigacion proyecto)
            => EliminarEntidadAsync(proyecto);

        public Task EliminarReconocimientoAcademicoAsync(ReconocimientoAcademico reconocimiento)
            => EliminarEntidadAsync(reconocimiento);

        public Task EliminarContribucionSocialAsync(ContribucionSocial contribucion)
            => EliminarEntidadAsync(contribucion);

        private async Task EliminarEntidadAsync<TEntity>(TEntity entidad) where TEntity : class
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                var entry = context.Entry(entidad);
                if (!entry.IsKeySet) return;

                entry.State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // El registro ya había sido eliminado
            }
        }

        public async Task ActualizarAsync(Egresado egresado)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();

                if (egresado.PerfilAcademico != null)
                {
                    egresado.PerfilAcademico.EgresadoId = egresado.IdEgresado;
                }

                if (egresado.SituacionLaboral != null)
                {
                    egresado.SituacionLaboral.EgresadoId = egresado.IdEgresado;
                }

                if (egresado.ResumenProductividad != null)
                {
                    egresado.ResumenProductividad.EgresadoId = egresado.IdEgresado;
                }

                context.Egresados.Update(egresado);

                if (egresado.PerfilAcademico != null)
                {
                    context.Entry(egresado.PerfilAcademico).State =
                        egresado.PerfilAcademico.Id == 0 ? EntityState.Added : EntityState.Modified;
                }

                if (egresado.SituacionLaboral != null)
                {
                    context.Entry(egresado.SituacionLaboral).State =
                        egresado.SituacionLaboral.IdSituacionLaboral == 0 ? EntityState.Added : EntityState.Modified;
                }

                if (egresado.ResumenProductividad != null)
                {
                    context.Entry(egresado.ResumenProductividad).State =
                        egresado.ResumenProductividad.IdResumen == 0 ? EntityState.Added : EntityState.Modified;
                }

                await context.SaveChangesAsync();
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