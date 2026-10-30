using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Secciones_Personalizadas")]
    public class SeccionPersonalizada
    {
        [Key]
        public int IdSeccion { get; set; }

        [Required]
        public int IdPrograma { get; set;  }

        [ForeignKey(nameof(IdPrograma))]
        public ProgramaEducativo ProgramaEducativo { get; set; } = null!;

        [Required]
        [MaxLength(160)]
        public string TituloSeccion { get; set; } = string.Empty;

        public int OrdenAparicion { get; set; } = 0;

        public ICollection<CampoPersonalizado> Campos { get; set; } = new List<CampoPersonalizado>();

    }
}
