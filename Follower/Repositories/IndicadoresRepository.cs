using Follower.Data;
using Follower.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Follower.Repositories
{
    public sealed class IndicadoresRepository : IIndicadoresRepository
    {
        private readonly ApplicationDbContext _context;

        // Función: prepara el repositorio de indicadores.
        // Flujo: recibe por inyección el contexto de EF Core.
        public IndicadoresRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Función: carga los egresados aprobados de los programas asignados al responsable.
        // Flujo: obtiene asignaciones y nombre de programa, consulta los egresados con relaciones necesarias y devuelve el origen de datos.
        public async Task<IndicadoresDatosDto?> ObtenerDatosAsync(int idResponsable)
        {
            var asignaciones = await _context.ProgramasResponsables
                .AsNoTracking()
                .Include(asignacion => asignacion.ProgramaEducativo)
                .Where(asignacion => asignacion.IdUsuario == idResponsable)
                .ToListAsync();
            if (asignaciones.Count == 0)
            {
                return null;
            }

            var programaIds = asignaciones.Select(asignacion => asignacion.IdPrograma).Distinct().ToList();
            var nombrePrograma = string.Join(", ", asignaciones
                .Select(asignacion => $"{asignacion.ProgramaEducativo.NombrePrograma} ({asignacion.ProgramaEducativo.Siglas})")
                .Distinct());
            var egresados = await _context.Egresados
                .AsNoTracking()
                .Where(egresado => programaIds.Contains(egresado.IdPrograma)
                    && egresado.EstadoSolicitud == "Aprobado")
                .Include(egresado => egresado.SituacionLaboral)
                .Include(egresado => egresado.ExperienciasLaborales)
                .Include(egresado => egresado.FormacionesAcademicas)
                .AsSplitQuery()
                .ToListAsync();

            return new IndicadoresDatosDto
            {
                NombrePrograma = nombrePrograma,
                Egresados = egresados,
                UltimaActualizacion = DateTime.UtcNow
            };
        }
    }
}
