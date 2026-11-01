using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Follower.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localstorage;

        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider (ProtectedLocalStorage localStorage)
        {
            _localstorage = localStorage;
        }

        public override async Task <AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _localstorage.GetAsync<string>("userId");
                var userRole = await _localstorage.GetAsync<string>("userRole");

                if (userSession.Success && !string.IsNullOrEmpty(userSession.Value) &&  userRole.Success && !string.IsNullOrEmpty(userRole.Value))
                {
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
            await _localstorage.SetAsync("userId", idUsuario);
            await _localstorage.SetAsync("userRole", rol);

            var claims = new List<Claim>
            {
                new Claim ( ClaimTypes.NameIdentifier, idUsuario),
                new Claim (ClaimTypes.Role,rol)
            };
            var identity = new ClaimsIdentity(claims, "BlazorAuth");
            var principal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task CerrarSesion()
        {
            await _localstorage.DeleteAsync("userId");
            await _localstorage.DeleteAsync("userRole");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
