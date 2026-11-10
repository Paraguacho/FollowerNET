using Follower.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Follower.Services;

public sealed class EvidenciaPdfService : IEvidenciaPdfService
{
    public const long TamanoMaximo = 10 * 1024 * 1024;
    private readonly string _directorioRaiz;

    public EvidenciaPdfService(IWebHostEnvironment environment)
    {
        _directorioRaiz = Path.Combine(environment.ContentRootPath, "App_Data", "Evidencias");
    }

    public Task<IReadOnlyList<EvidenciaPdf>> ListarAsync(int idEgresado)
    {
        var directorio = ObtenerDirectorio(idEgresado);
        if (!Directory.Exists(directorio))
        {
            return Task.FromResult<IReadOnlyList<EvidenciaPdf>>(Array.Empty<EvidenciaPdf>());
        }

        IReadOnlyList<EvidenciaPdf> archivos = Directory.EnumerateFiles(directorio, "*.pdf")
            .Select(path => CrearEvidencia(path))
            .Where(evidencia => evidencia is not null)
            .Select(evidencia => evidencia!)
            .OrderBy(evidencia => evidencia.Nombre, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return Task.FromResult(archivos);
    }

    public async Task<EvidenciaPdf> GuardarAsync(int idEgresado, IBrowserFile archivo)
    {
        if (idEgresado <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idEgresado));
        }

        if (!string.Equals(Path.GetExtension(archivo.Name), ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException("Solo se permiten archivos PDF.");
        }

        if (archivo.Size is <= 0 or > TamanoMaximo)
        {
            throw new InvalidDataException("El archivo debe tener un tamaño máximo de 10 MB.");
        }

        await using var input = archivo.OpenReadStream(TamanoMaximo);
        using var buffer = new MemoryStream();
        await input.CopyToAsync(buffer);
        var bytes = buffer.ToArray();
        if (bytes.Length < 5 || !bytes.AsSpan(0, 5).SequenceEqual("%PDF-"u8))
        {
            throw new InvalidDataException("El archivo seleccionado no contiene un documento PDF válido.");
        }

        var id = Guid.NewGuid();
        var nombre = LimpiarNombre(Path.GetFileNameWithoutExtension(archivo.Name));
        var directorio = ObtenerDirectorio(idEgresado);
        Directory.CreateDirectory(directorio);
        var path = Path.Combine(directorio, $"{id:N}--{nombre}.pdf");
        await File.WriteAllBytesAsync(path, bytes);
        return new EvidenciaPdf(id, Path.GetFileName(archivo.Name), bytes.Length);
    }

    public async Task<byte[]> LeerAsync(int idEgresado, Guid idArchivo)
    {
        var prefijo = $"{idArchivo:N}--";
        var path = Directory.Exists(ObtenerDirectorio(idEgresado))
            ? Directory.EnumerateFiles(ObtenerDirectorio(idEgresado), $"{prefijo}*.pdf").SingleOrDefault()
            : null;

        if (path is null)
        {
            throw new FileNotFoundException("No se encontró el documento solicitado.");
        }

        return await File.ReadAllBytesAsync(path);
    }

    private string ObtenerDirectorio(int idEgresado) =>
        Path.Combine(_directorioRaiz, idEgresado.ToString(System.Globalization.CultureInfo.InvariantCulture));

    private static EvidenciaPdf? CrearEvidencia(string path)
    {
        var fileName = Path.GetFileName(path);
        var separator = fileName.IndexOf("--", StringComparison.Ordinal);
        if (separator != 32 || !Guid.TryParseExact(fileName[..separator], "N", out var id))
        {
            return null;
        }

        return new EvidenciaPdf(id, fileName[(separator + 2)..], new FileInfo(path).Length);
    }

    private static string LimpiarNombre(string value)
    {
        var invalidCharacters = Path.GetInvalidFileNameChars();
        var clean = new string(value.Where(character =>
            !invalidCharacters.Contains(character) && character != '/' && character != '\\').ToArray()).Trim();

        return string.IsNullOrWhiteSpace(clean) ? "evidencia" : clean;
    }
}
