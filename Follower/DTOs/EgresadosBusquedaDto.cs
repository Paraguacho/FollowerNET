namespace Follower.DTOs
{
    public enum EgresadosOrdenCampo
    {
        Nombre,
        FechaEgreso,
        SituacionLaboral,
        FechaUltimaActualizacion
    }

    public sealed class EgresadosBusquedaFiltrosDto
    {
        public string? TerminoBusqueda { get; set; }
        public int? AnioEgresoDesde { get; set; }
        public int? AnioEgresoHasta { get; set; }
        public string? SituacionLaboral { get; set; }
        public string? Sector { get; set; }
        public string? NivelEstudioActual { get; set; }
        public string? InstitucionActual { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        public string? Pais { get; set; }
        public bool SoloDesactualizados { get; set; }
        public int Pagina { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;
        public EgresadosOrdenCampo OrdenarPor { get; set; } = EgresadosOrdenCampo.FechaUltimaActualizacion;
        public bool OrdenAscendente { get; set; }
    }

    public sealed class EgresadosBusquedaOpcionesDto
    {
        public string SiglasPrograma { get; init; } = string.Empty;
        public IReadOnlyList<string> SituacionesLaborales { get; init; } = Array.Empty<string>();
        public IReadOnlyList<string> Sectores { get; init; } = Array.Empty<string>();
        public IReadOnlyList<string> NivelesEstudio { get; init; } = Array.Empty<string>();
    }

    public sealed class EgresadosBusquedaFilaDto
    {
        public int IdEgresado { get; init; }
        public string Nombres { get; init; } = string.Empty;
        public string ApellidoPaterno { get; init; } = string.Empty;
        public string ApellidoMaterno { get; init; } = string.Empty;
        public int? FechaEgreso { get; init; }
        public string? SituacionLaboral { get; init; }
        public DateTime FechaUltimaActualizacion { get; init; }
    }

    public sealed class EgresadosBusquedaPaginaDto
    {
        public IReadOnlyList<EgresadosBusquedaFilaDto> Resultados { get; init; } = Array.Empty<EgresadosBusquedaFilaDto>();
        public long TotalResultados { get; init; }
        public int Pagina { get; init; }
        public int RegistrosPorPagina { get; init; }
    }
}
