using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Proyectos_Investigacion")]
    public class ProyectoInvestigacion
    {
        [Key]
        public int IdProyecto { get; set; } 
        public int EgresadoId { get; set;  }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;

        [MaxLength(250)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Institucion { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Periodo { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Funcion { get; set; } = string.Empty; //responsable
        [MaxLength(50)]
        public string Estado { get; set; } = string.Empty; //curso concluido
    }
}
