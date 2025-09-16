using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAgendamento.Models
{
    public class Agendamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public decimal ValorTotal { get; set; }
        public StatusAgendamento Status { get; set; } // Ex: "Confirmado", "Cancelado", "Pendente"

        // Relacionamentos com outras classes
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Propriedade de navegação para o usuário que fez o agendamento

        public int QuadraId { get; set; }
        public Quadra Quadra { get; set; } // Propriedade de navegação para a quadra agendada
    }
    public enum StatusAgendamento
    {
        PENDENTE,
        APROVADO,
        RECUSADO,
        CANCELADO
    }
}
