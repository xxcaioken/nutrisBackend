using Microsoft.AspNetCore.Identity;

namespace EstoqueNutris.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; } = string.Empty;
        public string EscolaId { get; set; } = "0";
        public bool IsAdmin { get; set; } = false;
    }
}
