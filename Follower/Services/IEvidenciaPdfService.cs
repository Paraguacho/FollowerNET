using Follower.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Follower.Services;

public interface IEvidenciaPdfService
{
    Task<IReadOnlyList<EvidenciaPdf>> ListarAsync(int idEgresado);
    Task<EvidenciaPdf> GuardarAsync(int idEgresado, IBrowserFile archivo);
    Task<byte[]> LeerAsync(int idEgresado, Guid idArchivo);
}
