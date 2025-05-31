namespace EstoqueNutris.DTOs
{
    public class ImportacaoManagerDto
    {
        public int Id { get; set; }
        public int UsuarioEscolaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string PlanilhaOrigemUrl { get; set; } = string.Empty;
        public string PlanilhaDestinoUrl { get; set; } = string.Empty;
        public string CelulasMapping { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? UltimaExecucao { get; set; }
    }
} 