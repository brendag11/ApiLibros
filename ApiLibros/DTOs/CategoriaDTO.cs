namespace ApiLibros.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<SeccionDTO> Seccions { get; set; }
    }
}
