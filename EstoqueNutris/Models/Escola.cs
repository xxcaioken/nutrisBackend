using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.Models
{
    public class Escola
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Endereco { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public virtual ICollection<UsuarioEscola> UsuarioEscolas { get; set; } = new List<UsuarioEscola>();
    }
} 