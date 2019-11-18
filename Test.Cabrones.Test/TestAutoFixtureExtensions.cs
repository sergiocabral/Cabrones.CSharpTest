using FluentAssertions;
using Xunit;

namespace Cabrones.Test
{
    public class TestAutoFixtureExtensions
    {
        [Fact]
        public void uma_instância_AutoFixture_única_deve_estar_acessível_de_qualquer_instância()
        {
            // Arrange, Given

            var tipo1 = typeof(object);
            var tipo2 = typeof(string);

            // Act, When

            var instância1 = tipo1.GetFixture();
            var instância2 = tipo2.GetFixture();

            // Assert, Then

            instância1.Should().BeSameAs(instância2);
        }
        
        [Fact]
        public void deve_ser_possível_criar_valor_para_qualquer_tipo_com_AutoFixture()
        {
            // Arrange, Given

            var tipoString1 = typeof(string);
            var tipoString2 = typeof(string);
            var tipoInteiro1 = typeof(int);
            var tipoInteiro2 = typeof(int);

            // Act, When

            var valorString1 = tipoString1.Fixture();
            var valorString2 = tipoString2.Fixture();
            var valorInteiro1 = tipoInteiro1.Fixture();
            var valorInteiro2 = tipoInteiro2.Fixture();

            // Assert, Then

            valorString1.GetType().Should().Be(tipoString1);
            valorString1.Should().NotBe(valorString2);
            valorInteiro1.GetType().Should().Be(tipoInteiro1);
            valorInteiro1.Should().NotBe(valorInteiro2);
        }
        
        [Fact]
        public void deve_ser_possível_criar_valor_para_qualquer_tipo_com_AutoFixture_a_partir_de_qualquer_instância()
        {
            // Arrange, Given
            // Act, When

            var valorString1 = this.Fixture<string>();
            var valorString2 = this.Fixture<string>();
            var valorInteiro1 = this.Fixture<int>();
            var valorInteiro2 = this.Fixture<int>();

            // Assert, Then

            valorString1.Should().NotBe(valorString2);
            valorInteiro1.Should().NotBe(valorInteiro2);
        }
    }
}