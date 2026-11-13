using Follower.DTOs;
using Follower.Models;
using Follower.Repositories;
using System.Globalization;
using System.Text;

namespace Follower.Services
{
    public sealed class IndicadoresService : IIndicadoresService
    {
        private readonly IIndicadoresRepository _repository;

        // Función: prepara el servicio de cálculo de indicadores.
        // Flujo: recibe por inyección el repositorio que carga datos autorizados.
        public IndicadoresService(IIndicadoresRepository repository)
        {
            _repository = repository;
        }

        // Función: carga los datos autorizados del panel y prepara sus opciones de generación.
        // Flujo: obtiene el conjunto del repositorio, agrupa los años en periodos de tres años y registra si hay egresados sin año.
        public async Task<IndicadoresDatosDto?> CargarDatosAsync(int idResponsable)
        {
            var datos = await _repository.ObtenerDatosAsync(idResponsable);
            if (datos == null)
            {
                return null;
            }

            datos.TieneEgresadosSinGeneracion = datos.Egresados.Any(egresado => !egresado.FechaEgreso.HasValue);
            var anios = datos.Egresados
                .Where(egresado => egresado.FechaEgreso.HasValue)
                .Select(egresado => egresado.FechaEgreso!.Value)
                .Distinct()
                .OrderBy(anio => anio)
                .ToList();
            if (anios.Count > 0)
            {
                var primerAnio = anios[0];
                datos.Generaciones = anios
                    .Select(anio => primerAnio + ((anio - primerAnio) / 3) * 3)
                    .Distinct()
                    .OrderBy(inicio => inicio)
                    .Select(inicio => new IndicadorGeneracionDto(inicio, $"{inicio}-{inicio + 2}"))
                    .ToList();
            }

            return datos;
        }

        // Función: calcula los indicadores del panel para la generación indicada.
        // Flujo: filtra la población y genera conteos de actualización, generación, situación laboral, sector, doctorado y becas.
        public IndicadoresPanelDto CalcularIndicadores(IndicadoresDatosDto datos, string generacionSeleccionada)
        {
            var generacion = datos.Generaciones.FirstOrDefault(opcion =>
                opcion.Start.ToString(CultureInfo.InvariantCulture) == generacionSeleccionada);
            var poblacion = generacionSeleccionada switch
            {
                "sin-generacion" => datos.Egresados.Where(egresado => !egresado.FechaEgreso.HasValue).ToList(),
                "todas" => datos.Egresados,
                _ when generacion != null => datos.Egresados.Where(egresado =>
                    egresado.FechaEgreso >= generacion.Start
                    && egresado.FechaEgreso <= generacion.Start + 2).ToList(),
                _ => datos.Egresados
            };

            var fechaLimite = DateTime.UtcNow.AddMonths(-12);
            var perfilesActualizados = poblacion.Count(egresado => egresado.FechaUltimaActualizacion >= fechaLimite);
            var distribucionGeneracion = poblacion
                .GroupBy(egresado => egresado.FechaEgreso.HasValue
                    ? ObtenerEtiquetaGeneracion(egresado.FechaEgreso.Value, datos.Generaciones)
                    : "Sin generación")
                .OrderBy(grupo => grupo.Key)
                .Select(grupo => new IndicadorConteoDto(grupo.Key, grupo.Count()))
                .ToList();

            var conEmpleo = poblacion.Count(ReportaEmpleoVigente);
            var sinEmpleo = poblacion.Count(egresado => !ReportaEmpleoVigente(egresado) && EsBusquedaEmpleo(egresado));
            var sinInformacion = Math.Max(0, poblacion.Count - conEmpleo - sinEmpleo);
            var situacionLaboral = new List<IndicadorConteoDto>
            {
                new("Empleo vigente", conEmpleo),
                new("Sin empleo", sinEmpleo),
                new("Sin información", sinInformacion)
            };

            var sectoresEmpleo = new List<IndicadorConteoDto>
            {
                new("Público", ContarPerfilesPorSector(poblacion, "Público"), "bg-blue-700"),
                new("Privado", ContarPerfilesPorSector(poblacion, "Privado"), "bg-indigo-500"),
                new("Social", ContarPerfilesPorSector(poblacion, "Social"), "bg-teal-600"),
                new("Educativo / otro", ContarPerfilesPorSector(poblacion, "Educativo", "Otro"), "bg-gray-500")
            };

            var conBeca = poblacion.Where(egresado => egresado.FormacionesAcademicas.Any(formacion => formacion.RecibioBeca)).ToList();
            var becas = new List<IndicadorConteoDto>
            {
                new("Beca nacional", conBeca.Count(egresado => egresado.FormacionesAcademicas.Any(formacion => formacion.RecibioBeca && EsBecaNacional(formacion.TipoApoyo))), "bg-[#003366]"),
                new("Beca internacional", conBeca.Count(egresado => egresado.FormacionesAcademicas.Any(formacion => formacion.RecibioBeca && EsBecaInternacional(formacion.TipoApoyo))), "bg-violet-600"),
                new("Tipo sin clasificar", conBeca.Count(egresado => egresado.FormacionesAcademicas.Any(formacion => formacion.RecibioBeca && !EsBecaNacional(formacion.TipoApoyo) && !EsBecaInternacional(formacion.TipoApoyo))), "bg-gray-400")
            };

            return new IndicadoresPanelDto
            {
                NombrePrograma = datos.NombrePrograma,
                Poblacion = poblacion,
                Generaciones = datos.Generaciones,
                TieneEgresadosSinGeneracion = datos.TieneEgresadosSinGeneracion,
                UltimaActualizacion = datos.UltimaActualizacion,
                TotalEgresados = poblacion.Count,
                PerfilesActualizados = perfilesActualizados,
                SinEmpleo = sinEmpleo,
                DoctoradoCursando = ContarPerfilesDoctorales(poblacion, cursando: true),
                DoctoradoConcluido = ContarPerfilesDoctorales(poblacion, cursando: false),
                DistribucionGeneracion = distribucionGeneracion,
                SituacionLaboral = situacionLaboral,
                SectoresEmpleo = sectoresEmpleo,
                Becas = becas
            };
        }

