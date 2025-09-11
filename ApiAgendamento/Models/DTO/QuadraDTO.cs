namespace ApiAgendamento.Models.DTO
{
    public class QuadraDTO
    {
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public string Descricao { get; set; }
        public bool EstaDisponivel { get; set; }
        public decimal PrecoPorHora { get; set; }
    }
}
