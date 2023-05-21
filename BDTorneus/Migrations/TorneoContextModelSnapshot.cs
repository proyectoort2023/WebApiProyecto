﻿// <auto-generated />
using System;
using BDTorneus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BDTorneus.Migrations
{
    [DbContext(typeof(TorneoContext))]
    partial class TorneoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BDTorneus.Equipo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abreviatura")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Deporte")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Equipos");
                });

            modelBuilder.Entity("BDTorneus.Inscripcion", b =>
                {
                    b.Property<int>("TorneoId")
                        .HasColumnType("int");

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedioPago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Monto")
                        .HasColumnType("float");

                    b.HasKey("TorneoId", "EquipoId");

                    b.HasIndex("EquipoId");

                    b.ToTable("Inscripciones");
                });

            modelBuilder.Entity("BDTorneus.Jugador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Capitan")
                        .HasColumnType("bit");

                    b.Property<string>("Cedula")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("NombreCompleto")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Jugadores");
                });

            modelBuilder.Entity("BDTorneus.Medalla", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("MedallaImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreTorneo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Medallas");
                });

            modelBuilder.Entity("BDTorneus.Notificacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("datetime2");

                    b.Property<bool>("General")
                        .HasColumnType("bit");

                    b.Property<bool>("Leido")
                        .HasColumnType("bit");

                    b.Property<string>("Mensaje")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TorneoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EquipoId");

                    b.HasIndex("TorneoId");

                    b.ToTable("Notificaciones");
                });

            modelBuilder.Entity("BDTorneus.Partido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EquipoLocalId")
                        .HasColumnType("int");

                    b.Property<int>("EquipoVisitanteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Fin")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HistorialSet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Inicio")
                        .HasColumnType("datetime2");

                    b.Property<int>("MarcadorLocal")
                        .HasColumnType("int");

                    b.Property<int>("MarcadorVisitante")
                        .HasColumnType("int");

                    b.Property<string>("NombreCancha")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PartidoSiguienteGuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PuntajeLocal")
                        .HasColumnType("int");

                    b.Property<int>("PuntajeVisitante")
                        .HasColumnType("int");

                    b.Property<int>("Ronda")
                        .HasColumnType("int");

                    b.Property<int>("SetActual")
                        .HasColumnType("int");

                    b.Property<int>("SetGanadosLocal")
                        .HasColumnType("int");

                    b.Property<int>("SetGanadosVisitante")
                        .HasColumnType("int");

                    b.Property<int>("TorneoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TorneoId");

                    b.ToTable("Partidos");
                });

            modelBuilder.Entity("BDTorneus.Torneo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Banner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CantidadCanchas")
                        .HasColumnType("int");

                    b.Property<string>("ConfiguracionEquipos")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HabilitacionInscripcion")
                        .HasColumnType("bit");

                    b.Property<DateTime>("HoraComienzo")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Indoor")
                        .HasColumnType("bit");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lugar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxEquiposInscriptos")
                        .HasColumnType("int");

                    b.Property<int>("MaxJugadoresPorEquipo")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreContacto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Otros")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.Property<int>("PuntajeMax")
                        .HasColumnType("int");

                    b.Property<int>("PuntajeMaxUltimoSet")
                        .HasColumnType("int");

                    b.Property<int>("SetsMax")
                        .HasColumnType("int");

                    b.Property<bool>("Suspendido")
                        .HasColumnType("bit");

                    b.Property<string>("TelContacto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoPrecio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UbicacionGPS")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Torneos");
                });

            modelBuilder.Entity("BDTorneus.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("BDTorneus.Equipo", b =>
                {
                    b.HasOne("BDTorneus.Usuario", "Usuario")
                        .WithMany("Equipos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("BDTorneus.Inscripcion", b =>
                {
                    b.HasOne("BDTorneus.Equipo", null)
                        .WithMany("Inscripciones")
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BDTorneus.Torneo", null)
                        .WithMany("Inscripciones")
                        .HasForeignKey("TorneoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BDTorneus.Notificacion", b =>
                {
                    b.HasOne("BDTorneus.Equipo", "Equipo")
                        .WithMany()
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BDTorneus.Torneo", "Torneo")
                        .WithMany()
                        .HasForeignKey("TorneoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipo");

                    b.Navigation("Torneo");
                });

            modelBuilder.Entity("BDTorneus.Partido", b =>
                {
                    b.HasOne("BDTorneus.Torneo", "Torneo")
                        .WithMany("Fixture")
                        .HasForeignKey("TorneoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Torneo");
                });

            modelBuilder.Entity("BDTorneus.Torneo", b =>
                {
                    b.HasOne("BDTorneus.Usuario", null)
                        .WithMany("Torneos")
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("BDTorneus.Equipo", b =>
                {
                    b.Navigation("Inscripciones");
                });

            modelBuilder.Entity("BDTorneus.Torneo", b =>
                {
                    b.Navigation("Fixture");

                    b.Navigation("Inscripciones");
                });

            modelBuilder.Entity("BDTorneus.Usuario", b =>
                {
                    b.Navigation("Equipos");

                    b.Navigation("Torneos");
                });
#pragma warning restore 612, 618
        }
    }
}
