using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueNutris.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioEscolaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Url { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }

        [ForeignKey("UsuarioEscolaId")]
        public virtual UsuarioEscola? UsuarioEscola { get; set; }
    }
} 