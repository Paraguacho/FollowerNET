using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Reconocomientos_Academicos")]
    public class ReconocimientoAcademico
    {
        [Key]
        public int IdReconomiento { get; set; }
        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;

        [MaxLength(100)]
        public string TipoRegistro { get; set; } = string.Empty; // snii prodep
        [MaxLength(100)]
        public string? Nivel { get; set; } // candidato nivel 1 etc

        public int? AnoInicio { get; set; }
        public int AnoFin { get; set; }
        [MaxLength(250)]
        public string? NombreReconocimiento { get; set; }

        [MaxLength(200)]
        public string? InstitucionOtorgante { get; set;  }

        [MaxLength(250)]
        public string? MotivoOCategoria { get; set; }
        [MaxLength(150)]
        public string? TipoParticipacion { get; set; }
        [MaxLength(50)]
        public string? EstadoActual { get; set; }
        [MaxLength(255)]
        public string? ArchivoEvidencia { get; set; }

    }
}
