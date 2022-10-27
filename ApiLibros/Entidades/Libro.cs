
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiLibros.Validaciones;

namespace ApiLibros.Entidades
{
    public class Libro : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " El campo {0} es requerido")]
        [StringLength(maximumLength: 25, ErrorMessage = " El campo {0} solo puede tener hasta 30 caracteres")]
        //[PrimeraLetraMayuscula] //Una forma de validar 

        public string Titulo { get; set; }
        public string Autor { get; set; }

        [Range(1, 600, ErrorMessage = " El campo Paginas_Leidas no se encuentra dentro del rango")]
        [NotMapped]

        public int Paginas_Leidas { get; set; }
        public List<Categoria> categorias { get; set; }


        //Otra forma de validar lo de la primera letra en mayuscula
        [NotMapped]
        public int Menor { get; set; }
 
        [NotMapped]
        public int Mayor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
                if (!string.IsNullOrEmpty(Titulo))
            {
                var primerLetra = Titulo[0].ToString();

                if (primerLetra != primerLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new String[] { nameof(Titulo) });
                }
            }

            if (Menor > Mayor)
            {
                yield return new ValidationResult("Este valor no puede ser mas grande que el campo Mayor",
                     new String[] { nameof(Menor) });
            }
        }
    }
}



