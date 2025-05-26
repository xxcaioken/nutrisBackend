using System.ComponentModel.DataAnnotations;
using EstoqueNutris.Interfaces;

namespace EstoqueNutris.DTOs
{
    public class LoginDto : IValidatable
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        public string? GoogleToken { get; set; } = null;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Email) && 
                   !string.IsNullOrEmpty(Password) && 
                   Password.Length >= 6 &&
                   new EmailAddressAttribute().IsValid(Email);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Email))
                errors.Add("O email é obrigatório");
            else if (!new EmailAddressAttribute().IsValid(Email))
                errors.Add("Email inválido");

            if (string.IsNullOrEmpty(Password))
                errors.Add("A senha é obrigatória");
            else if (Password.Length < 6)
                errors.Add("A senha deve ter no mínimo 6 caracteres");

            return errors;
        }
    }
}
