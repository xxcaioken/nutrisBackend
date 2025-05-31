using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.DTOs
{
    public class UpdateImportacaoManagerDto
    {
        [StringLength(100)]
        public string? Nome { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "A URL da planilha de origem não é válida.")]
        public string? PlanilhaOrigemUrl { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "A URL da planilha de destino não é válida.")]
        public string? PlanilhaDestinoUrl { get; set; }

        public string? CelulasMapping { get; set; }

        [StringLength(255)]
        public string? Descricao { get; set; }

        public bool? IsAtivo { get; set; }
    }
} 