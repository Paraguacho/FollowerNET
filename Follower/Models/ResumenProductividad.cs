using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Resumenes_Productividad")]

    public class ResumenProductividad
    {
        [Key]
        public int IdResumen { get; set;  }
        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;
        public bool TienePremios { get; set; }
        public bool TieneCertificaciones { get; set; }
        public bool TieneMembresias { get; set; }
        public bool TieneContribucionSocial { get; set; }
    }
}
