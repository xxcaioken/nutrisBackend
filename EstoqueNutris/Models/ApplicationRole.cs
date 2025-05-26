using Microsoft.AspNetCore.Identity;

namespace EstoqueNutris.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
} 