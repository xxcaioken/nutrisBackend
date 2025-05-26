using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueNutris.Models
{
    public class UsuarioEscola
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        public string EscolaId { get; set; } = string.Empty;

        [Required]
        public string LinkPlanilha { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser? Usuario { get; set; }

        [ForeignKey("EscolaId")]
        public virtual Escola? Escola { get; set; }
    }
} 