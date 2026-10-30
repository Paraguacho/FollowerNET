using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table ("Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [MaxLength(265)]
        public string ContrasenaHash { get; set;  } = string.Empty;

        [Required]
        public int IdRol { get; set; }

        [ForeignKey(nameof(IdRol))]
        public Rol Rol { get; set; } = null!;

        public Egresado? Egresado { get; set; }

        //Programa que administra (rp)
        public ICollection<ProgramaResponsable> ProgramasAsignados { get; set; } = new List<ProgramaResponsable>();




    }
}
