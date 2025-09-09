using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAgendamento.Models.DTO
{
    public class AgendamentoDto
    {
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } // Ex: "Confirmado", "Cancelado", "Pendente"

        // Relacionamentos com outras classes
        public int UsuarioId { get; set; }

        public int QuadraId { get; set; }
    }
}
