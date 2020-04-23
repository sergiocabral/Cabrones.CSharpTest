using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cabrones.Utils.Reflection;
using FluentAssertions;

namespace Cabrones.Test
{
    /// <summary>
    ///     Extensões relacionadas teste de declarações via reflection.
    /// </summary>
    public static class DeclarationsExtensions
    {
        /// <summary>
        ///     Testar se a quantidade de valores em um enum está correta.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="count">Total de esperado.</param>
        public static void AssertEnumValuesCount(this Type type, int count)
        {
            if (!type.IsEnum) throw new ArgumentException();
            Enum.GetNames(type).Should().HaveCount(count, nameof(AssertEnumValuesCount));
        }

        /// <summary>
        ///     Testar se os valores existem em um enum.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="values">Valores que devem estar presentes no enum.</param>
        public static void AssertEnumValuesContains(this Type type, params string[] values)
        {
            if (!type.IsEnum) throw new ArgumentException();
            var allValues = Enum.GetNames(type);
            foreach (var value in values) allValues.Should().Contain(value);
        }

        /// <summary>
        ///     Testa se a quantidade de propriedades próprios públicas está correta.
        ///     Não considera interface ou herança.
        ///     Inclui membros estáticos e da instância.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="count">Total de esperado.</param>
        public static void AssertMyOwnPublicPropertiesCount(this Type type, int count)
        {
            var own = type.MyOwnProperties().Where(a => a.IsPublic).ToList();
            own.Should().HaveCount(count, nameof(AssertMyOwnPublicPropertiesCount));
        }

        /// <summary>
        ///     Testa se a quantidade de métodos próprios públicos está correta.
        ///     Não considera interface ou herança.
        ///     Inclui membros estáticos e da instância.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="count">Total de esperado.</param>
        public static void AssertMyOwnPublicMethodsCount(this Type type, int count)
        {
            var own = type.MyOwnMethods().Where(a => a.IsPublic).ToList();
            own.Should().HaveCount(count, nameof(AssertMyOwnPublicMethodsCount));
        }

        /// <summary>
        ///     Testa se as implementações diretamente em um tipo estão corretas.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="implementations">Classe base e interfaces.</param>
        public static void AssertMyOwnImplementations(this Type type, params Type[] implementations)
        {
            var myOwnImplementationsAsString = type
                .MyOwnImplementations()
                .Select(a => a.ToString())
                .ToArray();

            var implementationsAsString = implementations
                .Select(a => a.ToString())
                .ToArray();

            myOwnImplementationsAsString.Should()
                .BeEquivalentTo(implementationsAsString.ToList(),
                    nameof(AssertMyOwnImplementations));
        }

        /// <summary>
        ///     Testa se uma propriedade existe declarado no tipo.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="signature">Assinatura esperada.</param>
        public static void AssertPublicPropertyPresence(this Type type, string signature)
        {
            var signatures = type
                .AllProperties().Where(a => a.IsPublic)
                .Select(a => a.GetProperty()).Distinct()
                .Select(a => a?.ToSignatureCSharp()).ToList();
            signatures.Should().Contain(signature, nameof(AssertPublicPropertyPresence));
        }

        /// <summary>
        ///     Testa se um método existe declarado no tipo.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="signature">Assinatura esperada.</param>
        public static void AssertPublicMethodPresence(this Type type, string signature)
        {
            var signatures = type.AllMethods().Where(a => a.IsPublic).Select(a => a.ToSignatureCSharp()).ToList();
            signatures.Should().Contain(signature, nameof(AssertPublicMethodPresence));
        }

        /// <summary>
        ///     Testa se todas as implementações de um tipo estão corretas.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="implementations">Classe base e interfaces.</param>
        public static void AssertMyImplementations(this Type type, params Type[] implementations)
        {
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            Type? loopType = type;
            var myImplementations = new List<Type>();
            while (loopType != null)
            {
                if (loopType.BaseType != null) myImplementations.Add(loopType.BaseType);
                myImplementations.AddRange(loopType.GetInterfaces());
                loopType = loopType.BaseType;
            }

            myImplementations = myImplementations.Where(a => a != typeof(object)).Distinct().ToList();

            const string regexRemoveGenericTypes = @"(?<=[\[,])[A-Za-z0-9_\.]+(?=[,\]])";
            var myImplementationsAsString = myImplementations
                .Select(a => 
                    Regex.Replace(a.ToString(), regexRemoveGenericTypes, string.Empty)).ToList();
            var implementationsAsString = implementations
                .Select(a => 
                    Regex.Replace(a.ToString(), regexRemoveGenericTypes, string.Empty)).ToList();

            myImplementationsAsString.Should()
                .BeEquivalentTo(implementationsAsString.ToList(), nameof(AssertMyImplementations));
        }
    }
}