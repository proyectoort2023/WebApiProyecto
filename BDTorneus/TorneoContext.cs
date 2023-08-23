using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDTorneus
{
    public class TorneoContext : DbContext
    {
        public TorneoContext(DbContextOptions options) : base(options)
        {
            // this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inscripcion>().Property(prop => prop.Id).ValueGeneratedOnAdd();

           modelBuilder.Entity<Inscripcion>().HasKey(prop => new {prop.Id, prop.TorneoId, prop.EquipoId, prop.UsuarioId });
        }

        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Medalla> Medallas { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Torneo> Torneos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AutorizacionPlanillero> AutorizacionesPlanilleros { get; set; }
    }
}
