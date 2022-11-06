namespace ApiLibros.DTOs
{
    public class LibroDTOConCategorias : GetLibroDTO
    {
        public List<CategoriaDTO> Categorias { get; set; }
    }
}
