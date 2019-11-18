﻿using System;
using FluentAssertions;
using Xunit;

namespace Cabrones.Test
{
    public class TestDeclarationsExtensions
    {
        [Fact]
        public void AssertEnumValuesCount_só_deve_funcionar_com_Enum()
        {
            // Arrange, Given

            var tipoCorreto = typeof(EnumList);
            var tipoIncorreto = typeof(string);

            // Act, When

            Action deveFuncionar = () => tipoCorreto.AssertEnumValuesCount(Enum.GetValues(typeof(EnumList)).Length);
            Action nãoDeveFuncionar = () => tipoIncorreto.AssertEnumValuesCount(0);

            // Assert, Then
            
            deveFuncionar.Should().NotThrow();
            nãoDeveFuncionar.Should().ThrowExactly<ArgumentException>();
        }
        
        [Theory]
        [InlineData(typeof(EnumList), 4)]
        public void AssertEnumValuesCount_deve_funcionar_corretamente(Type tipoEnum, int totalEsperado)
        {
            // Arrange, Given
            // Act, When
            // Assert, Then
            
            tipoEnum.AssertEnumValuesCount(totalEsperado);
        }
        
        [Theory]
        [InlineData(typeof(EnumList), 0)]
        [InlineData(typeof(IInterface1), 4)]
        [InlineData(typeof(IInterface2), 4)]
        [InlineData(typeof(IInterface3), 4)]
        [InlineData(typeof(ClassePai), 0)]
        [InlineData(typeof(ClasseFilha), 4)]
        [InlineData(typeof(ClasseNeta), 7)]
        public void AssertMyOwnPublicPropertiesCount_deve_funcionar_corretamente(Type tipo, int totalEsperado)
        {
            // Arrange, Given
            // Act, When
            // Assert, Then
            
            tipo.AssertMyOwnPublicPropertiesCount(totalEsperado);
        }
        
        [Theory]
        [InlineData(typeof(EnumList), 0)]
        [InlineData(typeof(IInterface1), 1)]
        [InlineData(typeof(IInterface2), 1)]
        [InlineData(typeof(IInterface3), 1)]
        [InlineData(typeof(ClassePai), 3)]
        [InlineData(typeof(ClasseFilha), 0)]
        [InlineData(typeof(ClasseNeta), 1)]
        public void AssertMyOwnPublicMethodsCount_deve_funcionar_corretamente(Type tipo, int totalEsperado)
        {
            // Arrange, Given
            // Act, When
            // Assert, Then
            
            tipo.AssertMyOwnPublicMethodsCount(totalEsperado);
        }
        
        [Theory]
        [InlineData(typeof(IInterface1), "Int32 Interface1Propriedade { get; set; }", true)]
        [InlineData(typeof(IInterface1), "Int32 Interface1Propriedade { get; }", false)]
        [InlineData(typeof(IInterface1), "Int32 Interface1Propriedade { set; }", false)]
        [InlineData(typeof(IInterface2), "Int32 Interface1Propriedade { get; set; }", false)]
        [InlineData(typeof(ClasseNeta), "Int32 ClasseNetaPropriedadePúblicaInstância { set; }", true)]
        [InlineData(typeof(ClasseNeta), "Int32 ClasseNetaPropriedadePúblicaComCacheEstática { get; }", true)]
        [InlineData(typeof(ClasseNeta), "Int32 ClasseNetaPropriedadePúblicaComCacheEstática { get; set; }", false)]
        [InlineData(typeof(ClasseNeta), "IDictionary<String[,], String[][][]> PropriedadeComplicada { get; set; }", true)]
        public void AssertPublicPropertyPresence_deve_funcionar_corretamente(Type tipo, string assinatura, bool existe)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertPublicPropertyPresence(assinatura);
            
            // Assert, Then

            if (existe)
            {
                testar.Should().NotThrow();
            }
            else
            {
                testar.Should().Throw<Exception>();
            }
        }
        
        [Theory]
        [InlineData(typeof(IInterface1), "String Interface1Método()", true)]
        [InlineData(typeof(IInterface2), "String Interface1Método()", false)]
        [InlineData(typeof(ClasseNeta), "TTipo[] MétodoGeneric<TTipo>(String, TTipo, TTipo[])", true)]
        public void AssertPublicMethodPresence_deve_funcionar_corretamente(Type tipo, string assinatura, bool existe)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertPublicMethodPresence(assinatura);
            
            // Assert, Then

            if (existe)
            {
                testar.Should().NotThrow();
            }
            else
            {
                testar.Should().Throw<Exception>();
            }
        }
        
        [Theory]
        [InlineData(typeof(IInterface1), false, typeof(IInterface3))]
        [InlineData(typeof(IInterface3), true, typeof(IInterface1))]
        [InlineData(typeof(ClassePai), true, typeof(IInterface1), typeof(IInterface2))]
        [InlineData(typeof(ClasseFilha), false, typeof(ClassePai), typeof(IInterface3))]
        [InlineData(typeof(ClasseFilha), true, typeof(ClassePai), typeof(IInterface3), typeof(IInterface1), typeof(IInterface2))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha))]
        [InlineData(typeof(ClasseNeta), true, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface1), typeof(IInterface2), typeof(IInterface3))]
        [InlineData(typeof(ClasseSozinha), true)]
        [InlineData(typeof(ClasseSozinha), false, typeof(object))]
        public void AssertMyImplementations_deve_funcionar_corretamente(Type tipo, bool estáCorreto, params Type[] implementações)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertMyImplementations(implementações);
            
            // Assert, Then

            if (estáCorreto)
            {
                testar.Should().NotThrow();
            }
            else
            {
                testar.Should().Throw<Exception>();
            }
        }
        
        [Theory]
        [InlineData(typeof(IInterface1), false, typeof(IInterface3))]
        [InlineData(typeof(IInterface3), true, typeof(IInterface1))]
        [InlineData(typeof(ClassePai), true, typeof(IInterface1), typeof(IInterface2))]
        [InlineData(typeof(ClasseFilha), true, typeof(ClassePai), typeof(IInterface3))]
        [InlineData(typeof(ClasseFilha), false, typeof(ClassePai), typeof(IInterface3), typeof(IInterface1), typeof(IInterface2))]
        [InlineData(typeof(ClasseSozinha), true)]
        [InlineData(typeof(ClasseSozinha), false, typeof(object))]
        public void AssertMyOwnImplementations_deve_funcionar_corretamente(Type tipo, bool estáCorreto, params Type[] implementações)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertMyOwnImplementations(implementações);
            
            // Assert, Then

            if (estáCorreto)
            {
                testar.Should().NotThrow();
            }
            else
            {
                testar.Should().Throw<Exception>();
            }
        }
    }
}