
using Microsoft.EntityFrameworkCore;
using EstoqueNutris.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EstoqueNutris.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}
