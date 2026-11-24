using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Services
{
    public class SolicitudesIngresoService : ISolicitudesIngresoService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SolicitudesIngresoService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ProgramaEducativo?> ObtenerProgramaAsignadoAsync(int idResponsable)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var asignacion = await context.ProgramasResponsables
                .Include(pr => pr.ProgramaEducativo)
                .AsNoTracking()
                .FirstOrDefaultAsync(pr => pr.IdUsuario == idResponsable);

            return asignacion?.ProgramaEducativo;
        }

        public async Task<List<Egresado>> ObtenerSolicitudesPendientesAsync(int idPrograma)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.Egresados
                .Include(e => e.Usuario)
                .Include(e => e.ProgramaEducativo)
                .Where(e => e.EstadoSolicitud == "Pendiente" && e.IdPrograma == idPrograma)
                .OrderBy(e => e.FechaSolicitud)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AprobarSolicitudAsync(int idEgresado, int idResponsable)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var egresado = await context.Egresados.FindAsync(idEgresado);
            if (egresado == null) return false;

            egresado.EstadoSolicitud = "Aprobado";
            egresado.IdUsuarioResolvio = idResponsable;
            egresado.FechaResolucion = DateTime.Now;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RechazarSolicitudAsync(int idEgresado, int idResponsable, string? motivoRechazo)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var egresado = await context.Egresados.FindAsync(idEgresado);
            if (egresado == null) return false;

            egresado.EstadoSolicitud = "Rechazado";
            egresado.IdUsuarioResolvio = idResponsable;
            egresado.FechaResolucion = DateTime.Now;
            egresado.MotivoRechazo = string.IsNullOrEmpty(motivoRechazo) ? null : motivoRechazo;

            await context.SaveChangesAsync();
            return true;
        }
    }
}