using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAgendamento.Models
{
    public class Quadra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public string Descricao { get; set; }
        public bool EstaDisponivel { get; set; }
        public decimal PrecoPorHora { get; set; }
    }
}
