namespace Follower.DTOs
{
    public class AutenticacionResultadoDto
    {
        public bool Exito { get; set; }
        public int IdUsuario { get; set; }
        public string Rol { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? MensajeError { get; set; }
    }
}