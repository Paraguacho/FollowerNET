using Follower.Models;

namespace Follower.DTOs
{
    public class SolicitudesIngresoDto
    {
        public ProgramaEducativo? ProgramaActual { get; set; }
        public List<Egresado> Pendientes { get; set; } = new();
    }
}
