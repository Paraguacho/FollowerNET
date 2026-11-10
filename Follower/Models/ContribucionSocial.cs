using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Follower.Models
{
    [Table("Contribuciones_Sociales")]

    public class ContribucionSocial
    {
        [Key]
        public int IdContribucion { get; set; }
        public int EgresadoId { get; set;  }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; }

        [MaxLength(200)]
        public string NombreActividad { get; set;  }  = string.Empty;

        [MaxLength(150)]
        public string TipoContribucion { get; set; } = string.Empty;
        [MaxLength(200)]
        public string SectorAtendido { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        [MaxLength(1000)]
        public string Descripcion { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Resultados { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? ArchivoEvidencia { get; set; }
    }
}
