using Follower.Data;
using Follower.DTOs;
using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Repositories
{
    public sealed class SolicitudesIngresoRepository : ISolicitudesIngresoRepository
    {
        private readonly ApplicationDbContext _context;

        // Función: prepara el repositorio de solicitudes de ingreso.
        // Flujo: recibe por inyección el contexto de EF Core.
        public SolicitudesIngresoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Función: carga el programa del responsable y las solicitudes pendientes de ese programa.
        // Flujo: busca la asignación, carga el programa y consulta egresados pendientes con sus datos de cuenta y programa.
        public async Task<SolicitudesIngresoDto?> ObtenerSolicitudesAsync(int idResponsable)
        {
            var asignacion = await _context.ProgramasResponsables
                .AsNoTracking()
                .Include(programaResponsable => programaResponsable.ProgramaEducativo)
                .FirstOrDefaultAsync(programaResponsable => programaResponsable.IdUsuario == idResponsable);

            if (asignacion == null)
            {
                return null;
            }

            var pendientes = await _context.Egresados
                .Include(egresado => egresado.Usuario)
                .Include(egresado => egresado.ProgramaEducativo)
                .Where(egresado => egresado.EstadoSolicitud == "Pendiente"
                    && egresado.IdPrograma == asignacion.IdPrograma)
                .OrderBy(egresado => egresado.FechaSolicitud)
                .ToListAsync();

            return new SolicitudesIngresoDto
            {
                ProgramaActual = asignacion.ProgramaEducativo,
                Pendientes = pendientes
            };
        }

        // Función: autoriza una solicitud que pertenece al programa del responsable.
        // Flujo: obtiene el programa asignado, localiza una solicitud pendiente dentro de ese programa, actualiza su resolución y guarda.
        public async Task<bool> AutorizarAsync(int idEgresado, int idResponsable)
        {
            var idPrograma = await ObtenerIdProgramaAsync(idResponsable);
            if (!idPrograma.HasValue)
            {
                return false;
            }

            var egresado = await _context.Egresados.FirstOrDefaultAsync(item =>
                item.IdEgresado == idEgresado
                && item.IdPrograma == idPrograma.Value
                && item.EstadoSolicitud == "Pendiente");
            if (egresado == null)
            {
                return false;
            }

            egresado.EstadoSolicitud = "Aprobado";
            egresado.IdUsuarioResolvio = idResponsable;
            egresado.FechaResolucion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        // Función: rechaza una solicitud que pertenece al programa del responsable.
        // Flujo: obtiene el programa asignado, localiza una solicitud pendiente dentro de ese programa, registra el motivo y guarda la resolución.
        public async Task<bool> RechazarAsync(int idEgresado, int idResponsable, string? motivo)
        {
            var idPrograma = await ObtenerIdProgramaAsync(idResponsable);
            if (!idPrograma.HasValue)
            {
                return false;
            }

            var egresado = await _context.Egresados.FirstOrDefaultAsync(item =>
                item.IdEgresado == idEgresado
                && item.IdPrograma == idPrograma.Value
                && item.EstadoSolicitud == "Pendiente");
            if (egresado == null)
            {
                return false;
            }

            egresado.EstadoSolicitud = "Rechazado";
            egresado.IdUsuarioResolvio = idResponsable;
            egresado.FechaResolucion = DateTime.Now;
            egresado.MotivoRechazo = motivo;
            await _context.SaveChangesAsync();
            return true;
        }

        // Función: resuelve el programa asignado a un responsable.
        // Flujo: consulta el primer identificador de programa asignado o devuelve null si no existe asignación.
        private Task<int?> ObtenerIdProgramaAsync(int idResponsable)
        {
            return _context.ProgramasResponsables
                .Where(programaResponsable => programaResponsable.IdUsuario == idResponsable)
                .Select(programaResponsable => (int?)programaResponsable.IdPrograma)
                .FirstOrDefaultAsync();
        }
    }
}
