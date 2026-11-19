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

        [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
        [MaxLength(100)]
        public string TipoProducto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(250)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la revista o libro es obligatorio.")]
        [MaxLength(200)]
        public string? RevistaOLibro { get; set; }

        [MaxLength(150)]
        public string? Editorial { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        [Range(1950, 2100, ErrorMessage = "Ingresa un año de publicación válido.")]
        public int? AnoPublicacion { get; set; }

        [MaxLength(250)]
        public string? DoiUrl { get; set; }

        [MaxLength(50)]
        public string? Isbn { get; set; }

        [MaxLength(100)]
        public string? Participacion { get; set; }

        [MaxLength(255)]
        public string? ArchivoPdf { get; set; }

        //MIED
        public bool ParticipoProfesorMied { get; set; }
        [MaxLength(200)]
        public string? NombreProfesorMied { get; set; }

        public ICollection<CoautorProducto> Coautores { get; set; } = new List<CoautorProducto>();
    }
}