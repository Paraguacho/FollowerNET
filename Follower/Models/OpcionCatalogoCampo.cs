using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Opciones_Catalogo_Campo")]
    public class OpcionCatalogoCampo
    {
        [Key]
        public int IdOpcion { get; set;  }

        [Required]
        public int IdCampo {  get; set; }
        [ForeignKey(nameof(IdCampo))]
        public CampoPersonalizado CampoPersonalizado { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string ValorOpcion { get; set; } = string.Empty;

        public int Orden { get; set; } = 0;
    }
}