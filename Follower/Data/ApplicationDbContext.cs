using Follower.Models;
using Microsoft.EntityFrameworkCore;

namespace Follower.Data
{
    public class ApplicationDbContext : DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } 
        public DbSet<ProgramaEducativo> ProgramasEducativos { get; set; }
        public DbSet<ProgramaResponsable> ProgramasResponsables { get; set;  }
        public DbSet<Egresado> Egresados { get; set; }
        public DbSet<SeccionPersonalizada> SeccionesPersonalizadas { get; set; }
        public DbSet<CampoPersonalizado> CamposPersonalizados { get; set; }
        public DbSet<OpcionCatalogoCampo> OpcionesCatalogoCampos { get; set; }
        public DbSet<ValorCampoEgresado> ValoresCamposEgresados { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Correo unico 
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Egresado)
                .WithOne(e => e.Usuario)
                .HasForeignKey<Egresado>(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Egresado>()
                .HasOne(e => e.UsuarioResolvio)
                .WithMany()
                .HasForeignKey(e => e.IdUsuarioResolvio)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProgramaResponsable>()
                .HasIndex(pr => new { pr.IdPrograma, pr.IdUsuario })
                .IsUnique();

            modelBuilder.Entity<ProgramaResponsable>()
                .HasOne(pr => pr.ProgramaEducativo)
                .WithMany(p => p.Responsables)
                .HasForeignKey(pr => pr.IdPrograma)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProgramaResponsable>()
                .HasOne(pr => pr.Usuario)
                .WithMany(u => u.ProgramasAsignados)
                .HasForeignKey(pr => pr.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Epica 8 

            modelBuilder.Entity<SeccionPersonalizada>()
                .HasOne(s => s.ProgramaEducativo)
                .WithMany(p => p.SeccionPersonalizadas)
                .HasForeignKey(s => s.IdPrograma)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampoPersonalizado>()
                .HasOne(c => c.SeccionPersonalizada)
                .WithMany(s => s.Campos)
                .HasForeignKey(c => c.IdSeccion)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampoPersonalizado>()
                .Property(c => c.TipoDato)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<OpcionCatalogoCampo>()
                .HasOne(o => o.CampoPersonalizado)
                .WithMany(c => c.Opciones)
                .HasForeignKey(o => o.IdCampo)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValorCampoEgresado>()
                .HasOne(v => v.Egresado)
                .WithMany(e => e.ValoresCamposPersonalizados)
                .HasForeignKey(v => v.IdEgresado)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValorCampoEgresado>()
                .HasOne(v => v.CampoPersonalizado)
                .WithMany(c => c.ValoresEgresados)
                .HasForeignKey(v => v.IdCampo)
                .OnDelete(DeleteBehavior.Cascade);
        
        }
   

    
            }
}
