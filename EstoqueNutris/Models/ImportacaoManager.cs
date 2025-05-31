using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueNutris.Models
{
    public class ImportacaoManager
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
        public string PlanilhaOrigemUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string PlanilhaDestinoUrl { get; set; } = string.Empty;

        [Required]
        public string CelulasMapping { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }

        public bool IsAtivo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }

        public DateTime? UltimaExecucao { get; set; }

        [ForeignKey("UsuarioEscolaId")]
        public virtual UsuarioEscola? UsuarioEscola { get; set; }
    }
} 