        // Función: genera la etiqueta del periodo de tres años al que pertenece un año de egreso.
        // Flujo: calcula el inicio del periodo usando la primera generación disponible y construye su etiqueta.
        private static string ObtenerEtiquetaGeneracion(int anio, IReadOnlyList<IndicadorGeneracionDto> generaciones)
        {
            var inicio = generaciones.FirstOrDefault()?.Start ?? anio;
            var inicioPeriodo = inicio + ((anio - inicio) / 3) * 3;
            return $"{inicioPeriodo}-{inicioPeriodo + 2}";
        }

        // Función: determina si el egresado reporta empleo vigente.
        // Flujo: revisa empleos actuales y el estado laboral registrado.
        private static bool ReportaEmpleoVigente(Egresado egresado)
        {
            return egresado.ExperienciasLaborales.Any(experiencia => experiencia.EsEmpleoActual)
                || (egresado.SituacionLaboral?.EstadoActual?.StartsWith("Empleo", StringComparison.OrdinalIgnoreCase) ?? false)
                || (egresado.SituacionLaboral?.EstadoActual?.StartsWith("Trabajador independiente", StringComparison.OrdinalIgnoreCase) ?? false);
        }

        // Función: determina si el egresado reportó buscar empleo o estudiar exclusivamente.
        // Flujo: consulta el estado laboral y comprueba si contiene alguna de esas situaciones.
        private static bool EsBusquedaEmpleo(Egresado egresado)
        {
            var estado = egresado.SituacionLaboral?.EstadoActual;
            return !string.IsNullOrWhiteSpace(estado)
                && (estado.Contains("Buscando empleo", StringComparison.OrdinalIgnoreCase)
                    || estado.Contains("Estudiando exclusivamente", StringComparison.OrdinalIgnoreCase));
        }

        // Función: cuenta egresados con empleo actual en alguno de los sectores indicados.
        // Flujo: normaliza los sectores solicitados y cuenta perfiles con una experiencia actual coincidente.
        private static int ContarPerfilesPorSector(IReadOnlyCollection<Egresado> poblacion, params string[] sectores)
        {
            var normalizados = sectores.Select(Normalizar).ToHashSet(StringComparer.Ordinal);
            return poblacion.Count(egresado => egresado.ExperienciasLaborales.Any(experiencia =>
                experiencia.EsEmpleoActual
                && experiencia.Sector != null
                && normalizados.Contains(Normalizar(experiencia.Sector))));
        }

        // Función: cuenta perfiles con formación doctoral cursando o concluida.
        // Flujo: filtra formaciones de doctorado por el indicador actual o por el estado de conclusión.
        private static int ContarPerfilesDoctorales(IReadOnlyCollection<Egresado> poblacion, bool cursando)
        {
            return poblacion.Count(egresado => egresado.FormacionesAcademicas.Any(formacion =>
                string.Equals(formacion.Grado, "Doctorado", StringComparison.OrdinalIgnoreCase)
                && (cursando
                    ? formacion.EsFormacionActual || formacion.EstadoPrograma?.Contains("cursando", StringComparison.OrdinalIgnoreCase) == true
                    : !formacion.EsFormacionActual && formacion.EstadoPrograma?.Contains("concluí", StringComparison.OrdinalIgnoreCase) == true)));
        }

        // Función: clasifica una beca como internacional.
        // Flujo: normaliza el tipo de apoyo y busca la categoría internacional.
        private static bool EsBecaInternacional(string? tipoApoyo)
        {
            return Normalizar(tipoApoyo).Contains("internacional", StringComparison.Ordinal);
        }

        // Función: clasifica una beca como nacional.
        // Flujo: comprueba la categoría nacional y descarta los apoyos internacionales.
        private static bool EsBecaNacional(string? tipoApoyo)
        {
            return Normalizar(tipoApoyo).Contains("nacional", StringComparison.Ordinal)
                && !EsBecaInternacional(tipoApoyo);
        }

        // Función: normaliza texto para comparar categorías sin diferencias de acentos o mayúsculas.
        // Flujo: descompone los caracteres, elimina marcas diacríticas, recompone y devuelve el texto en minúsculas.
        private static string Normalizar(string? texto)
        {
            return (texto ?? string.Empty).Normalize(NormalizationForm.FormD)
                .Where(caracter => CharUnicodeInfo.GetUnicodeCategory(caracter) != UnicodeCategory.NonSpacingMark)
                .Aggregate(new StringBuilder(), (builder, caracter) => builder.Append(caracter))
                .ToString().Normalize(NormalizationForm.FormC).Trim().ToLowerInvariant();
        }
    }
}
