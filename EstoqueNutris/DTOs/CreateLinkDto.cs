using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.DTOs
{
    public class CreateLinkDto
    {
        [Required]
        public int UsuarioEscolaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "A URL fornecida não é válida.")]
        public string Url { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descricao { get; set; }
    }
} 