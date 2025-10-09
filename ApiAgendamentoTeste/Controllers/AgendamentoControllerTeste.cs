using ApiAgendamento.Controllers;
using ApiAgendamento.Models;
using ApiAgendamento.Models.DTO;
using ApiAgendamento.Repositories.Implementations;
using ApiAgendamento.Repositories.Inferfaces;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgendamentoTeste.Controllers
{
    public class AgendamentoControllerTeste
    {
        private readonly Mock<IAgendamentoRepository> _agendamentoRepoMock;
        private readonly Mock<IRepository<Pagamento>> _pagamentoRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AgendamentoController _controller;

        public AgendamentoControllerTeste()
        {
            _agendamentoRepoMock = new Mock<IAgendamentoRepository>();
            _pagamentoRepoMock = new Mock<IRepository<Pagamento>>();
            _mapperMock = new Mock<IMapper>();
            _controller = new AgendamentoController(_agendamentoRepoMock.Object, _mapperMock.Object, _pagamentoRepoMock.Object);
        }
        [Fact]
        public async Task Post_DeveRetornarBadRequest_QuandoHouverConflito()
        {
            //arrange
            var dto = new AgendamentoDto()
            {
                QuadraId = 1,
                UsuarioId = 1,
                DataHoraInicio = new DateTime(2025, 10, 8, 21, 08, 0),
                DataHoraFim = new DateTime(2025, 10, 8, 22, 08, 0),
                MetodoPagamento = "Pix"
            };
            _agendamentoRepoMock.Setup(r => r.ExisteConflito(dto)).ReturnsAsync(true);

            //act
            var result = await _controller.Post(dto);

            //assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Já existe um agendamento nesse horário para essa quadra!");

        }
        [Fact]
        public async Task Post_DeveCriarAgendamento_QuandoNaoHouverConflito()
        {
            //arrange
            var dto = new AgendamentoDto()
            {
                QuadraId = 1,
                UsuarioId = 1,
                DataHoraInicio = new DateTime(2025, 10, 8, 21, 08, 0),
                DataHoraFim = new DateTime(2025, 10, 8, 22, 08, 0),
                MetodoPagamento = "Pix"
            };

            var agendamento = new Agendamento()
            {
                Id = 1,
                QuadraId = 1,
                UsuarioId = 1,
                DataHoraInicio = new DateTime(2025, 10, 8, 21, 08, 0),
                DataHoraFim = new DateTime(2025, 10, 8, 22, 08, 0),
                ValorTotal = 100
            };
            _agendamentoRepoMock.Setup(r => r.ExisteConflito(dto)).ReturnsAsync(false);
            _mapperMock.Setup(n => n.Map<Agendamento>(dto)).Returns(agendamento);

            //act
            var result = await _controller.Post(dto);

            //assert
            result.Should().NotBeNull();
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created.StatusCode.Should().Be(201);
            created.Value.Should().BeOfType<Agendamento>();
            var dados = created.Value as Agendamento;
            dados.ValorTotal.Should().Be(100);
            dados.QuadraId.Should().Be(1);

        }
    }
}
