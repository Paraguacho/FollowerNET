using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Perfiles_Academicos")]
    public class PerfilAcademico
    {
        [Key]
        public int Id { get; set; } 
        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;
        [MaxLength(255)]
        public string? Orcid { get; set;  }
        [MaxLength(255)]
        public string? GoogleScholar{ get; set; }
        [MaxLength(255)]
        public string? ResearchGate{ get; set; }
        [MaxLength(255)]
        public string? AcademiaEdu { get; set; }
        [MaxLength(255)]
        public string? ScopusId { get; set; }
        [MaxLength(255)]
        public string? LinkedIn { get; set; }


    }
}
