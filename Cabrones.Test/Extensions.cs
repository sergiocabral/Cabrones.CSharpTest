using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Cabrones.Test
{
    /// <summary>
    /// Extensões para classe de teste.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Instância para Fixture.
        /// </summary>
        private static readonly Fixture FixtureDefault = new Fixture();

        /// <summary>
        /// Fixture.
        /// </summary>
        public static Fixture Fixture(this object _) => FixtureDefault;

        /// <summary>
        /// Testar se um tipo implementa um ou mais tipos.
        /// </summary>
        /// <param name="type">Tipo da classe.</param>
        /// <param name="implementations">TipoS que deve ser implementado.</param>
        public static void TestImplementations(this Type type, params Type[] implementations)
        {
            // Arrange, Given
            
            var methodsOfType = type.GetMethods().Where(a => a.IsPublic && a.DeclaringType == type && a.DeclaringType?.Assembly == type.Assembly).Select(a => a.ToString()).ToList();
            var methods = new List<MethodInfo>();

            foreach (var implementation in implementations)
            {
                methods.AddRange(implementation.GetMethods()
                    .Where(a => a.IsPublic && a.DeclaringType == implementation &&
                                a.DeclaringType?.Assembly == implementation.Assembly)
                    .ToList());
            }

            var methodsInType = type.GetMethods().Where(a => a.IsPublic).Select(a => a.ToString()).ToList();
            methods.RemoveAll(a => !methodsOfType.Contains(a.ToString()) && methodsInType.Contains(a.ToString()));
            
            // Act, When

            var countImplementations = implementations.Count(a => a.IsAssignableFrom(type));
            var countImplemented = methods.Count;

            // Assert, Then

            countImplementations.Should().Be(implementations.Length);
            countImplemented.Should().Be(methodsOfType.Count);
        }

        /// <summary>
        /// Testa se a quantidade de métodos está correta.
        /// </summary>
        /// <param name="type">Tipo a ser consultado.</param>
        /// <param name="count">Total de métodos esperados.</param>
        public static void TestMethodsCount(this Type type, int count)
        {
            // Arrange, Given

            var methods = type.GetMethods();

            // Act, When

            var methodsOfType = methods.Where(a => a.IsPublic && a.DeclaringType == type && a.DeclaringType?.Assembly == type.Assembly).ToList();

            // Assert, Then

            methodsOfType.Count.Should().Be(count);
        }
        
        /// <summary>
        /// Testa se a quantidade de valores em um enum está correta.
        /// </summary>
        /// <param name="type">Tipo a ser consultado.</param>
        /// <param name="count">Total de métodos esperados.</param>
        public static void TestEnumValuesCount(this Type type, int count)
        {
            if (!type.IsEnum) throw new ArgumentException();
            Enum.GetNames(type).Should().HaveCount(count);
        }
        
        /// <summary>
        /// Testa se um valor de enum existe com base no seu nome e valor.
        /// </summary>
        /// <param name="type">Tipo a ser consultado.</param>
        /// <param name="name">Nome esperado.</param>
        /// <param name="value">Valor esperado.</param>
        public static void TestEnumValuePresence(this Type type, string name, int value)
        {
            if (!type.IsEnum) throw new ArgumentException();
            Enum.GetNames(type).Should().Contain(name);
            Enum.Parse(type, name).Should().Be(value);
        }
        
        /// <summary>
        /// Testa se um método existe com base na sua assinatura.
        /// </summary>
        /// <param name="type">Tipo a ser consultado.</param>
        /// <param name="signature">Assinatura esperada.</param>
        public static void TestMethodPresence(this Type type, string signature)
        {
            // Arrange, Given

            string Format(Type typeToFormat)
            {
                var result = typeToFormat.ToString();
                if (Regex.IsMatch(result, @"`\d+\["))
                {
                    result = result
                        .Replace("[", "<")
                        .Replace("]", ">");
                }
                result = result
                    .Replace(",", ", ");
                result = Regex.Replace(result, @"(\w+\.|`\d+)", string.Empty);
                return result;
            }
            
            string Signature(MethodInfo method)
            {
                var parametersForGeneric = method.GetGenericArguments().ToList();
                var parameters = method.GetParameters().ToList();
             
                var result = new StringBuilder();
                
                result.Append($"{Format(method.ReturnType)} {method.Name}");
                
                if (parametersForGeneric.Count > 0)
                {
                    result.Append($"<{parametersForGeneric.Select(Format).Aggregate((acumulador, nomeDoTipo) => $"{(string.IsNullOrWhiteSpace(acumulador) ? "" : $"{acumulador}, ")}{nomeDoTipo}")}>");
                }

                result.Append(parameters.Count > 0
                    ? $"({parameters.Select(a => $"{Format(a.ParameterType)}{(a.HasDefaultValue ? $" = {(a.DefaultValue == null ? "null" : a.ParameterType == typeof(char) && ((char)0).Equals(a.DefaultValue) ? "''" : $"'{a.DefaultValue}'")}": "")}").Aggregate((acumulador, nomeDoTipo) => $"{(string.IsNullOrWhiteSpace(acumulador) ? "" : $"{acumulador}, ")}{nomeDoTipo}")})"
                    : "()");

                return result.ToString();
            }

            IEnumerable<MethodInfo> MethodsFound()
            {
                var methodName = Regex.Match(signature, @"\w+(?=(|<[^>]+>)\()").Value;
                return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).Where(a => a.Name == methodName);
            }

            IEnumerable<string> SignaturesFound() => MethodsFound().Select(Signature).ToList();

            // Act, When

            var signaturesFound = SignaturesFound();

            // Assert, Then

            signaturesFound.Should().Contain(signature);
        }
        
        /// <summary>
        /// Verifica se o cache está sendo usado ao consulta uma propriedade.
        /// A evidência é o tempo menor na segunda consulta.
        /// </summary>
        /// <param name="instance">Instância.</param>
        /// <param name="propertyName">Nome da propriedade.</param>
        public static void TestPropertyCache(this object instance, string propertyName)
        {
            // Arrange, Given

            var type = instance.GetType();
            var property = type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
           
            object Query() => property?.GetValue(instance);

            // Act, When

            var (time1, value1) = StopwatchFor(Query);
            var (time2, value2) = StopwatchFor(Query);

            // Assert, Then
            
            value2.Should().BeEquivalentTo(value1);
            time2.Should().BeLessThan(time1);
        }
        
        /// <summary>
        /// Faz uma consulta qualquer e cronometra o tempo.
        /// </summary>
        /// <param name="query">Função de consulta.</param>
        /// <typeparam name="T">Tipo de retorno.</typeparam>
        /// <returns>Tempo e valor retornado.</returns>
        public static Tuple<long, T> StopwatchFor<T>(this Func<T> query)
        {
            var stopwatch = new Stopwatch();
                
            stopwatch.Start();
            var value = query();
            stopwatch.Stop();
            var time = stopwatch.ElapsedTicks;
            
            return new Tuple<long, T>(time, value);
        }
    }
}