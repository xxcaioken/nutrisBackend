using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "A nova senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string NewPassword { get; set; } = string.Empty;
    }
} 