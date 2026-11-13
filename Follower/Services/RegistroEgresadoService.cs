using Follower.DTOs;
using Follower.Models;
using Follower.Repositories;

namespace Follower.Services
{
    public sealed class RegistroEgresadoService : IRegistroEgresadoService
    {
        private readonly IRegistroEgresadoRepository _repository;

        // Función: prepara el servicio de registro.
        // Flujo: recibe por inyección el repositorio que consulta y persiste las cuentas nuevas.
        public RegistroEgresadoService(IRegistroEgresadoRepository repository)
        {
            _repository = repository;
        }

        // Función: obtiene los programas disponibles para el formulario.
        // Flujo: delega la consulta de programas al repositorio.
        public Task<List<ProgramaEducativo>> ObtenerProgramasDisponiblesAsync()
        {
            return _repository.ObtenerProgramasDisponiblesAsync();
        }

        // Función: registra la cuenta y el perfil inicial de un egresado.
        // Flujo: detecta correos duplicados, crea las entidades con estado inicial y solicita su persistencia transaccional.
        public async Task<RegistroResultadoDto> RegistrarAsync(RegistroEgresadoDto modelo)
        {
            if (await _repository.ExisteCorreoAsync(modelo.Correo))
            {
                return new RegistroResultadoDto { CorreoRegistrado = true };
            }

            var ahora = DateTime.UtcNow;
            var usuario = new Usuario
            {
                Correo = modelo.Correo,
                ContrasenaHash = modelo.Contrasena,
                IdRol = 2
            };
            var egresado = new Egresado
            {
                IdPrograma = modelo.IdPrograma,
                Nombres = modelo.Nombres,
                ApellidoPaterno = modelo.ApellidoPaterno,
                ApellidoMaterno = modelo.ApellidoMaterno,
                FechaEgreso = modelo.AnoEgreso,
                EstadoSolicitud = "Pendiente",
                PrivacidadPerfil = "Privado",
                FechaSolicitud = ahora,
                FechaAceptacionPrivacidad = ahora,
                FechaUltimaActualizacion = ahora
            };

            await _repository.RegistrarAsync(usuario, egresado);
            return new RegistroResultadoDto { Exitoso = true };
        }
    }
}
