using System;

namespace EstoqueNutris.DTOs
{
    public class LinkDto
    {
        public int Id { get; set; }
        public int UsuarioEscolaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
} 