using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Situaciones_Laborales")]
    public class SituacionLaboral
    {
        [Key]
        public int IdSituacionLaboral { get;set;  }
        public int EgresadoId { get; set;  }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;
        [MaxLength(150)]
        public string? EstadoActual { get; set; }

        // Cambios relevantes
        public bool NuevoEmpleo { get; set; }
        public bool Promocion { get; set; }
        public bool CambioInstitucion { get; set; }
        public bool NuevasResponsabilidades { get; set;  }
        public bool IncorporacionPosgradoñ { get; set; }
        public bool EmprendimientoPropio { get; set;  } 


    }
}
