using ApiAgendamento;

namespace ApiAgendamentoTeste
{
    public class UnitTest1
    {
        [Fact]
        public void Somar_DoisNumeros_DeveRetornarSomaCorreta()
        {
            //arrange
            var calculadora = new Calculadora();
            int a = 2;
            int b = 3;
            int resultadoEsperado = 5;


            //act
            int resultadoReal = calculadora.Somar(a, b);

            //assert
            Assert.Equal(resultadoEsperado, resultadoReal);



        }
        [Fact]
        public void Subtrair_DoisNumeros_DeveRetornarSubtracaoCorreta()
        {
            //arrange
            var calculadora = new Calculadora();
            int a = 2;
            int b = 3;
            int resultadoEsperado = -1;

            //act
            int resultadoReal = calculadora.Subtrair(a, b);

            //assert
            Assert.Equal(resultadoEsperado, resultadoReal);

        }
        [Theory]
        [InlineData(2,3,5)]
        [InlineData(4,5,9)]
        [InlineData(-1,-4,-5)]
        public void Somar_VariosNumeros_DeveRetornarSomasCorretas(int a, int b, int resuiltadoEsperado)
        {
            //arrange
            var calculadora = new Calculadora();

            //act
            int resultado = calculadora.Somar(a, b);

            //assert
            Assert.Equal(resuiltadoEsperado, resultado);

        }
    }
}