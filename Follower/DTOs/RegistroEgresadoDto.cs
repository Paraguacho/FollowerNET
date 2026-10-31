using System.ComponentModel.DataAnnotations;

namespace Follower.DTOs
{
    public class RegistroEgresadoDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre(s)")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Required(ErrorMessage ="El apellido materno es obligatorio")]
        [Display(Name ="Apellido Materno")]
        public string ApellidoMaterno { get; set;  } = string.Empty;

        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress(ErrorMessage ="El formato de correo no es válido")]
        [Display(Name ="Correo electrónico")]
        public string Correo { get; set; }= string.Empty;

        [Required(ErrorMessage ="La contraseña es obligatoria")]
        [StringLength(100, MinimumLength =8, ErrorMessage ="La contraseña debe tener al menos 8 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name ="Contraseña")]
        public string Contrasena { get; set;  } = string.Empty;

        [Required(ErrorMessage ="Selecciona un programa educativo")]
        [Display(Name ="Programa de egreso")]
        public int IdPrograma { get; set; }

        [Display(Name = "Año de egreso")]
        public int? AnoEgreso { get; set; }

        [Range(typeof(bool),"true" ,"true", ErrorMessage ="Debes aceptar el Aviso de Privacidad para continuar.")]
        public bool AceptarPrivacidad { get; set; }


    }
}
