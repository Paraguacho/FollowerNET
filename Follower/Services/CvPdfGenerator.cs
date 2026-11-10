using System.Globalization;
using System.Text;
using Follower.Models;

namespace Follower.Services;

public static class CvPdfGenerator
{
    private static readonly CultureInfo CulturaEspanol = CultureInfo.GetCultureInfo("es-MX");

    public static byte[] Generar(Egresado egresado)
    {
        var lineas = new List<PdfLine>();
        AgregarTextoEnvuelto(lineas, NombreCompleto(egresado), PdfLineStyle.Nombre);
        AgregarTextoEnvuelto(
            lineas,
            egresado.ProgramaEducativo?.NombrePrograma ?? "Programa educativo no especificado",
            PdfLineStyle.Subtitulo);

        AgregarDato(lineas, "Correo", egresado.Usuario?.Correo);
        AgregarDato(lineas, "Teléfono", egresado.Telefono);
        AgregarDato(lineas, "Residencia", string.Join(", ", new[] { egresado.Ciudad, egresado.Estado, egresado.Pais }
            .Where(value => !string.IsNullOrWhiteSpace(value))));
        AgregarDato(lineas, "Año de egreso", egresado.FechaEgreso?.ToString(CulturaEspanol));

        AgregarSeccion(lineas, "TRAYECTORIA LABORAL");
        if (egresado.ExperienciasLaborales.Count == 0)
        {
            lineas.Add(new PdfLine("Sin experiencias laborales registradas.", PdfLineStyle.Cuerpo));
        }
        else
        {
            foreach (var empleo in egresado.ExperienciasLaborales.OrderBy(item => item.FechaInicio))
            {
                lineas.Add(new PdfLine(empleo.Puesto, PdfLineStyle.Entrada));
                AgregarDato(lineas, "Institución", empleo.Institucion);
                AgregarDato(lineas, "Periodo", RangoFechas(empleo.FechaInicio, empleo.FechaFin, empleo.EsEmpleoActual));
                AgregarDato(lineas, "Sector", empleo.Sector);
                AgregarDato(lineas, "Actividades", empleo.ActividadesPrincipales, PdfLineStyle.Cuerpo);
            }
        }

        AgregarSeccion(lineas, "FORMACIÓN ACADÉMICA");
        if (egresado.FormacionesAcademicas.Count == 0)
        {
            lineas.Add(new PdfLine("Sin estudios registrados.", PdfLineStyle.Cuerpo));
        }
        else
        {
            foreach (var estudio in egresado.FormacionesAcademicas.OrderBy(item => item.FechaInicio))
            {
                lineas.Add(new PdfLine(estudio.NombrePrograma, PdfLineStyle.Entrada));
                AgregarDato(lineas, "Grado", estudio.Grado);
                AgregarDato(lineas, "Institución", estudio.Institucion);
                AgregarDato(lineas, "Periodo", RangoFechas(estudio.FechaInicio, estudio.FechaFin, estudio.EsFormacionActual));
            }
        }

        var perfiles = new[]
        {
            ("LinkedIn", egresado.PerfilAcademico?.LinkedIn),
            ("ORCID", egresado.PerfilAcademico?.Orcid),
            ("Google Scholar", egresado.PerfilAcademico?.GoogleScholar),
            ("ResearchGate", egresado.PerfilAcademico?.ResearchGate),
            ("Academia.edu", egresado.PerfilAcademico?.AcademiaEdu),
            ("Scopus ID", egresado.PerfilAcademico?.ScopusId)
        };
        if (perfiles.Any(perfil => !string.IsNullOrWhiteSpace(perfil.Item2)))
        {
            AgregarSeccion(lineas, "PERFILES ACADÉMICOS");
            foreach (var (nombre, valor) in perfiles)
            {
                AgregarDato(lineas, nombre, valor);
            }
        }

        var respuestas = egresado.ValoresCamposPersonalizados
            .Where(valor => !string.IsNullOrWhiteSpace(valor.ValorCapturado))
            .OrderBy(valor => valor.CampoPersonalizado.SeccionPersonalizada.OrdenAparicion)
            .ThenBy(valor => valor.CampoPersonalizado.Orden)
            .ToList();
        if (respuestas.Count > 0)
        {
            AgregarSeccion(lineas, "RESPUESTAS PERSONALIZADAS MIED");
            foreach (var respuesta in respuestas)
            {
                lineas.Add(new PdfLine(
                    $"{respuesta.CampoPersonalizado.SeccionPersonalizada.TituloSeccion} · {respuesta.CampoPersonalizado.NombreEtiqueta}",
                    PdfLineStyle.Entrada));
                AgregarTextoEnvuelto(lineas, respuesta.ValorCapturado, PdfLineStyle.Cuerpo);
            }
        }

        return CrearPdf(lineas);
    }

    private static string NombreCompleto(Egresado egresado) =>
        $"{egresado.Nombres} {egresado.ApellidoPaterno} {egresado.ApellidoMaterno}".Trim();

