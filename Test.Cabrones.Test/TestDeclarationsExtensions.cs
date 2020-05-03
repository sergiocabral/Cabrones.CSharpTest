using System;
using FluentAssertions;
using Xunit;

namespace Cabrones.Test
{
    public class TestDeclarationsExtensions
    {
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
        [InlineData(typeof(EnumList), true, "EnumItem1")]
        [InlineData(typeof(EnumList), true, "EnumItem1", "EnumItem2")]
        [InlineData(typeof(EnumList), true, "EnumItem1", "EnumItem2", "EnumItem3")]
        [InlineData(typeof(EnumList), true, "EnumItem1", "EnumItem2", "EnumItem3", "EnumItem4")]
        [InlineData(typeof(EnumList), false, "EnumItem1", "EnumItem2", "EnumItem3", "EnumItem4", "EnumItem5")]
        [InlineData(typeof(EnumList), false, "NotExists")]
        public void AssertEnumValuesContains_deve_funcionar_corretamente(Type tipoEnum, bool result,
            params string[] valores)
        {
            // Arrange, Given
            // Act, When

            Action executar = () => tipoEnum.AssertEnumValuesContains(valores);

            // Assert, Then

            if (result)
                executar.Should().NotThrow();
            else
                executar.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(typeof(EnumList), 0)]
        [InlineData(typeof(IInterface1), 0)]
        [InlineData(typeof(IInterface2), 0)]
        [InlineData(typeof(IInterface3), 0)]
        [InlineData(typeof(ClassePai), 0)]
        [InlineData(typeof(ClasseFilha), 0)]
        [InlineData(typeof(ClasseNeta), 0)]
        [InlineData(typeof(ClasseComEvento), 2)]
        [InlineData(typeof(InterfaceComEvento), 1)]
        public void AssertMyOwnPublicEventsCount_deve_funcionar_corretamente(Type tipo, int totalEsperado)
        {
            // Arrange, Given
            // Act, When
            // Assert, Then

            tipo.AssertMyOwnPublicEventsCount(totalEsperado);
        }

        [Theory]
        [InlineData(typeof(EnumList), 0)]
        [InlineData(typeof(IInterface1), 4)]
        [InlineData(typeof(IInterface2), 4)]
        [InlineData(typeof(IInterface3), 4)]
        [InlineData(typeof(ClassePai), 0)]
        [InlineData(typeof(ClasseFilha), 4)]
        [InlineData(typeof(ClasseNeta), 7)]
        [InlineData(typeof(ClasseComEvento), 0)]
        [InlineData(typeof(InterfaceComEvento), 0)]
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
        [InlineData(typeof(ClasseComEvento), 0)]
        [InlineData(typeof(InterfaceComEvento), 0)]
        public void AssertMyOwnPublicMethodsCount_deve_funcionar_corretamente(Type tipo, int totalEsperado)
        {
            // Arrange, Given
            // Act, When
            // Assert, Then

            tipo.AssertMyOwnPublicMethodsCount(totalEsperado);
        }

        [Theory]
        [InlineData(typeof(ClasseComEvento), "Action EventoDaInstância", true)]
        [InlineData(typeof(ClasseComEvento), "static Func<String, Int32> EventoEstático", true)]
        [InlineData(typeof(InterfaceComEvento), "Action EventoDaInterface", true)]
        public void AssertPublicEventPresence_deve_funcionar_corretamente(Type tipo, string assinatura, bool existe)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertPublicEventPresence(assinatura);

            // Assert, Then

            if (existe)
                testar.Should().NotThrow();
            else
                testar.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(typeof(IInterface1), "Int32 Interface1Propriedade { get; set; }", true)]
        [InlineData(typeof(IInterface1), "Int32 Interface1Propriedade { get; }", false)]
        [InlineData(typeof(IInterface1), "Int32 Interface1Propriedade { set; }", false)]
        [InlineData(typeof(IInterface2), "Int32 Interface1Propriedade { get; set; }", false)]
        [InlineData(typeof(ClasseNeta), "Int32 ClasseNetaPropriedadePúblicaInstância { set; }", true)]
        [InlineData(typeof(ClasseNeta), "static Int32 ClasseNetaPropriedadePúblicaComCacheEstática { get; }", true)]
        [InlineData(typeof(ClasseNeta), "static Int32 ClasseNetaPropriedadePúblicaComCacheEstática { get; set; }",
            false)]
        [InlineData(typeof(ClasseNeta),
            "static IDictionary<String[,], String[][][]> PropriedadeComplicada { get; set; }", true)]
        [InlineData(typeof(ClasseComModificadoresDeAcesso), "Int32 PropriedadeSetPrivate { get; }", true)]
        [InlineData(typeof(ClasseComModificadoresDeAcesso), "Int32 PropriedadeSetPrivate { get; set; }", false)]
        [InlineData(typeof(ClasseComModificadoresDeAcesso), "Int32 PropriedadeGetInternal { set; }", true)]
        [InlineData(typeof(ClasseComModificadoresDeAcesso), "Int32 PropriedadeGetInternal { get; set; }", false)]
        [InlineData(typeof(ClasseComModificadoresDeAcesso), "Int32 PropriedadeSetProtected { get; }", true)]
        [InlineData(typeof(ClasseComModificadoresDeAcesso), "Int32 PropriedadeSetProtected { get; set; }", false)]
        public void AssertPublicPropertyPresence_deve_funcionar_corretamente(Type tipo, string assinatura, bool existe)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertPublicPropertyPresence(assinatura);

