using Microsoft.EntityFrameworkCore;
using EstoqueNutris.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EstoqueNutris.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<UsuarioEscola> UsuarioEscolas { get; set; }
        public DbSet<Escola> Escolas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<ApplicationUser>()
                .Property(u => u.EscolaId)
                .IsRequired()
                .HasDefaultValue("0");

            // Configuração da tabela Escola
            builder.Entity<Escola>()
                .Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);

            // Configuração da tabela UsuarioEscola
            builder.Entity<UsuarioEscola>()
                .HasOne(ue => ue.Usuario)
                .WithMany()
                .HasForeignKey(ue => ue.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UsuarioEscola>()
                .HasOne(ue => ue.Escola)
                .WithMany(e => e.UsuarioEscolas)
                .HasForeignKey(ue => ue.EscolaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UsuarioEscola>()
                .Property(ue => ue.LinkPlanilha)
                .IsRequired()
                .HasMaxLength(500);

            builder.Entity<UsuarioEscola>()
                .Property(ue => ue.EscolaId)
                .IsRequired()
                .HasMaxLength(50);

            // Índice composto para evitar duplicatas
            builder.Entity<UsuarioEscola>()
                .HasIndex(ue => new { ue.UsuarioId, ue.EscolaId })
                .IsUnique();
        }
    }
}
