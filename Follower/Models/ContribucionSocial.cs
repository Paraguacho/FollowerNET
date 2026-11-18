using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Contribuciones_Sociales")]
    public class ContribucionSocial
    {
        [Key]
        public int IdContribucion { get; set; }
        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; }

        [Required(ErrorMessage = "El nombre de la actividad es obligatorio.")]
        [MaxLength(200)]
        public string NombreActividad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de contribución es obligatorio.")]
        [MaxLength(150)]
        public string TipoContribucion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El sector atendido es obligatorio.")]
        [MaxLength(200)]
        public string SectorAtendido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(1000)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los resultados son obligatorios.")]
        [MaxLength(1000)]
        public string Resultados { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ArchivoEvidencia { get; set; }
    }
}