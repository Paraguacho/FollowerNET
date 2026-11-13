using Follower.Models;

namespace Follower.DTOs
{
    public class IndicadoresDatosDto
    {
        public string NombrePrograma { get; set; } = "Programa autorizado";
        public List<Egresado> Egresados { get; set; } = new();
        public List<IndicadorGeneracionDto> Generaciones { get; set; } = new();
        public bool TieneEgresadosSinGeneracion { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }

    public class IndicadoresPanelDto
    {
        public string NombrePrograma { get; set; } = "Programa autorizado";
        public List<Egresado> Poblacion { get; set; } = new();
        public List<IndicadorGeneracionDto> Generaciones { get; set; } = new();
        public bool TieneEgresadosSinGeneracion { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public int TotalEgresados { get; set; }
        public int PerfilesActualizados { get; set; }
        public int SinEmpleo { get; set; }
        public int DoctoradoCursando { get; set; }
        public int DoctoradoConcluido { get; set; }
        public List<IndicadorConteoDto> DistribucionGeneracion { get; set; } = new();
        public List<IndicadorConteoDto> SituacionLaboral { get; set; } = new();
        public List<IndicadorConteoDto> SectoresEmpleo { get; set; } = new();
        public List<IndicadorConteoDto> Becas { get; set; } = new();
    }

    public record IndicadorGeneracionDto(int Start, string Label);
    public record IndicadorConteoDto(string Label, int Count, string Color = "bg-[#003366]");
}
