using System.ComponentModel.DataAnnotations;

namespace ApiLibros.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
