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

        [Required(ErrorMessage = "La institución o empresa es obligatoria.")]
        [MaxLength(200)]
        public string Institucion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El puesto es obligatorio.")]
        [MaxLength(150)]
        public string Puesto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El sector es obligatorio.")]
        [MaxLength(100)]
        public string? Sector { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }
        public bool EsEmpleoActual { get; set; }

        [Required(ErrorMessage = "Las actividades principales son obligatorias.")]
        [MaxLength(1000)]
        public string? ActividadesPrincipales { get; set; }

        // MIED
        [MaxLength(150)]
        public string? NivelEducativo { get; set; }

        [MaxLength(100)]
        public string? RelacionConFormacion { get; set; }
    }
}