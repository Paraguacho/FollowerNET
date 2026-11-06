using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Experiencias_Laborales")]
    public class ExperienciaLaboral
    {
        [Key]
        public int IdExperienciaLaboral { get; set; }

        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;
        [MaxLength(200)]
        public string Institucion { get; set; } = string.Empty;
        [MaxLength(150)]
        public string Puesto { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Sector { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool EsEmpleoActual { get; set; }
        [MaxLength(1000)]
        public string? ActividadesPrincipales { get; set; }
        // MIED


        [MaxLength(150)]
        public string? NivelEducativo { get; set; }
        [MaxLength(100)]
        public string? RelacionConFormacion { get; set; }
        
    }
}
