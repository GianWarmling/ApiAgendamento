using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgendamentoTeste.Mock
{
    public static class AgendamentoMockData
    {
        public static async Task<Agendamento> CriarAgendamento(IAgendamentoRepository repository, int id = 0)
        {
            var agendamento = new Agendamento()
            {
                Id = id,
                QuadraId = 1,
                UsuarioId = 1,
                DataHoraInicio = DateTime.Now,
                DataHoraFim = DateTime.Now.AddHours(1),
                ValorTotal = 100
            };

            await repository.AddAsync(agendamento);
            await repository.SaveChanges();

            return agendamento;
        }
    }
}
