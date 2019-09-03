using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CE.API.Entities
{
    public partial class CEDatabaseContext : DbContext
    {
        public CEDatabaseContext()
        {
        }

        public CEDatabaseContext(DbContextOptions<CEDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Calificacion> Calificacion { get; set; }
        public virtual DbSet<Docente> Docente { get; set; }
        public virtual DbSet<DocentePuestos> DocentePuestos { get; set; }
        public virtual DbSet<Estudiante> Estudiante { get; set; }
        public virtual DbSet<EstudianteGrupos> EstudianteGrupos { get; set; }
        public virtual DbSet<EvaluacionDocente> EvaluacionDocente { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<Puesto> Puesto { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolesUsuario> RolesUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calificacion>(entity =>
            {
                entity.HasKey(e => new { e.EstudianteId, e.GrupoId });

                entity.ToTable("Calificacion", "control_escolar");

                entity.Property(e => e.Calificacion1)
                    .HasColumnName("Calificacion")
                    .HasColumnType("decimal(3, 1)");

                entity.Property(e => e.Comentarios).HasMaxLength(100);

                entity.HasOne(d => d.Estudiante)
                    .WithMany(p => p.Calificacion)
                    .HasForeignKey(d => d.EstudianteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calificacion_Estudiante");

                entity.HasOne(d => d.Grupo)
                    .WithMany(p => p.Calificacion)
                    .HasForeignKey(d => d.GrupoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calificacion_Grupo");
            });

            modelBuilder.Entity<Docente>(entity =>
            {
                entity.HasKey(e => e.UsuarioId);

                entity.ToTable("Docente", "control_escolar");

                entity.Property(e => e.UsuarioId).ValueGeneratedNever();

                entity.Property(e => e.Evaluacion).HasColumnType("decimal(2, 1)");

                entity.HasOne(d => d.Usuario)
                    .WithOne(p => p.Docente)
                    .HasForeignKey<Docente>(d => d.UsuarioId)
                    .HasConstraintName("FK_Docente_Usuario");
            });

            modelBuilder.Entity<DocentePuestos>(entity =>
            {
                entity.HasKey(e => new { e.DocenteId, e.PuestoId });

                entity.ToTable("DocentePuestos", "control_escolar");

                entity.HasOne(d => d.Docente)
                    .WithMany(p => p.DocentePuestos)
                    .HasForeignKey(d => d.DocenteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocentePuestos_Docente");

                entity.HasOne(d => d.Puesto)
                    .WithMany(p => p.DocentePuestos)
                    .HasForeignKey(d => d.PuestoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocentePuestos_Puesto");
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.HasKey(e => e.UsuarioId);

                entity.ToTable("Estudiante", "control_escolar");

                entity.Property(e => e.UsuarioId).ValueGeneratedNever();

                entity.Property(e => e.Estatus).HasMaxLength(50);

                entity.HasOne(d => d.Usuario)
                    .WithOne(p => p.Estudiante)
                    .HasForeignKey<Estudiante>(d => d.UsuarioId)
                    .HasConstraintName("FK_Estudiante_Usuario");
            });

            modelBuilder.Entity<EstudianteGrupos>(entity =>
            {
                entity.HasKey(e => new { e.EstudianteId, e.GrupoId });

                entity.ToTable("EstudianteGrupos", "control_escolar");

                entity.HasOne(d => d.Estudiante)
                    .WithMany(p => p.EstudianteGrupos)
                    .HasForeignKey(d => d.EstudianteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstudianteGrupos_Estudiante");

                entity.HasOne(d => d.Grupo)
                    .WithMany(p => p.EstudianteGrupos)
                    .HasForeignKey(d => d.GrupoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstudianteGrupos_Grupo");
            });

            modelBuilder.Entity<EvaluacionDocente>(entity =>
            {
                entity.ToTable("EvaluacionDocente", "control_escolar");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PreguntaEvaluacion)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Puntaje).HasColumnType("decimal(3, 1)");

                entity.HasOne(d => d.Docente)
                    .WithMany(p => p.EvaluacionDocente)
                    .HasForeignKey(d => d.DocenteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EvaluacionDocente_Docente");
            });

            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.ToTable("Grupo", "control_escolar");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NivelGrupo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TurnoCurso).HasMaxLength(50);

                entity.HasOne(d => d.Docente)
                    .WithMany(p => p.Grupo)
                    .HasForeignKey(d => d.DocenteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grupo_Docente");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post", "control_escolar");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Mensaje)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Grupo)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.GrupoId)
                    .HasConstraintName("FK_Post_Grupo");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_Usuario");
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.ToTable("Pregunta", "control_escolar");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Texto)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Puesto>(entity =>
            {
                entity.ToTable("Puesto", "control_escolar");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NombrePuesto)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Salario).HasColumnType("money");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "control_escolar");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RolesUsuario>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.UsuarioId });

                entity.ToTable("RolesUsuario", "control_escolar");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolesUsuario)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RolesUsuario_Role");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.RolesUsuario)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RolesUsuario_Usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario", "control_escolar");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApellidoMaterno)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CodigoPostal).HasMaxLength(5);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TelefonoCasa).HasMaxLength(11);

                entity.Property(e => e.TelefonoCelular)
                    .IsRequired()
                    .HasMaxLength(11);
            });
        }
    }
}
