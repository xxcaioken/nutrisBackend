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
        public DbSet<Link> Links { get; set; }
        public DbSet<ImportacaoManager> ImportacaoManagers { get; set; }

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

            builder.Entity<Escola>()
                .Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);

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

            builder.Entity<UsuarioEscola>()
                .HasIndex(ue => new { ue.UsuarioId, ue.EscolaId })
                .IsUnique();

            builder.Entity<Link>()
                .HasOne(l => l.UsuarioEscola)
                .WithMany(ue => ue.Links)
                .HasForeignKey(l => l.UsuarioEscolaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Link>()
                .Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<Link>()
                .Property(l => l.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Entity<Link>()
                .Property(l => l.Descricao)
                .HasMaxLength(255);

            builder.Entity<ImportacaoManager>()
                .HasOne(im => im.UsuarioEscola)
                .WithMany()
                .HasForeignKey(im => im.UsuarioEscolaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ImportacaoManager>()
                .Property(im => im.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Entity<ImportacaoManager>()
                .Property(im => im.PlanilhaOrigemUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Entity<ImportacaoManager>()
                .Property(im => im.PlanilhaDestinoUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Entity<ImportacaoManager>()
                .Property(im => im.CelulasMapping)
                .IsRequired();

            builder.Entity<ImportacaoManager>()
                .Property(im => im.Descricao)
                .HasMaxLength(255);

            builder.Entity<ImportacaoManager>()
                .Property(im => im.DataCriacao)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            builder.Entity<ImportacaoManager>()
                .Property(im => im.DataAtualizacao)
                .IsRequired(false);

            builder.Entity<ImportacaoManager>()
                .Property(im => im.UltimaExecucao)
                .IsRequired(false);
        }
    }
}
