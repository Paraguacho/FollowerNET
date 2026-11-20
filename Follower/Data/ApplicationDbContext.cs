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
        public DbSet<PerfilAcademico> PerfilesAcademicos { get; set; }

        public DbSet<SituacionLaboral>SituacionesLaborales { get; set; }
        public DbSet<ExperienciaLaboral> ExperienciasLaborales { get; set; }
        public DbSet<FormacionAcademica> FormacionesAcademicas { get; set; }
        
        public DbSet<ResumenProductividad> ResumenesProductividad { get; set; }
        public DbSet<ProductividadCientifica> ProductividadesCientificas { get; set; }
        public DbSet <CoautorProducto> CoautoresProductos { get; set; }
        public DbSet<ProyectoInvestigacion> ProyectosInvestigacion { get; set;  }
        public DbSet<ReconocimientoAcademico> ReconocimientosAcademicos { get; set; }
        public DbSet<ContribucionSocial> ContribucionesSociales { get; set; }

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

        public override int SaveChanges()
        {
            ActualizarFechaModificacionEgresados();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await ActualizarFechaModificacionEgresadosAsync(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ActualizarFechaModificacionEgresados()
        {
            var ahora = DateTime.UtcNow;
            var idsPendientes = RecolectarIdsEgresadosModificados(ahora);

            if (idsPendientes.Count == 0) return;

            var egresados = Egresados.Where(e => idsPendientes.Contains(e.IdEgresado)).ToList();
            foreach (var egresado in egresados)
            {
                egresado.FechaUltimaActualizacion = ahora;
            }
        }

        private async Task ActualizarFechaModificacionEgresadosAsync(CancellationToken cancellationToken)
        {
            var ahora = DateTime.UtcNow;
            var idsPendientes = RecolectarIdsEgresadosModificados(ahora);

            if (idsPendientes.Count == 0) return;

            var egresados = await Egresados
                .Where(e => idsPendientes.Contains(e.IdEgresado))
                .ToListAsync(cancellationToken);

            foreach (var egresado in egresados)
            {
                egresado.FechaUltimaActualizacion = ahora;
            }
        }

        private HashSet<int> RecolectarIdsEgresadosModificados(DateTime ahora)
        {
            var idsAfectados = new HashSet<int>();
            var entradas = ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .ToList();

            foreach (var entry in entradas)
            {
                // 1. Si se modificó directamente el Egresado
                if (entry.Entity is Egresado egresado && entry.State != EntityState.Deleted)
                {
                    egresado.FechaUltimaActualizacion = ahora;
                    continue;
                }

                // 2. Si se agregó, editó o eliminó cualquier entidad hija relacionada
                int? idEgresado = entry.Entity switch
                {
                    PerfilAcademico x => x.EgresadoId,
                    SituacionLaboral x => x.EgresadoId,
                    ExperienciaLaboral x => x.EgresadoId,
                    FormacionAcademica x => x.EgresadoId,
                    ProductividadCientifica x => x.EgresadoId,
                    ProyectoInvestigacion x => x.EgresadoId,
                    ReconocimientoAcademico x => x.EgresadoId,
                    ContribucionSocial x => x.EgresadoId,
                    ResumenProductividad x => x.EgresadoId,
                    ValorCampoEgresado x => x.IdEgresado,
                    _ => null
                };

                if (idEgresado is > 0)
                {
                    idsAfectados.Add(idEgresado.Value);
                }
            }

            // Si algún Egresado ya está siendo rastreado en memoria, actualízalo directo sin consultar la BD
            foreach (var entry in ChangeTracker.Entries<Egresado>())
            {
                if (idsAfectados.Remove(entry.Entity.IdEgresado) && entry.State != EntityState.Deleted)
                {
                    entry.Entity.FechaUltimaActualizacion = ahora;
                }
            }

            return idsAfectados;
        }



    }
}
