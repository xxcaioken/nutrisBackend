using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string EscolaId { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
} 