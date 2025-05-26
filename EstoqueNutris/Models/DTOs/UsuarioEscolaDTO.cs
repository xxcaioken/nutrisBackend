using System.ComponentModel.DataAnnotations;

namespace EstoqueNutris.Models.DTOs
{
    public class UsuarioEscolaDTO
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public string EscolaId { get; set; } = string.Empty;
        public string LinkPlanilha { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public EscolaDTO Escola { get; set; } = null!;
    }

    public class EscolaDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime DataCriacao { get; set; }
    }
} 