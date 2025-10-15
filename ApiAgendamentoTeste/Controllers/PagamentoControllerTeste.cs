using ApiAgendamento.Controllers;
using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Implementations;
using ApiAgendamento.Repositories.Inferfaces;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApiAgendamentoTeste.Controllers
{
    public class PagamentoControllerTeste
    {
        private readonly Mock<IRepository<Pagamento>> _pagamentoRepoMock;
        private readonly Mock<IAgendamentoRepository> _agendamentoRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PagamentoController _controller;

        public PagamentoControllerTeste()
        {
            _pagamentoRepoMock = new Mock<IRepository<Pagamento>>();
            _agendamentoRepoMock = new Mock<IAgendamentoRepository>();
            _mapperMock = new Mock<IMapper>();

            _controller = new PagamentoController(
                _mapperMock.Object,
                _pagamentoRepoMock.Object,
                _agendamentoRepoMock.Object
            );
        }

        [Fact]
        public async Task ConfirmarPagamento_DeveRetornarBadRequest_QuandoPagamentoNaoExiste()
        {
            // Arrange
            _pagamentoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Pagamento)null);

            // Act
            var result = await _controller.ConfirmarPagamento(1, StatusPagamento.APROVADO);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Pagamento não encontrado!");
        }

        [Fact]
        public async Task ConfirmarPagamento_DeveRetornarBadRequest_QuandoAgendamentoNaoExiste()
        {
            // Arrange
            var pagamento = new Pagamento()
            {
                Id = 1,
                AgendamentoId = 99,
                Status = StatusPagamento.PENDENTE
            };

            _pagamentoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pagamento);
            _agendamentoRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Agendamento)null);

            // Act
            var result = await _controller.ConfirmarPagamento(1, StatusPagamento.APROVADO);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Pagamento não encontrado!");
        }

        [Fact]
        public async Task ConfirmarPagamento_DeveAprovarPagamentoEAgendamento()
        {
            // Arrange
            var pagamento = new Pagamento()
            {
                Id = 1,
                AgendamentoId = 1,
                Status = StatusPagamento.PENDENTE
            };

            var agendamento = new Agendamento()
            {
                Id = 1,
                Status = StatusAgendamento.PENDENTE
            };

            _pagamentoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pagamento);
            _agendamentoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(agendamento);

            // Act
            var result = await _controller.ConfirmarPagamento(1, StatusPagamento.APROVADO);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().Be("Pagamento confirmado com sucesso!");

            pagamento.Status.Should().Be(StatusPagamento.APROVADO);
            pagamento.DataPagamento.Should().NotBeNull();
            agendamento.Status.Should().Be(StatusAgendamento.APROVADO);

            _pagamentoRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Pagamento>()), Times.Once);
            _agendamentoRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Agendamento>()), Times.Once);
            _pagamentoRepoMock.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task ConfirmarPagamento_DeveEstornarPagamentoECancelarAgendamento()
        {
            // Arrange
            var pagamento = new Pagamento()
            {
                Id = 2,
                AgendamentoId = 2,
                Status = StatusPagamento.PENDENTE
            };

            var agendamento = new Agendamento()
            {
                Id = 2,
                Status = StatusAgendamento.PENDENTE
            };

            _pagamentoRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(pagamento);
            _agendamentoRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(agendamento);

            // Act
            var result = await _controller.ConfirmarPagamento(2, StatusPagamento.ESTORNADO);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            agendamento.Status.Should().Be(StatusAgendamento.CANCELADO);
            pagamento.Status.Should().Be(StatusPagamento.ESTORNADO);
        }

        [Fact]
        public async Task ConfirmarPagamento_DeveRecusarPagamentoEAgendamento()
        {
            // Arrange
            var pagamento = new Pagamento()
            {
                Id = 3,
                AgendamentoId = 3,
                Status = StatusPagamento.PENDENTE
            };

            var agendamento = new Agendamento()
            {
                Id = 3,
                Status = StatusAgendamento.PENDENTE
            };

            _pagamentoRepoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(pagamento);
            _agendamentoRepoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(agendamento);

            // Act
            var result = await _controller.ConfirmarPagamento(3, StatusPagamento.RECUSADO);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            pagamento.Status.Should().Be(StatusPagamento.RECUSADO);
            agendamento.Status.Should().Be(StatusAgendamento.RECUSADO);
        }
    }
}
