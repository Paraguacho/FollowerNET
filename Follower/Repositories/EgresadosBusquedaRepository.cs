using Follower.Data;
using Follower.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Follower.Repositories
{
    public sealed class EgresadosBusquedaRepository : IEgresadosBusquedaRepository
    {
        private readonly ApplicationDbContext _context;

        // HU3.2 / HU3.3
        // Flujo: recibe el contexto EF que proporciona la conexión MySQL configurada.
        // Función: prepara la ejecución de consultas de búsqueda y carga de perfiles.
        public EgresadosBusquedaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // HU3.2 / HU3.3
        // Flujo: crea un comando parametrizado, lo ejecuta y convierte el valor escalar obtenido a entero largo.
        // Función: devuelve el conteo total de resultados que coincide con los filtros.
        public Task<long> EjecutarEscalarAsync(
            string sql,
            IReadOnlyDictionary<string, object?> parametros,
            CancellationToken cancellationToken = default)
        {
            return EjecutarConConexionAsync(async connection =>
            {
                await using var command = CrearComando(connection, sql, parametros);
                var resultado = await command.ExecuteScalarAsync(cancellationToken);
                return resultado is null or DBNull ? 0 : Convert.ToInt64(resultado, CultureInfo.InvariantCulture);
            }, cancellationToken);
        }

        // HU3.3
        // Flujo: consulta el egresado aprobado del programa indicado e incluye sus secciones relacionadas sin seguimiento de cambios.
        // Función: carga datos para la vista de perfil de solo lectura y respeta el alcance autorizado.
        public Task<Egresado?> ObtenerPerfilAutorizadoAsync(
            int idEgresado,
            int idPrograma,
            CancellationToken cancellationToken = default)
        {
            return _context.Egresados
                .AsNoTracking()
                .AsSplitQuery()
                .Where(e => e.IdEgresado == idEgresado
                    && e.IdPrograma == idPrograma
                    && e.EstadoSolicitud == "Aprobado")
                .Include(e => e.PerfilAcademico)
                .Include(e => e.SituacionLaboral)
                .Include(e => e.ExperienciasLaborales)
                .Include(e => e.FormacionesAcademicas)
                .Include(e => e.ResumenProductividad)
                .Include(e => e.ProductividadesCientificas)
                    .ThenInclude(p => p.Coautores)
                .Include(e => e.ProyectosInvestigacion)
                .Include(e => e.ReconocimientosAcademicos)
                .Include(e => e.ContribucionesSociales)
                .Include(e => e.ValoresCamposPersonalizados)
                    .ThenInclude(v => v.CampoPersonalizado)
                    .ThenInclude(c => c.SeccionPersonalizada)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // HU3.2 / HU3.3
        // Flujo: ejecuta el comando, recorre el lector y aplica la función de mapeo a cada fila.
        // Función: retorna filas de búsqueda u opciones tipadas al servicio.
        public Task<IReadOnlyList<T>> EjecutarConsultaAsync<T>(
            string sql,
            IReadOnlyDictionary<string, object?> parametros,
            Func<DbDataReader, T> mapear,
            CancellationToken cancellationToken = default)
        {
            return EjecutarConConexionAsync<IReadOnlyList<T>>(async connection =>
            {
                await using var command = CrearComando(connection, sql, parametros);
                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                var resultados = new List<T>();
                while (await reader.ReadAsync(cancellationToken))
                {
                    resultados.Add(mapear(reader));
                }

                return resultados;
            }, cancellationToken);
        }

        // HU3.2 / HU3.3
        // Flujo: obtiene la conexión de EF, la abre si estaba cerrada y la cierra al finalizar la ejecución.
        // Función: administra la conexión usada por los comandos nativos del repositorio.
        private async Task<T> EjecutarConConexionAsync<T>(
            Func<DbConnection, Task<T>> ejecutar,
            CancellationToken cancellationToken)
        {
            var connection = _context.Database.GetDbConnection();
            var abrirConexion = connection.State != ConnectionState.Open;
            if (abrirConexion)
            {
                await connection.OpenAsync(cancellationToken);
            }

            try
            {
                return await ejecutar(connection);
            }
            finally
            {
                if (abrirConexion)
                {
                    await connection.CloseAsync();
                }
            }
        }

        // HU3.2 / HU3.3
        // Flujo: asigna el SQL y crea un parámetro de base de datos por cada valor recibido.
        // Función: evita insertar valores de filtros directamente en el comando SQL.
        private static DbCommand CrearComando(
            DbConnection connection,
            string sql,
            IReadOnlyDictionary<string, object?> parametros)
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            foreach (var (nombre, valor) in parametros)
            {
                var parametro = command.CreateParameter();
                parametro.ParameterName = nombre.StartsWith('@') ? nombre : $"@{nombre}";
                parametro.Value = valor ?? DBNull.Value;
                command.Parameters.Add(parametro);
            }

            return command;
        }
    }
}
