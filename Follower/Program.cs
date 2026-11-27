using Follower.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Follower.Auth;
using Microsoft.AspNetCore.Identity;
using Follower.Services;
using Follower.Pages.Responsable;

namespace Follower
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

       

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
            builder.Services.AddScoped<IEgresadoService, EgresadoService>();
            builder.Services.AddScoped<IRegistroEgresadoService, RegistroEgresadoService>();
            builder.Services.AddScoped<ISolicitudesIngresoService, SolicitudesIngresoService>();
            builder.Services.AddScoped<IEgresadosBusquedaService, EgresadosBusquedaService>();
            builder.Services.AddScoped<PdfEvidenceStorage>();

            var app = builder.Build();

            // 
            var pathBase = builder.Configuration["PathBase"];
            if (!string.IsNullOrWhiteSpace(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapBlazorHub();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapFallbackToController("Blazor", "Home");

            app.Run();
        }
    }
}