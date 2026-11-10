using System.Globalization;
using System.Text;
using Follower.Models;

namespace Follower.Services;

public static class CvPdfGenerator
{
    public static byte[] Generar(Egresado egresado)
    {
        var lineas = new List<string>
        {
            "CURRICULUM VITAE",
            egresado.Nombres + " " + egresado.ApellidoPaterno + " " + egresado.ApellidoMaterno,
            egresado.ProgramaEducativo?.NombrePrograma ?? "Programa educativo no especificado",
            $"Correo: {egresado.Usuario?.Correo ?? "No especificado"}",
            $"Telefono: {egresado.Telefono ?? "No especificado"}",
            $"Residencia: {string.Join(", ", new[] { egresado.Ciudad, egresado.Estado, egresado.Pais }.Where(value => !string.IsNullOrWhiteSpace(value)))}",
            $"Ano de egreso: {egresado.FechaEgreso?.ToString() ?? "No especificado"}",
            string.Empty,
            "TRAYECTORIA LABORAL"
        };

        if (egresado.ExperienciasLaborales.Count == 0)
        {
            lineas.Add("Sin experiencias laborales registradas.");
        }
        else
        {
            foreach (var empleo in egresado.ExperienciasLaborales.OrderBy(item => item.FechaInicio))
            {
                lineas.Add($"{empleo.Puesto} | {empleo.Institucion}");
                lineas.Add($"Periodo: {RangoFechas(empleo.FechaInicio, empleo.FechaFin, empleo.EsEmpleoActual)}");
                AddWrapped(lineas, empleo.ActividadesPrincipales);
            }
        }

        lineas.Add(string.Empty);
        lineas.Add("FORMACION ACADEMICA");
        if (egresado.FormacionesAcademicas.Count == 0)
        {
            lineas.Add("Sin estudios registrados.");
        }
        else
        {
            foreach (var estudio in egresado.FormacionesAcademicas.OrderBy(item => item.FechaInicio))
            {
                lineas.Add($"{estudio.NombrePrograma} | {estudio.Grado} | {estudio.Institucion}");
                lineas.Add($"Periodo: {RangoFechas(estudio.FechaInicio, estudio.FechaFin, estudio.EsFormacionActual)}");
            }
        }

        lineas.Add(string.Empty);
        lineas.Add("PERFILES ACADEMICOS");
        lineas.Add($"LinkedIn: {egresado.PerfilAcademico?.LinkedIn ?? "No especificado"}");
        lineas.Add($"ORCID: {egresado.PerfilAcademico?.Orcid ?? "No especificado"}");
        lineas.Add($"Google Scholar: {egresado.PerfilAcademico?.GoogleScholar ?? "No especificado"}");
        lineas.Add($"ResearchGate: {egresado.PerfilAcademico?.ResearchGate ?? "No especificado"}");
        lineas.Add($"Academia.edu: {egresado.PerfilAcademico?.AcademiaEdu ?? "No especificado"}");
        lineas.Add($"Scopus ID: {egresado.PerfilAcademico?.ScopusId ?? "No especificado"}");

        var respuestas = egresado.ValoresCamposPersonalizados
            .Where(valor => !string.IsNullOrWhiteSpace(valor.ValorCapturado))
            .OrderBy(valor => valor.CampoPersonalizado.SeccionPersonalizada.OrdenAparicion)
            .ThenBy(valor => valor.CampoPersonalizado.Orden)
            .ToList();
        if (respuestas.Count > 0)
        {
            lineas.Add(string.Empty);
            lineas.Add("RESPUESTAS PERSONALIZADAS MIED");
            foreach (var respuesta in respuestas)
            {
                lineas.Add($"{respuesta.CampoPersonalizado.SeccionPersonalizada.TituloSeccion} - {respuesta.CampoPersonalizado.NombreEtiqueta}");
                AddWrapped(lineas, respuesta.ValorCapturado);
            }
        }

        return CrearPdf(lineas);
    }

