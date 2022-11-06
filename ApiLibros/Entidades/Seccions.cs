using Microsoft.AspNetCore.Identity;

namespace ApiLibros.Entidades
{
    public class Seccions
    {
        public int Id { get; set; }
        public string Contenido { get; set; }

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario { get; set; }
    }
}
