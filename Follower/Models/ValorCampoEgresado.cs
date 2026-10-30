using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Valores_Campo_Egresados")]          
    public class ValorCampoEgresado
    {
        [Key]
        public int IdValor { get; set; }
        [Required]
        public int IdEgresado { get; set;  }

        [ForeignKey(nameof(IdEgresado))]
        public Egresado Egresado { get; set; } = null!;

        public int IdCampo { get; set; }
        [Required]
        [ForeignKey(nameof(IdCampo))]
        public CampoPersonalizado CampoPersonalizado { get; set; } = null!;
        public string? ValorCapturado { get; set; }
    }
}