    private static void AgregarSeccion(List<PdfLine> lineas, string titulo)
    {
        lineas.Add(new PdfLine(string.Empty, PdfLineStyle.Espacio));
        lineas.Add(new PdfLine(titulo, PdfLineStyle.Seccion));
    }

    private static void AgregarDato(
        List<PdfLine> lineas,
        string etiqueta,
        string? valor,
        PdfLineStyle estilo = PdfLineStyle.Secundario)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return;
        }

        AgregarTextoEnvuelto(lineas, $"{etiqueta}: {valor}", estilo);
    }

    private static void AgregarTextoEnvuelto(List<PdfLine> lineas, string? valor, PdfLineStyle estilo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return;
        }

        foreach (var parrafo in valor.Replace("\r", string.Empty).Split('\n'))
        {
            var restante = parrafo.Trim();
            if (restante.Length == 0)
            {
                lineas.Add(new PdfLine(string.Empty, PdfLineStyle.Espacio));
                continue;
            }

            var maxCaracteres = estilo == PdfLineStyle.Nombre ? 42 : 92;
            while (restante.Length > maxCaracteres)
            {
                var corte = restante.LastIndexOf(' ', maxCaracteres);
                if (corte <= 0)
                {
                    corte = maxCaracteres;
                }

                lineas.Add(new PdfLine(restante[..corte].Trim(), estilo));
                restante = restante[corte..].Trim();
            }

            lineas.Add(new PdfLine(restante, estilo));
        }
    }

    private static byte[] CrearPdf(IReadOnlyList<PdfLine> lineas)
    {
        const float margenIzquierdo = 54;
        const float margenDerecho = 558;
        const float inicioY = 744;
        const float finY = 58;

        var paginas = new List<List<PdfLine>>();
        var paginaActual = new List<PdfLine>();
        var y = inicioY;

        foreach (var linea in lineas)
        {
            var alto = Alto(linea.Estilo);
            if (y - alto < finY || (linea.Estilo == PdfLineStyle.Seccion && y - alto < finY + 28))
            {
                paginas.Add(paginaActual);
                paginaActual = new List<PdfLine>();
                y = inicioY;
            }

            paginaActual.Add(linea);
            y -= alto;
        }

        if (paginaActual.Count > 0 || paginas.Count == 0)
        {
            paginas.Add(paginaActual);
        }

        var objetos = new List<byte[]>();
        var numerosPagina = Enumerable.Range(0, paginas.Count)
            .Select(indice => 3 + indice * 2)
            .ToList();
        var numeroFuenteRegular = 3 + paginas.Count * 2;
        var numeroFuenteNegrita = numeroFuenteRegular + 1;

        objetos.Add(Ascii("<< /Type /Catalog /Pages 2 0 R >>"));
        objetos.Add(Ascii($"<< /Type /Pages /Kids [{string.Join(" ", numerosPagina.Select(numero => $"{numero} 0 R"))}] /Count {paginas.Count} >>"));

        for (var indice = 0; indice < paginas.Count; indice++)
        {
            var numeroPagina = numerosPagina[indice];
            var numeroContenido = numeroPagina + 1;
            var contenido = CrearContenidoPagina(
                paginas[indice],
                indice + 1,
                paginas.Count,
                numeroFuenteRegular,
                numeroFuenteNegrita,
                margenIzquierdo,
                margenDerecho,
                inicioY);
            var flujo = Encoding.Latin1.GetBytes(contenido);

            objetos.Add(Ascii(
                $"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] " +
                $"/Resources << /Font << /F1 {numeroFuenteRegular} 0 R /F2 {numeroFuenteNegrita} 0 R >> >> " +
                $"/Contents {numeroContenido} 0 R >>"));
            objetos.Add(Combinar(
                Ascii($"<< /Length {flujo.Length} >>\nstream\n"),
                flujo,
                Ascii("\nendstream")));
        }

        objetos.Add(Ascii("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>"));
        objetos.Add(Ascii("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>"));

        using var salida = new MemoryStream();
        Escribir(salida, Ascii("%PDF-1.4\n%\u00e2\u00e3\u00cf\u00d3\n"));
        var desplazamientos = new List<long> { 0 };
        for (var indice = 0; indice < objetos.Count; indice++)
        {
            desplazamientos.Add(salida.Position);
            Escribir(salida, Ascii($"{indice + 1} 0 obj\n"));
            Escribir(salida, objetos[indice]);
            Escribir(salida, Ascii("\nendobj\n"));
        }

        var desplazamientoXref = salida.Position;
        Escribir(salida, Ascii($"xref\n0 {objetos.Count + 1}\n0000000000 65535 f \n"));
        foreach (var desplazamiento in desplazamientos.Skip(1))
        {
            Escribir(salida, Ascii($"{desplazamiento:D10} 00000 n \n"));
        }

        Escribir(salida, Ascii(
            $"trailer\n<< /Size {objetos.Count + 1} /Root 1 0 R >>\nstartxref\n{desplazamientoXref}\n%%EOF"));
        return salida.ToArray();
    }

    private static string CrearContenidoPagina(
        IReadOnlyList<PdfLine> lineas,
        int numeroPagina,
        int totalPaginas,
        int numeroFuenteRegular,
        int numeroFuenteNegrita,
        float margenIzquierdo,
        float margenDerecho,
        float inicioY)
    {
        var contenido = new StringBuilder();
        Texto(contenido, "CURRÍCULUM VITAE", margenIzquierdo, 770, 8, numeroFuenteNegrita, "0.00 0.20 0.40");
        contenido.Append("0.82 0.86 0.90 RG 0.7 w ")
            .Append(margenIzquierdo).Append(" 758 m ")
            .Append(margenDerecho).Append(" 758 l S\n");

        var y = inicioY;
        foreach (var linea in lineas)
        {
            if (linea.Estilo == PdfLineStyle.Seccion)
            {
                Texto(contenido, Normalizar(linea.Texto), margenIzquierdo, y, 10, numeroFuenteNegrita, "0.00 0.20 0.40");
                contenido.Append("0.82 0.86 0.90 RG 0.5 w ")
                    .Append(margenIzquierdo).Append(' ').Append(y - 6).Append(" m ")
                    .Append(margenDerecho).Append(' ').Append(y - 6).Append(" l S\n");
            }
            else if (linea.Estilo != PdfLineStyle.Espacio)
            {
                var (tamano, fuente, color) = linea.Estilo switch
                {
                    PdfLineStyle.Nombre => (22, numeroFuenteNegrita, "0.08 0.12 0.18"),
                    PdfLineStyle.Subtitulo => (11, numeroFuenteRegular, "0.00 0.20 0.40"),
                    PdfLineStyle.Entrada => (10, numeroFuenteNegrita, "0.08 0.12 0.18"),
                    PdfLineStyle.Cuerpo => (9, numeroFuenteRegular, "0.18 0.22 0.27"),
                    _ => (8, numeroFuenteRegular, "0.36 0.40 0.45")
                };
                Texto(contenido, Normalizar(linea.Texto), margenIzquierdo, y, tamano, fuente, color);
            }

            y -= Alto(linea.Estilo);
        }

        contenido.Append("0.82 0.86 0.90 RG 0.5 w 54 42 m 558 42 l S\n");
        Texto(contenido, $"Página {numeroPagina} de {totalPaginas}", 54, 28, 8, numeroFuenteRegular, "0.36 0.40 0.45");
        contenido.Append("BT /F1 8 Tf 0.36 0.40 0.45 rg 1 0 0 1 490 28 Tm (Egresados · MIED) Tj ET\n");
        return contenido.ToString();
    }

    private static void Texto(
        StringBuilder contenido,
        string texto,
        float x,
        float y,
        int tamano,
        int fuente,
        string color)
    {
        contenido.Append("BT /F").Append(fuente).Append(' ')
            .Append(tamano).Append(" Tf ").Append(color)
            .Append(" rg 1 0 0 1 ").Append(x).Append(' ').Append(y)
            .Append(" Tm (").Append(Escapar(Normalizar(texto))).Append(") Tj ET\n");
    }

    private static float Alto(PdfLineStyle estilo) => estilo switch
    {
        PdfLineStyle.Nombre => 31,
        PdfLineStyle.Subtitulo => 20,
        PdfLineStyle.Seccion => 23,
        PdfLineStyle.Entrada => 16,
        PdfLineStyle.Cuerpo => 14,
        PdfLineStyle.Espacio => 7,
        _ => 13
    };

    private static string RangoFechas(DateTime? inicio, DateTime? fin, bool actual) =>
        $"{inicio?.ToString("MMM yyyy", CulturaEspanol) ?? "No especificado"} – " +
        $"{(actual ? "Actualidad" : fin?.ToString("MMM yyyy", CulturaEspanol) ?? "No especificado")}";

    private static string Normalizar(string value)
    {
        var result = new StringBuilder(value.Length);
        foreach (var character in value)
        {
            result.Append(character switch
            {
                '\u2013' or '\u2014' => '-',
                '\u2018' or '\u2019' => '\'',
                '\u201c' or '\u201d' => '"',
                '\u2022' => '-',
                _ when character <= '\u00ff' && !char.IsControl(character) => character,
                _ when char.IsWhiteSpace(character) => ' ',
                _ => '?'
            });
        }

        return result.ToString();
    }

    private static string Escapar(string value) =>
        value.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");

    private static byte[] Ascii(string value) => Encoding.Latin1.GetBytes(value);

    private static byte[] Combinar(params byte[][] arrays)
    {
        using var stream = new MemoryStream();
        foreach (var array in arrays)
        {
            stream.Write(array);
        }

        return stream.ToArray();
    }

    private static void Escribir(Stream stream, byte[] bytes) => stream.Write(bytes);

    private sealed record PdfLine(string Texto, PdfLineStyle Estilo);

    private enum PdfLineStyle
    {
        Nombre,
        Subtitulo,
        Seccion,
        Entrada,
        Secundario,
        Cuerpo,
        Espacio
    }
}
