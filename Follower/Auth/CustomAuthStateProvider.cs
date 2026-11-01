using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Follower.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider (ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task <AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _sessionStorage.GetAsync<string>("userId");
                var userRole = await _sessionStorage.GetAsync<string>("userRole");

                if (userSession.Success && !string.IsNullOrEmpty(userSession.Value))
                {
                    var claims = new List<Claim>
                    {
                        new Claim (ClaimTypes.NameIdentifier,userSession.Value),
                        new Claim(ClaimTypes.Role, userRole.Success && userRole.Value != null ? userRole.Value : "")
                    };

                    var identity = new ClaimsIdentity(claims, "BlazorAuth");
                    var principal = new ClaimsPrincipal(identity);
                    return new AuthenticationState(principal);
                }


            }
            catch
            {

            }
            //si trunea retornar usuario anon
            return new AuthenticationState(_anonymous);
        }

        public async Task IniciarSesion(string idUsuario, string rol)
        {
            await _sessionStorage.SetAsync("userId", idUsuario);
            await _sessionStorage.SetAsync("userRole", rol);

            var claims = new List<Claim>
            {
                new Claim ( ClaimTypes.NameIdentifier, idUsuario),
                new Claim (ClaimTypes.Role,rol)
            };
            var identity = new ClaimsIdentity(claims, "BlazorAuth");
            var principal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task CerrarSesion()
        {
            await _sessionStorage.DeleteAsync("userId");
            await _sessionStorage.DeleteAsync("userRole");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
