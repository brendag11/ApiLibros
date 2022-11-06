namespace ApiLibros.DTOs
{
    public class CategoriaDTOConLibros: CategoriaDTO
    {
        public List<GetLibroDTO> Libros { get; set; }
    }
}
