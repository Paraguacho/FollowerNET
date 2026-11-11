using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Coautores_Productos")]

    public class CoautorProducto
    {
        [Key]
        public int IdCoautor { get; set; }

        public int ProductividadId { get; set; }
        [ForeignKey("ProductividadId")]
        public ProductividadCientifica Productividad { get; set; } = null!;
        [MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;
        [MaxLength(100)]
        public string ApellidoPaterno { get; set; } = string.Empty;
        [MaxLength(100) ]
        public string? ApellidoMaterno { get; set; }
        [MaxLength(100)]
        public string Rol { get; set; } = string.Empty;
    }
}
