using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLibros.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required(ErrorMessage = " El campo {0} es requerido")]
        [StringLength(maximumLength: 25, ErrorMessage = " El campo {0} solo puede tener hasta 30 caracteres")]

        public string Titulo { get; set; }
        public string Autor { get; set; }

        [Range(1, 600, ErrorMessage = " El campo Paginas_Leidas no se encuentra dentro del rango")]
        [NotMapped]

        public int Paginas_Leidas { get; set; }
        public List<Categoria> categorias { get; set; }


    }
}


