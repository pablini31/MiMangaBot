using Microsoft.EntityFrameworkCore;
using MiMangaBot.Domain.Entities;

namespace MiMangaBot.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Manga> Mangas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Manga>()
                .Property(m => m.Titulo)
                .IsRequired();

            modelBuilder.Entity<Manga>()
                .Property(m => m.Calificacion)
                .HasPrecision(3, 2);
        }
    }
} 