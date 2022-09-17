namespace ApiLibros.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Genero { get; set; }
        public int Año_Publicacion { get; set; }

        public int LibroId { get; set; }
        public Libro Libro { get; set; }
    }
}
