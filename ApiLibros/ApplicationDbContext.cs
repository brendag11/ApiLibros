using ApiLibros.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LibroCategoria>()
                .HasKey(lib => new { lib.LibroId, lib.CategoriaId });
        }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Seccions> Seccions { get; set; }

        public DbSet<LibroCategoria> LibroCategoria { get; set; }
    }

}

