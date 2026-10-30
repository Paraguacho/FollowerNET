using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Campos_Personalizados")]
    public class CampoPersonalizado
    {
        [Key]
        public int IdCampo { get; set;  }

        [Required]

        public int IdSeccion { get; set;  }

        [ForeignKey(nameof(IdSeccion))]
        public SeccionPersonalizada SeccionPersonalizada { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string NombreEtiqueta { get; set; } = string.Empty;

        public enum TipoDatoCampo
        {
            TextoCorto = 1,
            TextoArea = 2,
            ListaDesplegable = 3,
            Casilla = 4,
            Fecha = 5,
            EscalaValoracion = 6
        
        }

        [Required]
        [MaxLength(50)]
        public TipoDatoCampo TipoDato { get; set; } = TipoDatoCampo.TextoCorto;

        public bool EsObligatorio { get; set; } = false;
        public int Orden { get; set; } = 0;

        public ICollection<OpcionCatalogoCampo> Opciones { get; set; } = new List<OpcionCatalogoCampo>();
        public ICollection<ValorCampoEgresado> ValoresEgresados { get; set; } = new List<ValorCampoEgresado>();

    }
}