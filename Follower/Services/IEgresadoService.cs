using Follower.Models;

namespace Follower.Services
{
    public interface IEgresadoService
    {
        Task<Egresado?> ObtenerPorUsuarioIdAsync(int idUsuario);
        Task ActualizarAsync(Egresado egresado);
        Task EliminarExperienciaLaboralAsync(ExperienciaLaboral experiencia);
        Task EliminarFormacionAcademicaAsync(FormacionAcademica formacion);

        Task EliminarProductividadCientificaAsync(ProductividadCientifica productividad);
        Task EliminarCoautorAsync (CoautorProducto coautor);
        Task EliminarProyectoInvestigacionAsync(ProyectoInvestigacion proyecto);
        Task EliminarReconocimientoAcademicoAsync (ReconocimientoAcademico reconocimiento);
        Task EliminarContribucionSocialAsync(ContribucionSocial contribucion);

    }
}
