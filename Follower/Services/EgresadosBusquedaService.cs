using Follower.Data;
using Follower.DTOs;
using Follower.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Follower.Services
{
    public sealed class EgresadosBusquedaService : IEgresadosBusquedaService
    {
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
        private readonly ApplicationDbContext _context;

        public EgresadosBusquedaService(
            AuthenticationStateProvider authStateProvider,
            ApplicationDbContext context)
        {
            _authStateProvider = authStateProvider;
            _context = context;
        }

        public async Task<EgresadosBusquedaOpcionesDto> ObtenerOpcionesAsync(CancellationToken cancellationToken = default)
        {
            var programa = await ObtenerProgramaAutorizadoAsync(cancellationToken);

            // Consultamos directamente el DbSet SituacionesLaborales y filtramos cruzando con Egresados
            var situacionesRegistradas = await _context.SituacionesLaborales
                .Where(sl => _context.Egresados.Any(e => e.IdEgresado == sl.EgresadoId && e.IdPrograma == programa.IdPrograma && e.EstadoSolicitud == "Aprobado"))
                .Where(sl => sl.EstadoActual != null && sl.EstadoActual.Trim() != "")
                .Select(sl => sl.EstadoActual)
                .Distinct()
                .OrderBy(estado => estado)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var situacionesLaborales = SituacionesLaboralesDisponibles
                .Concat(situacionesRegistradas!)
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
            if (idEgresado <= 0) return null;

            var programa = await ObtenerProgramaAutorizadoAsync(cancellationToken);

            return await _context.Egresados
                .Include(e => e.ExperienciasLaborales)
                .Include(e => e.FormacionesAcademicas)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdEgresado == idEgresado && e.IdPrograma == programa.IdPrograma, cancellationToken);
        }

        public async Task<EgresadosBusquedaPaginaDto> BuscarAsync(
            EgresadosBusquedaFiltrosDto filtros,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filtros);

            var programa = await ObtenerProgramaAutorizadoAsync(cancellationToken);
            var filtrosNormalizados = NormalizarFiltros(filtros);

            // 1. Iniciar la consulta base
            var query = _context.Egresados
                .Where(e => e.IdPrograma == programa.IdPrograma && e.EstadoSolicitud == "Aprobado")
                .AsNoTracking();

            // 2. Aplicar filtros dinámicos
            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.TerminoBusqueda))
            {
                var termino = filtrosNormalizados.TerminoBusqueda.ToLower();
                query = query.Where(e =>
                    e.Nombres.ToLower().Contains(termino) ||
                    e.ApellidoPaterno.ToLower().Contains(termino) ||
                    e.ApellidoMaterno.ToLower().Contains(termino));
            }

            if (filtrosNormalizados.AnoEgresadoDesde.HasValue)
            {
                query = query.Where(e => e.FechaEgreso >= filtrosNormalizados.AnoEgresadoDesde.Value);
            }

            if (filtrosNormalizados.AnoEgresoHasta.HasValue)
            {
                query = query.Where(e => e.FechaEgreso <= filtrosNormalizados.AnoEgresoHasta.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.SituacionLaboral))
            {
                query = query.Where(e => _context.SituacionesLaborales.Any(sl => sl.EgresadoId == e.IdEgresado && sl.EstadoActual == filtrosNormalizados.SituacionLaboral));
            }

            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.Sector))
            {
                query = query.Where(e => _context.ExperienciasLaborales.Any(el =>
                    el.EgresadoId == e.IdEgresado && el.EsEmpleoActual && el.Sector == filtrosNormalizados.Sector));
            }

            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.NivelEstudioActual))
            {
                query = query.Where(e => _context.FormacionesAcademicas.Any(fa =>
                    fa.EgresadoId == e.IdEgresado && fa.EsFormacionActual && fa.Grado == filtrosNormalizados.NivelEstudioActual));
            }

            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.InstitucionActual))
            {
                var inst = filtrosNormalizados.InstitucionActual.ToLower();
                query = query.Where(e => _context.ExperienciasLaborales.Any(el =>
                    el.EgresadoId == e.IdEgresado && el.EsEmpleoActual && el.Institucion.ToLower().Contains(inst)));
            }

            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.Ciudad))
            {
                var ciudad = filtrosNormalizados.Ciudad.ToLower();
                query = query.Where(e => e.Ciudad != null && e.Ciudad.ToLower().Contains(ciudad));
            }

            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.Estado))
            {
                var estado = filtrosNormalizados.Estado.ToLower();
                query = query.Where(e => e.Estado != null && e.Estado.ToLower().Contains(estado));
            }

            if (!string.IsNullOrWhiteSpace(filtrosNormalizados.Pais))
            {
                var pais = filtrosNormalizados.Pais.ToLower();
                query = query.Where(e => e.Pais != null && e.Pais.ToLower().Contains(pais));
            }

            if (filtrosNormalizados.SoloDesactualizados)
            {
                var fechaLimite = DateTime.UtcNow.AddMonths(-12);
                query = query.Where(e => e.FechaUltimaActualizacion < fechaLimite);
            }

            // 3. Conteo Total
            var total = await query.CountAsync(cancellationToken);

            // 4. Paginación y Ordenamiento
            var tamanoPagina = filtrosNormalizados.RegistrosPorPagina;
            var totalPaginas = total == 0 ? 1 : (total - 1) / tamanoPagina + 1;
            var pagina = (int)Math.Min(filtrosNormalizados.Pagina, Math.Min(totalPaginas, int.MaxValue));
            var offset = (pagina - 1) * tamanoPagina;

            query = filtrosNormalizados.OrdenarPor switch
            {
                EgresadosOrdenCampo.Nombre => filtrosNormalizados.OrdenAscendente
                    ? query.OrderBy(e => e.Nombres).ThenBy(e => e.ApellidoPaterno)
                    : query.OrderByDescending(e => e.Nombres).ThenByDescending(e => e.ApellidoPaterno),

                EgresadosOrdenCampo.FechaEgreso => filtrosNormalizados.OrdenAscendente
                    ? query.OrderBy(e => e.FechaEgreso)
                    : query.OrderByDescending(e => e.FechaEgreso),

                EgresadosOrdenCampo.FechaUltimaActualizacion => filtrosNormalizados.OrdenAscendente
                    ? query.OrderBy(e => e.FechaUltimaActualizacion)
                    : query.OrderByDescending(e => e.FechaUltimaActualizacion),

                _ => query.OrderBy(e => e.IdEgresado)
            };

            // 5. Ejecución y Mapeo al DTO
            var resultados = await query
                .Skip(offset)
                .Take(tamanoPagina)
                .Select(e => new EgresadosBusquedaFilaDto
                {
                    IdEgresado = e.IdEgresado,
                    Nombres = e.Nombres,
                    ApellidoPaterno = e.ApellidoPaterno,
                    ApellidoMaterno = e.ApellidoMaterno,
                    FechaEgreso = e.FechaEgreso,
                    SituacionLaboral = _context.SituacionesLaborales
                                        .Where(sl => sl.EgresadoId == e.IdEgresado)
                                        .Select(sl => sl.EstadoActual)
                                        .FirstOrDefault(),
                    FechaUltimaActualizacion = e.FechaUltimaActualizacion
                })
                .ToListAsync(cancellationToken);

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

            var programas = await _context.ProgramasResponsables
                .Include(pr => pr.ProgramaEducativo)
                .Where(pr => pr.IdUsuario == usuarioId)
                .Select(pr => new { pr.IdPrograma, pr.ProgramaEducativo.Siglas })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return programas.Count switch
            {
                0 => throw new UnauthorizedAccessException("No tienes un programa educativo autorizado."),
                1 => (programas[0].IdPrograma, programas[0].Siglas),
                _ => throw new InvalidOperationException("La búsqueda requiere que el responsable tenga un solo programa asignado.")
            };
        }

        private static EgresadosBusquedaFiltrosDto NormalizarFiltros(EgresadosBusquedaFiltrosDto filtros)
        {
            var resultado = new EgresadosBusquedaFiltrosDto
            {
                TerminoBusqueda = NormalizarTexto(filtros.TerminoBusqueda, 150),
                AnoEgresadoDesde = filtros.AnoEgresadoDesde,
                AnoEgresoHasta = filtros.AnoEgresoHasta,
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

            if (resultado.AnoEgresadoDesde.HasValue && resultado.AnoEgresoHasta.HasValue
                && resultado.AnoEgresadoDesde > resultado.AnoEgresoHasta)
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
    }
}