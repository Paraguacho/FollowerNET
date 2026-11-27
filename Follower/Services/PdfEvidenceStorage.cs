using Microsoft.AspNetCore.Components.Forms;

namespace Follower.Services
{
    public sealed class PdfEvidenceStorage
    {
        public const long MaxFileSize = 5 * 1024 * 1024;
        private const string RelativeDirectory = "evidencias/";
        private readonly IWebHostEnvironment _environment;

        public PdfEvidenceStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveAsync(IBrowserFile file)
        {
            if (!string.Equals(Path.GetExtension(file.Name), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("El archivo debe tener formato PDF.");
            }

            if (file.Size == 0 || file.Size > MaxFileSize)
            {
                throw new InvalidDataException("El PDF debe pesar más de 0 y como máximo 5 MB.");
            }

            await using var input = file.OpenReadStream(MaxFileSize);
            using var buffer = new MemoryStream();
            await input.CopyToAsync(buffer);

            var bytes = buffer.ToArray();
            if (bytes.Length < 5 || !bytes.AsSpan(0, 5).SequenceEqual("%PDF-"u8))
            {
                throw new InvalidDataException("El archivo seleccionado no contiene un PDF válido.");
            }

            var fileName = $"{Guid.NewGuid():N}.pdf";
            var directory = GetStorageDirectory();
            Directory.CreateDirectory(directory);
            await File.WriteAllBytesAsync(Path.Combine(directory, fileName), bytes);

            return RelativeDirectory + fileName;
        }

        public bool IsStoredEvidence(string? relativePath)
        {
            return TryGetManagedPath(relativePath, out _);
        }

        public Stream OpenReadStream(string relativePath)
        {
            if (!TryGetManagedPath(relativePath, out var filePath))
            {
                throw new InvalidDataException("La ruta de evidencia no es válida.");
            }

            return File.OpenRead(filePath);
        }

        public void Delete(string? relativePath)
        {
            if (!TryGetManagedPath(relativePath, out var filePath))
            {
                return;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string? GetExternalUrl(string? value)
        {
            if (Uri.TryCreate(value, UriKind.Absolute, out var externalUri)
                && (externalUri.Scheme == Uri.UriSchemeHttp || externalUri.Scheme == Uri.UriSchemeHttps))
            {
                return externalUri.ToString();
            }

            return null;
        }

        private bool TryGetManagedPath(string? relativePath, out string filePath)
        {
            filePath = string.Empty;
            if (string.IsNullOrWhiteSpace(relativePath)
                || !relativePath.StartsWith(RelativeDirectory, StringComparison.Ordinal))
            {
                return false;
            }

            var fileName = relativePath.Substring(RelativeDirectory.Length);
            if (fileName.Length != 36
                || !fileName.EndsWith(".pdf", StringComparison.Ordinal)
                || !Guid.TryParseExact(fileName.Substring(0, 32), "N", out _))
            {
                return false;
            }

            filePath = Path.Combine(GetStorageDirectory(), fileName);
            return true;
        }

        private string GetStorageDirectory()
        {
            return Path.Combine(_environment.ContentRootPath, "App_Data", "Evidencias");
        }
    }
}
