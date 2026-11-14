using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Services
{
    public class SolicitudesIngresoService : ISolicitudesIngresoService
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesIngresoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProgramaEducativo?> ObtenerProgramaAsignadoAsync(int idResponsable)
        {
            var asignacion = await _context.ProgramasResponsables
                .Include(pr => pr.ProgramaEducativo)
                .AsNoTracking() 
                .FirstOrDefaultAsync(pr => pr.IdUsuario == idResponsable);

            return asignacion?.ProgramaEducativo;
        }

        public async Task<List<Egresado>> ObtenerSolicitudesPendientesAsync(int idPrograma)
        {
            return await _context.Egresados
                .Include(e => e.Usuario)
                .Include(e => e.ProgramaEducativo)
                .Where(e => e.EstadoSolicitud == "Pendiente" && e.IdPrograma == idPrograma)
                .OrderBy(e => e.FechaSolicitud)
                .AsNoTracking() 
                .ToListAsync();
        }

        public async Task<bool> AprobarSolicitudAsync(int idEgresado, int idResponsable)
        {
            var egresado = await _context.Egresados.FindAsync(idEgresado);
            if (egresado == null) return false;

            egresado.EstadoSolicitud = "Aprobado";
            egresado.IdUsuarioResolvio = idResponsable;
            egresado.FechaResolucion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RechazarSolicitudAsync(int idEgresado, int idResponsable, string? motivoRechazo)
        {
            var egresado = await _context.Egresados.FindAsync(idEgresado);
            if (egresado == null) return false;

            egresado.EstadoSolicitud = "Rechazado";
            egresado.IdUsuarioResolvio = idResponsable;
            egresado.FechaResolucion = DateTime.Now;
            egresado.MotivoRechazo = string.IsNullOrEmpty(motivoRechazo) ? null : motivoRechazo;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}