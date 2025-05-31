using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.DTOs
{
    public class UpdateLinkDto
    {
        [StringLength(100)]
        public string? Nome { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "A URL fornecida não é válida.")]
        public string? Url { get; set; }

        [StringLength(255)]
        public string? Descricao { get; set; }
    }
} 