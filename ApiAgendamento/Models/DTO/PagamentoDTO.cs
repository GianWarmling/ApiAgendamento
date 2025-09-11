namespace ApiAgendamento.Models.DTO
{
    public class PagamentoDTO
    {
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string MetodoPagamento { get; set; } 
        public string Status { get; set; }
        public int AgendamentoId { get; set; }
    }
}
