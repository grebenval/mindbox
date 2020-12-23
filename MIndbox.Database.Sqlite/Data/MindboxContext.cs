using Microsoft.EntityFrameworkCore;
using Mindbox.Bl;

#nullable disable

namespace Mindbox.Database.Sqlite.Data
{
    public partial class MindboxContext : DbContext
    {
        /// <summary>
        /// Строка соединения
        /// </summary>
        protected readonly string _connectionString;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionString">Строка соединения</param>
        public MindboxContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MindboxContext(DbContextOptions<MindboxContext> options, IDatabaseConnect databaseConnect)
            : base(options)
        {
            _connectionString = databaseConnect.GetConnectionString();
        }

        public virtual DbSet<Circle> Circles { get; set; }
        public virtual DbSet<Figure> Figures { get; set; }
        public virtual DbSet<Triangle> Triangles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Circle>(entity =>
            {
                entity.HasKey(e => e.Idcircle);

                entity.ToTable("circles");

                entity.Property(e => e.Idcircle)
                    .ValueGeneratedNever()
                    .HasColumnName("idcircle");

                entity.Property(e => e.Radius)
                    .HasColumnType("DOUBLE")
                    .HasColumnName("radius");

                entity.HasOne(d => d.IdcircleNavigation)
                    .WithOne(p => p.Circle)
                    .HasForeignKey<Circle>(d => d.Idcircle)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Figure>(entity =>
            {
                entity.ToTable("figures");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Area)
                    .HasColumnType("DOUBLE")
                    .HasColumnName("area");

                entity.Property(e => e.Type)
                    .HasColumnType("INT")
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Triangle>(entity =>
            {
                entity.ToTable("triangle");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.A)
                    .HasColumnType("DOUBLE")
                    .HasColumnName("a");

                entity.Property(e => e.B)
                    .HasColumnType("DOUBLE")
                    .HasColumnName("b");

                entity.Property(e => e.C)
                    .HasColumnType("DOUBLE")
                    .HasColumnName("c");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Triangle)
                    .HasForeignKey<Triangle>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
