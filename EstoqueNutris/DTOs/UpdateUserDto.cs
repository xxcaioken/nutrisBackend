using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ID da escola é obrigatório")]
        public string EscolaId { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }
    }
} 