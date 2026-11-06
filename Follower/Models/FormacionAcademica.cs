using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Formaciones_Academicas")]
    public class FormacionAcademica
    {
        [Key]
        public int IdFormacionAcademica { get; set;  }
        public int EgresadoId { get; set;  }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;
        [MaxLength(200)]
        public string NombrePrograma { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Grado { get; set;  }
        [MaxLength(200)]
        public string? Institucion { get; set; }
        [MaxLength(100)]
        public string? Pais { get; set; }
        [MaxLength(100)]
        public string? EstadoPrograma { get; set;  }
        public DateTime? FechaInicio { get; set;  }
        public DateTime? FechaFin { get; set;  }
        public bool RecibioBeca { get; set; }
        [MaxLength(150)]
        public string? TipoApoyo { get; set; }
        [MaxLength(200)]
        public string? InstitucionOtorgante { get; set; }
        public int? AnoOtorgamiento { get; set;  }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MontoAsignado { get; set; }


    }
}