            // Assert, Then

            if (existe)
                testar.Should().NotThrow();
            else
                testar.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(typeof(IInterface1), "String Interface1Método()", true)]
        [InlineData(typeof(IInterface2), "String Interface1Método()", false)]
        [InlineData(typeof(ClasseNeta), "TTipo[] MétodoGeneric<TTipo>(String, TTipo, TTipo[])", true)]
        [InlineData(typeof(ClasseEstática), "static DateTime Agora()", true)]
        public void AssertPublicMethodPresence_deve_funcionar_corretamente(Type tipo, string assinatura, bool existe)
        {
            //TODO: Criar uma verificação de eventos ao invés de localizar por métodos.
            
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertPublicMethodPresence(assinatura);

            // Assert, Then

            if (existe)
                testar.Should().NotThrow();
            else
                testar.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(typeof(IInterface1), false, typeof(IInterface3))]
        [InlineData(typeof(IInterface3), true, typeof(IInterface1))]
        [InlineData(typeof(ClassePai), true, typeof(IInterface1), typeof(IInterface2))]
        [InlineData(typeof(ClasseFilha), false, typeof(ClassePai), typeof(IInterface3))]
        [InlineData(typeof(ClasseFilha), true, typeof(ClassePai), typeof(IInterface3), typeof(IInterface1),
            typeof(IInterface2))]
        [InlineData(typeof(ClasseNeta), false, typeof(ClasseFilha))]
        [InlineData(typeof(ClasseNeta), true, typeof(ClasseFilha), typeof(ClassePai), typeof(IInterface1),
            typeof(IInterface2), typeof(IInterface3))]
        [InlineData(typeof(ClasseSozinha), true)]
        [InlineData(typeof(ClasseSozinha), false, typeof(object))]
        [InlineData(typeof(ClasseGenérica<>), true, typeof(InterfaceGenerica<>))]
        [InlineData(typeof(ClasseGenérica<string>), true, typeof(InterfaceGenerica<string>))]
        [InlineData(typeof(InterfaceGenericaDuplaComUmaDefinida<>), true, typeof(InterfaceGenericaDupla<,>))]
        [InlineData(typeof(ClasseComInterfaceRepetida), true, typeof(InterfaceGenericaDupla<,>))]
        [InlineData(typeof(ClasseComInterfaceRepetida), false, typeof(InterfaceGenericaDupla<,>), typeof(InterfaceGenericaDupla<,>))]
        public void AssertMyImplementations_deve_funcionar_corretamente(Type tipo, bool estáCorreto,
            params Type[] implementações)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertMyImplementations(implementações);

            // Assert, Then

            if (estáCorreto)
                testar.Should().NotThrow();
            else
                testar.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData(typeof(IInterface1), false, typeof(IInterface3))]
        [InlineData(typeof(IInterface3), true, typeof(IInterface1))]
        [InlineData(typeof(ClassePai), true, typeof(IInterface1), typeof(IInterface2))]
        [InlineData(typeof(ClasseFilha), true, typeof(ClassePai), typeof(IInterface3))]
        [InlineData(typeof(ClasseFilha), false, typeof(ClassePai), typeof(IInterface3), typeof(IInterface1),
            typeof(IInterface2))]
        [InlineData(typeof(ClasseSozinha), true)]
        [InlineData(typeof(ClasseSozinha), false, typeof(object))]
        [InlineData(typeof(ClasseGenérica<>), true, typeof(InterfaceGenerica<>))]
        [InlineData(typeof(ClasseGenérica<string>), true, typeof(InterfaceGenerica<string>))]
        [InlineData(typeof(InterfaceGenericaDuplaComUmaDefinida<>), true, typeof(InterfaceGenericaDupla<,>))]
        [InlineData(typeof(ClasseComInterfaceRepetida), true, typeof(InterfaceGenericaDupla<,>))]
        [InlineData(typeof(ClasseComInterfaceRepetida), false, typeof(InterfaceGenericaDupla<,>), typeof(InterfaceGenericaDupla<,>))]
        public void AssertMyOwnImplementations_deve_funcionar_corretamente(Type tipo, bool estáCorreto,
            params Type[] implementações)
        {
            // Arrange, Given
            // Act, When

            Action testar = () => tipo.AssertMyOwnImplementations(implementações);

            // Assert, Then

            if (estáCorreto)
                testar.Should().NotThrow();
            else
                testar.Should().Throw<Exception>();
        }

        [Fact]
        public void AssertEnumValuesContains_só_deve_funcionar_com_Enum()
        {
            // Arrange, Given

            var tipoCorreto = typeof(EnumList);
            var tipoIncorreto = typeof(string);

            // Act, When

            Action deveFuncionar = () => tipoCorreto.AssertEnumValuesContains();
            Action nãoDeveFuncionar = () => tipoIncorreto.AssertEnumValuesContains();

            // Assert, Then

            deveFuncionar.Should().NotThrow();
            nãoDeveFuncionar.Should().ThrowExactly<ArgumentException>();
        }

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
    }
}