using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Follower.Models
{
    [Table("Productividades_Cientificas")]

    public class ProductividadCientifica
    {
        [Key]
        public int IdProductividad { get; set; }
        public int EgresadoId { get; set; }
        [ForeignKey("EgresadoId")]
        public Egresado Egresado { get; set; } = null!;

        [MaxLength(100)]
        public string TipoProducto { get; set; } = string.Empty;
        [MaxLength(250)]
        public string Titulo { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? RevistaOLibro{ get; set; }
        [MaxLength(150)]
        public string? Editorial { get; set; }
        public int? AnoPublicacion { get; set;  }
        [MaxLength(250)]
        public string? DoiUrl { get; set; }
        [MaxLength(50)]
        public string? Isbn { get; set;  }
        [MaxLength(100)]
        public string? Participacion { get; set;  }
        [MaxLength(255)]
        public string? ArchivoPdf { get; set; }

        //MIED
        [MaxLength(200)]
        public string? NombreProfesorMied { get; set; }

        public ICollection<CoautorProducto> Coautores { get; set; } = new List<CoautorProducto>();

    }
}
