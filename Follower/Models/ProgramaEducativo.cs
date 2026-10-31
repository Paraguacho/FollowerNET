using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table ("Programas_Educativos")]
    public class ProgramaEducativo
    {
        [Key]
        public int IdPrograma { get; set; }

        [Required]
        [MaxLength(20)]
        public string Siglas { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string NombrePrograma { get; set; }  = string.Empty;

       
        //1:N el programa tiene muchos egresados
        public ICollection<ProgramaResponsable> Responsables { get; set; } = new List<ProgramaResponsable>();
        public ICollection<SeccionPersonalizada> SeccionPersonalizadas { get; set; } = new List<SeccionPersonalizada>();
        



    }
}