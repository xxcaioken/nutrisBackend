using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.DTOs
{
    public class CreateImportacaoManagerDto
    {
        [Required]
        public int UsuarioEscolaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "A URL da planilha de origem não é válida.")]
        public string PlanilhaOrigemUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "A URL da planilha de destino não é válida.")]
        public string PlanilhaDestinoUrl { get; set; } = string.Empty;

        [Required]
        public string CelulasMapping { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }

        public bool IsAtivo { get; set; } = true;
    }
} 