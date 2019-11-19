using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Cabrones.Test
{
    public class TestAutoFixtureExtensions
    {
        [Fact]
        public void GetFixture_deve_estar_acessível_de_qualquer_instância_e_entregar_uma_instância_única()
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
        public void Fixture_deve_estar_acessível_de_qualquer_tipo_e_criar_valor_aleatório_para_ele()
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
        public void Fixture_deve_estar_acessível_de_qualquer_lugar_e_criar_valor_aleatório_de_qualquer_tipo()
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
        
        [Fact]
        public void FixtureMany_deve_estar_acessível_de_qualquer_tipo_e_criar_valor_aleatório_para_ele()
        {
            // Arrange, Given

            var random = new Random(DateTime.Now.Millisecond);
            
            var tipoString1 = typeof(string);
            var tipoString2 = typeof(string);
            var tipoInteiro1 = typeof(int);
            var tipoInteiro2 = typeof(int);

            // Act, When

            var valorString1 = tipoString1.FixtureMany(random.Next(1, 20)).ToList();
            var valorString2 = tipoString2.FixtureMany(random.Next(1, 20)).ToList();
            var valorInteiro1 = tipoInteiro1.FixtureMany(random.Next(1, 20)).ToList();
            var valorInteiro2 = tipoInteiro2.FixtureMany(random.Next(1, 20)).ToList();

            // Assert, Then

            valorString1.First().GetType().Should().Be(tipoString1);
            valorString1.Should().NotBeEquivalentTo(valorString2);
            valorInteiro1.First().GetType().Should().Be(tipoInteiro1);
            valorInteiro1.Should().NotBeEquivalentTo(valorInteiro2);
        }
        
        [Fact]
        public void FixtureMany_sem_parâmetro_deve_estar_acessível_de_qualquer_lugar_e_criar_valor_aleatório_de_qualquer_tipo()
        {
            // Arrange, Given
            // Act, When

            var valorString1 = this.FixtureMany<string>();
            var valorString2 = this.FixtureMany<string>();
            var valorInteiro1 = this.FixtureMany<int>();
            var valorInteiro2 = this.FixtureMany<int>();

            // Assert, Then

            valorString1.Should().NotBeEquivalentTo(valorString2);
            valorInteiro1.Should().NotBeEquivalentTo(valorInteiro2);
        }
        
        [Fact]
        public void FixtureMany_informando_total_de_resultados_deve_estar_acessível_de_qualquer_lugar_e_criar_valor_aleatório_de_qualquer_tipo()
        {
            // Arrange, Given
            // Act, When

            var valorString1 = this.FixtureMany<string>(3);
            var valorString2 = this.FixtureMany<string>(3);
            var valorInteiro1 = this.FixtureMany<int>(3);
            var valorInteiro2 = this.FixtureMany<int>(3);

            // Assert, Then

            valorString1.Should().NotBeEquivalentTo(valorString2);
            valorInteiro1.Should().NotBeEquivalentTo(valorInteiro2);
        }
    }
}