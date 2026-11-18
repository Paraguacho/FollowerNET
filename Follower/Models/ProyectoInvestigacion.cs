using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Proyectos_Investigacion")]
    public class ProyectoInvestigacion
    {
        [Key]
        public int IdProyecto { get; set; }
        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;

        [Required(ErrorMessage = "El nombre del proyecto es obligatorio.")]
        [MaxLength(250)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La institución es obligatoria.")]
        [MaxLength(200)]
        public string Institucion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El periodo es obligatorio.")]
        [MaxLength(100)]
        public string Periodo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La función en el proyecto es obligatoria.")]
        [MaxLength(100)]
        public string Funcion { get; set; } = string.Empty; //responsable

        [Required(ErrorMessage = "El estado del proyecto es obligatorio.")]
        [MaxLength(50)]
        public string Estado { get; set; } = string.Empty; //curso concluido
    }
}