namespace ApiAgendamento.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string MetodoPagamento { get; set; } // Ex: "Cartão de Crédito", "Pix"
        public StatusPagamento Status { get; set; } // Ex: "Aprovado", "Recusado"

        // Relacionamento com o Agendamento
        public int AgendamentoId { get; set; }
        public Agendamento Agendamento { get; set; }

    }
    public enum StatusPagamento 
    {
        PENDENTE,
        APROVADO,
        RECUSADO,
        ESTORNADO
    }

}
