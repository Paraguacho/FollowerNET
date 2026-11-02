using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.ComponentModel.DataAnnotations;
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
                var timestamp = await _sessionStorage.GetAsync<string>("loginTimestamp");

                if (userSession.Success && !string.IsNullOrEmpty(userSession.Value) && userRole.Success && !string.IsNullOrEmpty(userRole.Value)
                    && timestamp.Success && !string.IsNullOrEmpty(timestamp.Value))
                {
                    if (DateTime.TryParse(timestamp.Value, out DateTime fechaLogin))
                    {
                        if ((DateTime.Now - fechaLogin).TotalHours > 12)
                        {
                            await CerrarSesion();
                            return new AuthenticationState(_anonymous);
                        }
                    }
                    var claims = new List<Claim>
                    {
                        new Claim (ClaimTypes.NameIdentifier,userSession.Value),
                        new Claim(ClaimTypes.Role, userRole.Value)

                    };
                    var identity = new ClaimsIdentity(claims, "BlazorAuth");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
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
            await _sessionStorage.SetAsync("loginTimestamp", DateTime.Now.ToString("O"));

            var claims = new List<Claim>
            {
                new Claim ( ClaimTypes.NameIdentifier, idUsuario),
                new Claim (ClaimTypes.Role,rol)
            };
            var identity = new ClaimsIdentity(claims, "BlazorAuth");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task CerrarSesion()
        {
            await _sessionStorage.DeleteAsync("userId");
            await _sessionStorage.DeleteAsync("userRole");
            await _sessionStorage.DeleteAsync("loginTimestamp");
            //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
