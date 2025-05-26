using System.ComponentModel.DataAnnotations;
using EstoqueNutris.Interfaces;

namespace EstoqueNutris.DTOs
{
    public class GoogleAuthDto : IValidatable
    {
        [Required(ErrorMessage = "O token do Google é obrigatório")]
        public string GoogleToken { get; set; } = string.Empty;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(GoogleToken);
        }

        public IEnumerable<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(GoogleToken))
                errors.Add("O token do Google é obrigatório");

            return errors;
        }
    }
} 