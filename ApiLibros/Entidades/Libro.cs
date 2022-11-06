
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiLibros.Validaciones;

namespace ApiLibros.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

    [Required(ErrorMessage = " El campo {0} es requerido")]
    [StringLength(maximumLength: 150, ErrorMessage = " El campo {0} solo puede tener hasta 150 caracteres")]
    [PrimeraLetraMayuscula] //Una forma de validar 

    public string Titulo { get; set; }
    public string Autor { get; set; }

    public List<LibroCategoria> LibroCategoria { get; set; }
}
}