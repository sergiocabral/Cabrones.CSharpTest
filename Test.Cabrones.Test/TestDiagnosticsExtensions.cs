using System;
using FluentAssertions;
using Xunit;

namespace Cabrones.Test
{
    public class TestDiagnosticsExtensions
    {
        [Fact]
        public void StopwatchFor_Func_deve_funcionar_corretamente()
        {
            // Arrange, Given

            var valorDeRetorno = DateTime.Now.Ticks;
            Func<long> método = () => valorDeRetorno;
                
            // Act, When

            var (tempo, valorRetornado) = método.StopwatchFor();
            
            // Assert, Then

            tempo.Should().BeGreaterThan(0);
            valorRetornado.Should().Be(valorDeRetorno);
        }
        
        [Fact]
        public void StopwatchFor_Action_deve_funcionar_corretamente()
        {
            // Arrange, Given

            Action método = () => { };
                
            // Act, When

            var tempo = método.StopwatchFor();
            
            // Assert, Then

            tempo.Should().BeGreaterThan(0);
        }
        
        [Fact]
        public void AssertTheSameValueButTheSecondTimeIsFaster_Func_deve_funcionar_corretamente()
        {
            // Arrange, Given

            Func<int> comCache = () => new ClasseNeta().ClasseNetaPropriedadePúblicaComCache;
            Func<int> semCache = () => new ClasseNeta().ClasseNetaPropriedadePúblicaSemCache;
                
            // Act, When

            Action testarParaPassar = () => comCache.AssertTheSameValueButTheSecondTimeIsFaster();
            Action testarParaFalhar = () => semCache.AssertTheSameValueButTheSecondTimeIsFaster();
            
            // Assert, Then

            testarParaPassar.Should().NotThrow();
            testarParaFalhar.Should().Throw<Exception>();
        }
        
        [Fact]
        public void AssertTheSameValueButTheSecondTimeIsFaster_Action_deve_funcionar_corretamente()
        {
            // Arrange, Given

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action comCache = () => new ClasseNeta().ClasseNetaPropriedadePúblicaComCache.ToString();
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action semCache = () => new ClasseNeta().ClasseNetaPropriedadePúblicaSemCache.ToString();
                
            // Act, When

            Action testarParaPassar = () => comCache.AssertTheSameValueButTheSecondTimeIsFaster();
            Action testarParaFalhar = () => semCache.AssertTheSameValueButTheSecondTimeIsFaster();
            
            // Assert, Then

            testarParaPassar.Should().NotThrow();
            testarParaFalhar.Should().Throw<Exception>();
        }
        
        [Fact]
        public void AssertTheSameValueButTheSecondTimeIsFaster_Object_deve_funcionar_corretamente()
        {
            // Arrange, Given

            var instância = new ClasseNeta();
                
            // Act, When

            Action testarParaPassar = () => instância.AssertTheSameValueButTheSecondTimeIsFaster("ClasseNetaPropriedadePúblicaComCache");
            Action testarParaFalhar = () => instância.AssertTheSameValueButTheSecondTimeIsFaster("ClasseNetaPropriedadePúblicaSemCache");
            
            // Assert, Then

            testarParaPassar.Should().NotThrow();
            testarParaFalhar.Should().Throw<Exception>();
        }
        
        [Fact]
        public void AssertTheSameValueButTheSecondTimeIsFaster_Type_deve_funcionar_corretamente()
        {
            // Arrange, Given

            var tipo = typeof(ClasseNeta);
                
            // Act, When

            Action testarParaPassar = () => tipo.AssertTheSameValueButTheSecondTimeIsFaster("ClasseNetaPropriedadePúblicaComCacheEstática");
            Action testarParaFalhar = () => tipo.AssertTheSameValueButTheSecondTimeIsFaster("ClasseNetaPropriedadePúblicaSemCacheEstática");
            
            // Assert, Then

            testarParaPassar.Should().NotThrow();
            testarParaFalhar.Should().Throw<Exception>();
        }
    }
}