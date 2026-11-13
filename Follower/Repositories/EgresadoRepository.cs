using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Repositories
{
    public sealed class EgresadoRepository : IEgresadoRepository
    {
        private readonly ApplicationDbContext _context;

        // Función: prepara el repositorio para acceder a los datos del perfil de egresado.
        // Flujo: recibe por inyección el contexto de EF Core y lo conserva para las operaciones.
        public EgresadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Función: carga el perfil asociado al usuario indicado.
        // Flujo: consulta el egresado con sus secciones y relaciones; inicializa las secciones opcionales faltantes.
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
                .FirstOrDefaultAsync(e => e.IdUsuario == idUsuario);

            if (egresado != null)
            {
                egresado.ResumenProductividad ??= new ResumenProductividad { EgresadoId = egresado.IdEgresado };
                egresado.PerfilAcademico ??= new PerfilAcademico { EgresadoId = egresado.IdEgresado };
                egresado.SituacionLaboral ??= new SituacionLaboral { EgresadoId = egresado.IdEgresado };
            }

            return egresado;
        }

        // Función: persiste el agregado completo del egresado.
        // Flujo: determina si las secciones uno-a-uno son nuevas o existentes, actualiza el agregado y guarda los cambios.
        public async Task ActualizarAsync(Egresado egresado)
        {
            try
            {
                if (egresado.PerfilAcademico != null)
                {
                    egresado.PerfilAcademico.EgresadoId = egresado.IdEgresado;
                    _context.Entry(egresado.PerfilAcademico).State = egresado.PerfilAcademico.Id == 0
                        ? EntityState.Added
                        : EntityState.Modified;
                }

                if (egresado.SituacionLaboral != null)
                {
                    egresado.SituacionLaboral.EgresadoId = egresado.IdEgresado;
                    _context.Entry(egresado.SituacionLaboral).State = egresado.SituacionLaboral.IdSituacionLaboral == 0
                        ? EntityState.Added
                        : EntityState.Modified;
                }

                if (egresado.ResumenProductividad != null)
                {
                    egresado.ResumenProductividad.EgresadoId = egresado.IdEgresado;
                    _context.Entry(egresado.ResumenProductividad).State = egresado.ResumenProductividad.IdResumen == 0
                        ? EntityState.Added
                        : EntityState.Modified;
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

        // Función: elimina una experiencia laboral y actualiza la fecha de modificación del perfil.
        // Flujo: busca el egresado relacionado, actualiza su fecha si existe, elimina la experiencia y guarda.
        public async Task EliminarExperienciaLaboralAsync(ExperienciaLaboral experiencia)
        {
            var egresado = await _context.Egresados.FindAsync(experiencia.EgresadoId);
            if (egresado != null)
            {
                egresado.FechaUltimaActualizacion = DateTime.UtcNow;
            }

            _context.ExperienciasLaborales.Remove(experiencia);
            await _context.SaveChangesAsync();
        }

        // Función: elimina una formación académica y actualiza la fecha de modificación del perfil.
        // Flujo: busca el egresado relacionado, actualiza su fecha si existe, elimina la formación y guarda.
        public async Task EliminarFormacionAcademicaAsync(FormacionAcademica formacion)
        {
            var egresado = await _context.Egresados.FindAsync(formacion.EgresadoId);
            if (egresado != null)
            {
                egresado.FechaUltimaActualizacion = DateTime.UtcNow;
            }

            _context.FormacionesAcademicas.Remove(formacion);
            await _context.SaveChangesAsync();
        }

        // Función: elimina un producto científico.
        // Flujo: elimina la entidad del conjunto y guarda los cambios en la base de datos.
        public async Task EliminarProductividadCientificaAsync(ProductividadCientifica productividad)
        {
            _context.ProductividadesCientificas.Remove(productividad);
            await _context.SaveChangesAsync();
        }

        // Función: elimina un coautor.
        // Flujo: elimina la entidad del conjunto y guarda los cambios en la base de datos.
        public async Task EliminarCoautorAsync(CoautorProducto coautor)
        {
            _context.CoautoresProductos.Remove(coautor);
            await _context.SaveChangesAsync();
        }

        // Función: elimina un proyecto de investigación.
        // Flujo: elimina la entidad del conjunto y guarda los cambios en la base de datos.
        public async Task EliminarProyectoInvestigacionAsync(ProyectoInvestigacion proyecto)
        {
            _context.ProyectosInvestigacion.Remove(proyecto);
            await _context.SaveChangesAsync();
        }

        // Función: elimina un reconocimiento académico.
        // Flujo: elimina la entidad del conjunto y guarda los cambios en la base de datos.
        public async Task EliminarReconocimientoAcademicoAsync(ReconocimientoAcademico reconocimiento)
        {
            _context.ReconocimientosAcademicos.Remove(reconocimiento);
            await _context.SaveChangesAsync();
        }

        // Función: elimina una contribución social.
        // Flujo: elimina la entidad del conjunto y guarda los cambios en la base de datos.
        public async Task EliminarContribucionSocialAsync(ContribucionSocial contribucion)
        {
            _context.ContribucionesSociales.Remove(contribucion);
            await _context.SaveChangesAsync();
        }
    }
}
