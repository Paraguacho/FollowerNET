using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table("Egresados")]
    public class Egresado
    {
        [Key]
        public int IdEgresados { get; set; }
        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;

        [Required]
        public int IdPrograma { get; set; }

        [ForeignKey(nameof(IdPrograma))]
        public ProgramaEducativo ProgramaEducativo { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string EstadoSolicitud { get; set; } = "Pendiente";  // Pendiente, Aprobado, Denegado

        [Required]
        [MaxLength(20)]
        public string PrivacidadPerfil { get; set; } = "Privado";

        [Required]
        [MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; } 
        
        public DateTime? FechaNacimiento { get; set; }

        
        public DateTime? FechaEgreso { get; set; }
        
        [MaxLength(100)]
        public string? Ciudad { get; set; }

        [MaxLength(100)]
        public string? Estado { get; set; }

        [MaxLength(100)]
        public string? Pais { get; set; }

        [MaxLength(255)]
        public string? FotoPerfilUrl { get; set; }

        public bool AutorizaDirectorio { get; set; } = false;

        public DateTime? FechaSolicitud { get; set; } = DateTime.UtcNow;

        public DateTime FechaAceptacionPrivacidad { get; set; }
        public DateTime? FechaResolucion { get; set; }

        public int? IdUsuarioResolvio { get; set; }

        [ForeignKey(nameof(IdUsuarioResolvio))]
        public Usuario? UsuarioResolvio { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.UtcNow;

        public ICollection<ValorCampoEgresado> ValoresCamposPersonalizados { get; set; } = new List<ValorCampoEgresado>();
















    }
}