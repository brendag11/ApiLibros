using ApiLibros.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace ApiLibros.DTOs
{
    public class LibroDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public string Autor { get; set; }
    }
}
