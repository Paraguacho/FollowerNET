using Follower.DTOs;
using Follower.Models;
using Follower.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data.Common;
using System.Security.Claims;
using System.Text;

namespace Follower.Services
{
    public sealed class EgresadosBusquedaService : IEgresadosBusquedaService
    {
        private const string BaseFrom = """
            FROM `egresados` e
            LEFT JOIN `situaciones_laborales` sl ON sl.`EgresadoId` = e.`IdEgresado`
            """;

        private static readonly string[] SectoresDisponibles = { "Público", "Privado", "Educativo" };
        private static readonly string[] NivelesDisponibles = { "Maestría", "Doctorado", "Diplomado", "Especialidad" };
        private static readonly string[] SituacionesLaboralesDisponibles =
        {
            "Empleo de tiempo completo",
            "Empleo de tiempo medio",
            "Trabajador independiente / Freelance",
            "Buscando empleo",
            "Estudiando exclusivamente"
        };

        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IEgresadosBusquedaRepository _repository;

        public EgresadosBusquedaService(
            AuthenticationStateProvider authStateProvider,
            IEgresadosBusquedaRepository repository)
        {
            _authStateProvider = authStateProvider;
            _repository = repository;
        }

        public async Task<EgresadosBusquedaOpcionesDto> ObtenerOpcionesAsync(CancellationToken cancellationToken = default)
        {
            var programa = await ObtenerProgramaAutorizadoAsync(cancellationToken);
            const string sql = """
                SELECT DISTINCT sl.`EstadoActual`
                FROM `egresados` e
                INNER JOIN `situaciones_laborales` sl ON sl.`EgresadoId` = e.`IdEgresado`
                WHERE e.`IdPrograma` = @IdPrograma
                  AND e.`EstadoSolicitud` = 'Aprobado'
                  AND sl.`EstadoActual` IS NOT NULL
                  AND TRIM(sl.`EstadoActual`) <> ''
                ORDER BY sl.`EstadoActual`
                """;
            var situacionesRegistradas = await _repository.EjecutarConsultaAsync(
                sql,
                new Dictionary<string, object?> { ["IdPrograma"] = programa.IdPrograma },
                reader => reader.GetString(0),
                cancellationToken);
            var situacionesLaborales = SituacionesLaboralesDisponibles
                .Concat(situacionesRegistradas)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return new EgresadosBusquedaOpcionesDto
            {
                SiglasPrograma = programa.Siglas,
                SituacionesLaborales = situacionesLaborales,
                Sectores = SectoresDisponibles,
                NivelesEstudio = NivelesDisponibles
            };
        }

        public async Task<Egresado?> ObtenerPerfilAutorizadoAsync(
            int idEgresado,
            CancellationToken cancellationToken = default)
        {
            if (idEgresado <= 0)
            {
                return null;
            }

            var programa = await ObtenerProgramaAutorizadoAsync(cancellationToken);
            return await _repository.ObtenerPerfilAutorizadoAsync(idEgresado, programa.IdPrograma, cancellationToken);
        }

        public async Task<EgresadosBusquedaPaginaDto> BuscarAsync(
            EgresadosBusquedaFiltrosDto filtros,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filtros);
            var programa = await ObtenerProgramaAutorizadoAsync(cancellationToken);
            var filtrosNormalizados = NormalizarFiltros(filtros);
            var (whereSql, parametros) = CrearCondiciones(filtrosNormalizados, programa.IdPrograma);

            var total = await _repository.EjecutarEscalarAsync(
                $"SELECT COUNT(*) {whereSql}",
                parametros,
                cancellationToken);

            var tamanoPagina = filtrosNormalizados.RegistrosPorPagina;
            var totalPaginas = total == 0 ? 1 : (total - 1) / tamanoPagina + 1;
            var pagina = (int)Math.Min(filtrosNormalizados.Pagina, Math.Min(totalPaginas, int.MaxValue));
            var offset = (long)(pagina - 1) * tamanoPagina;
            var ordenSql = ObtenerOrdenSql(filtrosNormalizados.OrdenarPor);
            var direccionSql = filtrosNormalizados.OrdenAscendente ? "ASC" : "DESC";
            parametros["Limit"] = tamanoPagina;
            parametros["Offset"] = offset;

            var sql = $"""
                SELECT e.`IdEgresado`, e.`Nombres`, e.`ApellidoPaterno`, e.`ApellidoMaterno`,
                       e.`FechaEgreso`, sl.`EstadoActual`, e.`FechaUltimaActualizacion`
                {whereSql}
                ORDER BY {ordenSql} {direccionSql}, e.`IdEgresado` ASC
                LIMIT @Limit OFFSET @Offset
                """;

            var resultados = await _repository.EjecutarConsultaAsync(
                sql,
                parametros,
                MapearFila,
                cancellationToken);

            return new EgresadosBusquedaPaginaDto
            {
                Resultados = resultados,
                TotalResultados = total,
                Pagina = pagina,
                RegistrosPorPagina = tamanoPagina
            };
        }

