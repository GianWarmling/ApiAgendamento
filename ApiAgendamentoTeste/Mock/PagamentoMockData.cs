using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Inferfaces;
using System;
using System.Threading.Tasks;

namespace ApiAgendamentoTeste.Mock
{
    public static class PagamentoMockData
    {
        public static async Task<Pagamento> CriarPagamento(IRepository<Pagamento> repository, int id = 0)
        {
            var pagamento = new Pagamento()
            {
                Id = id,
                AgendamentoId = 1,
                MetodoPagamento = "Pix",
                Valor = 100,
                Status = StatusPagamento.PENDENTE,
                DataPagamento = DateTime.Now
            };

            await repository.AddAsync(pagamento);
            await repository.SaveChanges();

            return pagamento;
        }
    }
}