    private static void AddWrapped(List<string> lines, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        foreach (var paragraph in value.Replace("\r", string.Empty).Split('\n'))
        {
            var remaining = paragraph;
            while (remaining.Length > 90)
            {
                var breakAt = remaining.LastIndexOf(' ', 90);
                if (breakAt <= 0)
                {
                    breakAt = 90;
                }

                lines.Add("  " + remaining[..breakAt].Trim());
                remaining = remaining[breakAt..].Trim();
            }

            lines.Add("  " + remaining);
        }
    }

    private static byte[] CrearPdf(IEnumerable<string> sourceLines)
    {
        var lines = sourceLines.Select(Normalizar).ToList();
        var pages = lines.Chunk(45).ToList();
        if (pages.Count == 0)
        {
            pages.Add(new[] { "CURRICULUM VITAE" });
        }

        var objects = new List<byte[]>();
        var pageObjectNumbers = Enumerable.Range(0, pages.Count)
            .Select(index => 3 + index * 2)
            .ToList();
        var fontObjectNumber = 3 + pages.Count * 2;

        objects.Add(Ascii("<< /Type /Catalog /Pages 2 0 R >>"));
        objects.Add(Ascii($"<< /Type /Pages /Kids [{string.Join(" ", pageObjectNumbers.Select(number => $"{number} 0 R"))}] /Count {pages.Count} >>"));

        for (var index = 0; index < pages.Count; index++)
        {
            var pageObjectNumber = pageObjectNumbers[index];
            var contentObjectNumber = pageObjectNumber + 1;
            var content = new StringBuilder("BT /F1 10 Tf 50 760 Td 16 TL\n");
            foreach (var line in pages[index])
            {
                content.Append('(').Append(EscapePdfString(line)).Append(") Tj T*\n");
            }
            content.Append("ET");
            var stream = Encoding.Latin1.GetBytes(content.ToString());

            objects.Add(Ascii($"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 {fontObjectNumber} 0 R >> >> /Contents {contentObjectNumber} 0 R >>"));
            objects.Add(Combine(Ascii($"<< /Length {stream.Length} >>\nstream\n"), stream, Ascii("\nendstream")));
        }

        objects.Add(Ascii("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>"));

        using var output = new MemoryStream();
        Write(output, Ascii("%PDF-1.4\n%\u00e2\u00e3\u00cf\u00d3\n"));
        var offsets = new List<long> { 0 };
        for (var index = 0; index < objects.Count; index++)
        {
            offsets.Add(output.Position);
            Write(output, Ascii($"{index + 1} 0 obj\n"));
            Write(output, objects[index]);
            Write(output, Ascii("\nendobj\n"));
        }

        var xrefOffset = output.Position;
        Write(output, Ascii($"xref\n0 {objects.Count + 1}\n0000000000 65535 f \n"));
        foreach (var offset in offsets.Skip(1))
        {
            Write(output, Ascii($"{offset:D10} 00000 n \n"));
        }
        Write(output, Ascii($"trailer\n<< /Size {objects.Count + 1} /Root 1 0 R >>\nstartxref\n{xrefOffset}\n%%EOF"));
        return output.ToArray();
    }

    private static string RangoFechas(DateTime? inicio, DateTime? fin, bool actual) =>
        $"{inicio?.ToString("MMM yyyy", CultureInfo.GetCultureInfo("es-MX")) ?? "N/D"} - " +
        $"{(actual ? "Actualidad" : fin?.ToString("MMM yyyy", CultureInfo.GetCultureInfo("es-MX")) ?? "N/D")}";

    private static string Normalizar(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var result = new StringBuilder(normalized.Length);
        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            result.Append(character switch
            {
                '\u2013' or '\u2014' => '-',
                '\u2018' or '\u2019' => '\'',
                '\u201c' or '\u201d' => '"',
                _ => character <= '\u00ff' ? character : '?'
            });
        }

        return result.ToString();
    }

    private static string EscapePdfString(string value) =>
        value.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");

    private static byte[] Ascii(string value) => Encoding.Latin1.GetBytes(value);

    private static byte[] Combine(params byte[][] arrays)
    {
        using var stream = new MemoryStream();
        foreach (var array in arrays)
        {
            stream.Write(array);
        }
        return stream.ToArray();
    }

    private static void Write(Stream stream, byte[] bytes) => stream.Write(bytes);
}
