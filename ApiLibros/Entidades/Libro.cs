namespace ApiLibros.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }

        public List<Categoria> categorias { get; set; }


    }
}
