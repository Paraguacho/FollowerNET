using Follower.Models;

namespace Follower.Repositories
{
    public interface IEgresadoRepository
    {
        // Función: define la carga del perfil asociado a un usuario.
        // Flujo: recibe el identificador de usuario y devuelve el agregado con sus relaciones o null.
        Task<Egresado?> ObtenerPorUsuarioIdAsync(int idUsuario);

        // Función: define la persistencia del agregado del egresado.
        // Flujo: recibe el egresado y guarda sus secciones relacionadas y cambios.
        Task ActualizarAsync(Egresado egresado);

        // Función: define la eliminación de una experiencia laboral.
        // Flujo: recibe la entidad, actualiza la fecha del perfil relacionado y persiste la eliminación.
        Task EliminarExperienciaLaboralAsync(ExperienciaLaboral experiencia);

        // Función: define la eliminación de una formación académica.
        // Flujo: recibe la entidad, actualiza la fecha del perfil relacionado y persiste la eliminación.
        Task EliminarFormacionAcademicaAsync(FormacionAcademica formacion);

        // Función: define la eliminación de un producto científico.
        // Flujo: recibe la entidad, la elimina y guarda los cambios.
        Task EliminarProductividadCientificaAsync(ProductividadCientifica productividad);

        // Función: define la eliminación de un coautor.
        // Flujo: recibe la entidad, la elimina y guarda los cambios.
        Task EliminarCoautorAsync(CoautorProducto coautor);

        // Función: define la eliminación de un proyecto de investigación.
        // Flujo: recibe la entidad, la elimina y guarda los cambios.
        Task EliminarProyectoInvestigacionAsync(ProyectoInvestigacion proyecto);

        // Función: define la eliminación de un reconocimiento académico.
        // Flujo: recibe la entidad, la elimina y guarda los cambios.
        Task EliminarReconocimientoAcademicoAsync(ReconocimientoAcademico reconocimiento);

        // Función: define la eliminación de una contribución social.
        // Flujo: recibe la entidad, la elimina y guarda los cambios.
        Task EliminarContribucionSocialAsync(ContribucionSocial contribucion);
    }
}
