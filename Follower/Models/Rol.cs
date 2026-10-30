using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Follower.Models
{
    [Table ("Roles")]
    public class Rol
    {
        [Key]
        public int IdRol {  get; set; }
        [Required] [StringLength (50)]
        public string NombreRol { get; set; } = string.Empty;

        // Relacion con usuarios, un rol tiene muchos usuarios
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();


    }
}
