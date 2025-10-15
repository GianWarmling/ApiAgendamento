using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Implementations;
using ApiAgendamento.Repositories.Inferfaces;
using ApiAgendamentoTeste.Mock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiAgendamentoTeste.Repositories
{
    public class PagamentoRepositoryTeste
    {
        private readonly IRepository<Pagamento> _pagamentoRepository;
        private readonly AppDbContext _context;

        public PagamentoRepositoryTeste()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // cria DB em memória único por teste
                .Options;

            _context = new AppDbContext(options);
            _pagamentoRepository = new Repository<Pagamento>(_context);
        }

        [Fact]
        public async Task DeveAdicionarPagamento()
        {
            // Arrange
            await PagamentoMockData.CriarPagamento(_pagamentoRepository);

            // Act
            var registros = await _pagamentoRepository.GetAllAsync();

            // Assert
            registros.Should().ContainSingle();
            registros.First().MetodoPagamento.Should().Be("Pix");
            registros.First().Valor.Should().Be(100);
        }

        [Fact]
        public async Task GetById_DeveRetornarPagamentoPorId()
        {
            // Arrange
            await PagamentoMockData.CriarPagamento(_pagamentoRepository, 10);

            // Act
            var registro = await _pagamentoRepository.GetByIdAsync(10);

            // Assert
            registro.Should().NotBeNull();
            registro.Id.Should().Be(10);
            registro.MetodoPagamento.Should().Be("Pix");
        }

        [Fact]
        public async Task DeveRemoverPagamento()
        {
            // Arrange
            var pagamento = await PagamentoMockData.CriarPagamento(_pagamentoRepository);

            // Verifica se foi criado corretamente
            var registro = await _pagamentoRepository.GetByIdAsync(1);
            registro.Should().NotBeNull();
            registro.MetodoPagamento.Should().Be("Pix");

            // Act
            await _pagamentoRepository.DeleteAsync(pagamento);
            await _pagamentoRepository.SaveChanges();

            // Assert
            var registros = await _pagamentoRepository.GetAllAsync();
            registros.Should().BeEmpty();
        }

        [Fact]
        public async Task DeveAtualizarStatusDoPagamento()
        {
            // Arrange
            var pagamento = await PagamentoMockData.CriarPagamento(_pagamentoRepository);
            pagamento.Status = StatusPagamento.PENDENTE;

            // Act
            pagamento.Status = StatusPagamento.APROVADO;
            await _pagamentoRepository.UpdateAsync(pagamento);
            await _pagamentoRepository.SaveChanges();

            // Assert
            var atualizado = await _pagamentoRepository.GetByIdAsync(pagamento.Id);
            atualizado.Status.Should().Be(StatusPagamento.APROVADO);
        }
    }
}