        private async Task<(int IdPrograma, string Siglas)> ObtenerProgramaAutorizadoAsync(CancellationToken cancellationToken)
        {
            var authenticationState = await _authStateProvider.GetAuthenticationStateAsync();
            var usuario = authenticationState.User;
            if (usuario.Identity?.IsAuthenticated != true || !usuario.IsInRole("ResponsablePrograma"))
            {
                throw new UnauthorizedAccessException("La búsqueda está disponible únicamente para responsables de programa autenticados.");
            }

            var claimUsuarioId = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(claimUsuarioId, out var usuarioId))
            {
                throw new UnauthorizedAccessException("No se pudo identificar al responsable de programa.");
            }

            const string sql = """
                SELECT pr.`IdPrograma`, p.`Siglas`
                FROM `programas_responsables` pr
                INNER JOIN `programas_educativos` p ON p.`IdPrograma` = pr.`IdPrograma`
                WHERE pr.`IdUsuario` = @IdUsuario
                ORDER BY pr.`IdPrograma`
                LIMIT 2
                """;
            var programas = await _repository.EjecutarConsultaAsync(
                sql,
                new Dictionary<string, object?> { ["IdUsuario"] = usuarioId },
                reader => (IdPrograma: reader.GetInt32(0), Siglas: reader.GetString(1)),
                cancellationToken);

