using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Formaciones_Academicas")]
    public class FormacionAcademica
    {
        [Key]
        public int IdFormacionAcademica { get; set; }
        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;

        [Required(ErrorMessage = "El nombre del programa es obligatorio.")]
        [MaxLength(200)]
        public string NombrePrograma { get; set; } = string.Empty;

        [Required(ErrorMessage = "El grado académico es obligatorio.")]
        [MaxLength(100)]
        public string? Grado { get; set; }

        [Required(ErrorMessage = "La institución es obligatoria.")]
        [MaxLength(200)]
        public string? Institucion { get; set; }

        [Required(ErrorMessage = "El país es obligatorio.")]
        [MaxLength(100)]
        public string? Pais { get; set; }

        [Required(ErrorMessage = "El estado del programa es obligatorio.")]
        [MaxLength(100)]
        public string? EstadoPrograma { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool EsFormacionActual { get; set; }
        public bool RecibioBeca { get; set; }

        [MaxLength(150)]
        public string? TipoApoyo { get; set; }

        [MaxLength(200)]
        public string? InstitucionOtorgante { get; set; }

        [Range(1950, 2100, ErrorMessage = "Ingresa un año de otorgamiento válido.")]
        public int? AnoOtorgamiento { get; set; }

        [Range(0, 999999999.99, ErrorMessage = "Ingresa un monto válido.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MontoAsignado { get; set; }
    }
}