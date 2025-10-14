using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Implementations;
using ApiAgendamentoTeste.Mock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgendamentoTeste.Repositories
{
    public class AgendamentoRepositoryTeste
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly AppDbContext _context;

        public AgendamentoRepositoryTeste()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(options);
            _agendamentoRepository = new AgendamentoRepository(_context);
        }
        [Fact]
        public async Task DeveAdicionarAgendamento()
        {
            await AgendamentoMockData.CriarAgendamento(_agendamentoRepository);

            //assert
            var registros = await _agendamentoRepository.GetAllAsync();
            registros.Should().ContainSingle();
            registros.First().QuadraId.Should().Be(1);

        }
        [Fact]
        public async Task GetById_DeveRetornarPorId()
        {
            await AgendamentoMockData.CriarAgendamento(_agendamentoRepository, 10);

            //assert
            var registro = await _agendamentoRepository.GetByIdAsync(10);
            registro.QuadraId.Should().Be(1);
        }
        [Fact]
        public async Task DeveRemoverAgendamento()
        {
            var agendamento = await AgendamentoMockData.CriarAgendamento(_agendamentoRepository);

            //assert
            var registro = await _agendamentoRepository.GetByIdAsync(1);
            registro.Should().NotBeNull();
            registro.QuadraId.Should().Be(1);

            await _agendamentoRepository.DeleteAsync(agendamento);
            await _agendamentoRepository.SaveChanges();

            //assert
            var registros = await _agendamentoRepository.GetAllAsync();
            registros.Should().BeEmpty();

        }
    }
}