            return programas.Count switch
            {
                0 => throw new UnauthorizedAccessException("No tienes un programa educativo autorizado."),
                1 => programas[0],
                _ => throw new InvalidOperationException("La búsqueda requiere que el responsable tenga un solo programa asignado.")
            };
        }

        private static EgresadosBusquedaFiltrosDto NormalizarFiltros(EgresadosBusquedaFiltrosDto filtros)
        {
            var resultado = new EgresadosBusquedaFiltrosDto
            {
                TerminoBusqueda = NormalizarTexto(filtros.TerminoBusqueda, 150),
                AnioEgresoDesde = filtros.AnioEgresoDesde,
                AnioEgresoHasta = filtros.AnioEgresoHasta,
                SituacionLaboral = NormalizarTexto(filtros.SituacionLaboral, 150),
                Sector = NormalizarTexto(filtros.Sector, 100),
                NivelEstudioActual = NormalizarTexto(filtros.NivelEstudioActual, 150),
                InstitucionActual = NormalizarTexto(filtros.InstitucionActual, 200),
                Ciudad = NormalizarTexto(filtros.Ciudad, 100),
                Estado = NormalizarTexto(filtros.Estado, 100),
                Pais = NormalizarTexto(filtros.Pais, 100),
                SoloDesactualizados = filtros.SoloDesactualizados,
                Pagina = Math.Max(1, filtros.Pagina),
                RegistrosPorPagina = filtros.RegistrosPorPagina is 10 or 20 or 50 ? filtros.RegistrosPorPagina : 10,
                OrdenarPor = filtros.OrdenarPor,
                OrdenAscendente = filtros.OrdenAscendente
            };

            if (resultado.AnioEgresoDesde.HasValue && resultado.AnioEgresoHasta.HasValue
                && resultado.AnioEgresoDesde > resultado.AnioEgresoHasta)
            {
                throw new ArgumentException("El año inicial no puede ser mayor que el año final.", nameof(filtros));
            }

            if (resultado.Sector is not null && !SectoresDisponibles.Contains(resultado.Sector, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El sector seleccionado no es válido.", nameof(filtros));
            }

            if (resultado.NivelEstudioActual is not null
                && !NivelesDisponibles.Contains(resultado.NivelEstudioActual, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El nivel de estudios seleccionado no es válido.", nameof(filtros));
            }

            return resultado;
        }

        private static (string Sql, Dictionary<string, object?> Parametros) CrearCondiciones(
            EgresadosBusquedaFiltrosDto filtros,
            int idPrograma)
        {
            var condiciones = new List<string>
            {
                "e.`IdPrograma` = @IdPrograma",
                "e.`EstadoSolicitud` = 'Aprobado'"
            };
            var parametros = new Dictionary<string, object?> { ["IdPrograma"] = idPrograma };

            if (filtros.TerminoBusqueda is not null)
            {
                condiciones.Add("CONCAT_WS(' ', e.`Nombres`, e.`ApellidoPaterno`, e.`ApellidoMaterno`) LIKE @TerminoBusqueda ESCAPE '!'");
                parametros["TerminoBusqueda"] = CrearPatronLike(filtros.TerminoBusqueda);
            }

            if (filtros.AnioEgresoDesde.HasValue)
            {
                condiciones.Add("e.`FechaEgreso` >= @AnioEgresoDesde");
                parametros["AnioEgresoDesde"] = filtros.AnioEgresoDesde.Value;
            }

            if (filtros.AnioEgresoHasta.HasValue)
            {
                condiciones.Add("e.`FechaEgreso` <= @AnioEgresoHasta");
                parametros["AnioEgresoHasta"] = filtros.AnioEgresoHasta.Value;
            }

            if (filtros.SituacionLaboral is not null)
            {
                condiciones.Add("sl.`EstadoActual` = @SituacionLaboral");
                parametros["SituacionLaboral"] = filtros.SituacionLaboral;
            }

            if (filtros.Sector is not null)
            {
                condiciones.Add("EXISTS (SELECT 1 FROM `experiencias_laborales` el WHERE el.`EgresadoId` = e.`IdEgresado` AND el.`EsEmpleoActual` = 1 AND el.`Sector` = @Sector)");
                parametros["Sector"] = filtros.Sector;
            }

            if (filtros.NivelEstudioActual is not null)
            {
                condiciones.Add("EXISTS (SELECT 1 FROM `formaciones_academicas` fa WHERE fa.`EgresadoId` = e.`IdEgresado` AND fa.`EsFormacionActual` = 1 AND fa.`Grado` = @NivelEstudioActual)");
                parametros["NivelEstudioActual"] = filtros.NivelEstudioActual;
            }

            if (filtros.InstitucionActual is not null)
            {
                condiciones.Add("EXISTS (SELECT 1 FROM `experiencias_laborales` el WHERE el.`EgresadoId` = e.`IdEgresado` AND el.`EsEmpleoActual` = 1 AND el.`Institucion` LIKE @InstitucionActual ESCAPE '!')");
                parametros["InstitucionActual"] = CrearPatronLike(filtros.InstitucionActual);
            }

            AgregarFiltroRegexp(condiciones, parametros, "e.`Ciudad`", "Ciudad", filtros.Ciudad);
            AgregarFiltroRegexp(condiciones, parametros, "e.`Estado`", "Estado", filtros.Estado);
            AgregarFiltroRegexp(condiciones, parametros, "e.`Pais`", "Pais", filtros.Pais);

            if (filtros.SoloDesactualizados)
            {
                condiciones.Add("e.`FechaUltimaActualizacion` < @FechaLimiteDesactualizado");
                parametros["FechaLimiteDesactualizado"] = DateTime.UtcNow.AddMonths(-12);
            }

            return ($"{BaseFrom} WHERE {string.Join(" AND ", condiciones)}", parametros);
        }

        private static void AgregarFiltroRegexp(
            ICollection<string> condiciones,
            IDictionary<string, object?> parametros,
            string columna,
            string nombreParametro,
            string? valor)
        {
            if (valor is null)
            {
                return;
            }

            condiciones.Add($"{columna} REGEXP @{nombreParametro}");
            parametros[nombreParametro] = CrearPatronRegexpLiteral(valor);
        }

        private static string ObtenerOrdenSql(EgresadosOrdenCampo ordenarPor) => ordenarPor switch
        {
            EgresadosOrdenCampo.Nombre => "CONCAT_WS(' ', e.`Nombres`, e.`ApellidoPaterno`, e.`ApellidoMaterno`)",
            EgresadosOrdenCampo.FechaEgreso => "e.`FechaEgreso`",
            EgresadosOrdenCampo.SituacionLaboral => "sl.`EstadoActual`",
            EgresadosOrdenCampo.FechaUltimaActualizacion => "e.`FechaUltimaActualizacion`",
            _ => throw new ArgumentOutOfRangeException(nameof(ordenarPor))
        };

        private static EgresadosBusquedaFilaDto MapearFila(DbDataReader reader) => new()
        {
            IdEgresado = reader.GetInt32(0),
            Nombres = reader.GetString(1),
            ApellidoPaterno = reader.GetString(2),
            ApellidoMaterno = reader.GetString(3),
            FechaEgreso = reader.IsDBNull(4) ? null : reader.GetInt32(4),
            SituacionLaboral = reader.IsDBNull(5) ? null : reader.GetString(5),
            FechaUltimaActualizacion = reader.GetDateTime(6)
        };

        private static string? NormalizarTexto(string? valor, int longitudMaxima)
        {
            var texto = valor?.Trim();
            if (string.IsNullOrEmpty(texto))
            {
                return null;
            }

            if (texto.Length > longitudMaxima)
            {
                throw new ArgumentException($"El valor no puede exceder {longitudMaxima} caracteres.");
            }

            return texto;
        }

        private static string CrearPatronLike(string texto)
        {
            var literal = texto.Replace("!", "!!", StringComparison.Ordinal)
                .Replace("%", "!%", StringComparison.Ordinal)
                .Replace("_", "!_", StringComparison.Ordinal);
            return $"%{literal}%";
        }

        private static string CrearPatronRegexpLiteral(string texto)
        {
            var resultado = new StringBuilder(".*");
            foreach (var caracter in texto)
            {
                if ("\\.^$|?*+()[]{}".Contains(caracter))
                {
                    resultado.Append('\\');
                }

                resultado.Append(caracter);
            }

            return resultado.Append(".*").ToString();
        }
    }
}
