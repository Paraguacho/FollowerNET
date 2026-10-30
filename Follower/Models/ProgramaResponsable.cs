using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Programas_Responsables")]
    public class ProgramaResponsable
    {
        [Key]
        public int IdProgramaResponsable { get; set; }

        [Required]
        public int IdPrograma { get; set; }

        [ForeignKey(nameof(IdPrograma))]
        public ProgramaEducativo ProgramaEducativo { get; set; } = null!;

        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
    }
}
