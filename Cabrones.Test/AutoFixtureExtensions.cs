using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;

namespace Cabrones.Test
{
    /// <summary>
    /// Extensões relacionadas a biblioteca AutoFixture.
    /// </summary>
    public static class AutoFixtureExtensions
    {
        /// <summary>
        /// Instância para Fixture.
        /// </summary>
        private static Fixture? _fixtureDefault;

        /// <summary>
        /// Instância para Fixture.
        /// Criado instância apenas na primeira vez que usar.
        /// </summary>
        private static Fixture FixtureDefault => _fixtureDefault ??= new Fixture();

        /// <summary>
        /// Criador de valores para tipos.
        /// </summary>
        // ReSharper disable once UnusedParameter.Global
        public static Fixture GetFixture(this object _) => FixtureDefault;

        /// <summary>
        /// Cria um valor qualquer para um tipo.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <returns>Valor criado.</returns>
        public static object Fixture(this Type type)
        {
            var method = typeof(SpecimenFactory).GetMethods()
                .Single(a => a.Name == "Create" &&
                             a.IsGenericMethod &&
                             a.GetParameters().Length == 1 &&
                             a.GetParameters()[0].ParameterType == typeof(ISpecimenBuilder));
            var methodGeneric = method.MakeGenericMethod(type);
            return methodGeneric.Invoke(null, new object[] {FixtureDefault})!;
        }

        /// <summary>
        /// Cria um valor qualquer para um tipo.
        /// </summary>
        /// <param name="_">Não utilizado.</param>
        /// <typeparam name="T">Tipo.</typeparam>
        /// <returns>Valor criado.</returns>
        // ReSharper disable once UnusedParameter.Global
        public static T Fixture<T>(this object _) =>
            (T) Fixture(typeof(T));

        /// <summary>
        /// Cria um valor qualquer para um tipo.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="count">Total de instâncias para criar.</param>
        /// <returns>Valor criado.</returns>
        public static IEnumerable<object> FixtureMany(this Type type, int count = 3)
        {
            var method = typeof(SpecimenFactory).GetMethods()
                .Single(a => a.Name == "CreateMany" &&
                             a.IsGenericMethod &&
                             a.GetParameters().Length == 2 &&
                             a.GetParameters()[0].ParameterType == typeof(ISpecimenBuilder) &&
                             a.GetParameters()[1].ParameterType == typeof(int));
            var methodGeneric = method.MakeGenericMethod(type);
            return ((IEnumerable) methodGeneric.Invoke(null, new object[] {FixtureDefault, count})!).Cast<object>();
        }

        /// <summary>
        /// Cria um valor qualquer para um tipo.
        /// </summary>
        /// <param name="_">Não utilizado.</param>
        /// <param name="count">Total de instâncias para criar.</param>
        /// <typeparam name="T">Tipo.</typeparam>
        /// <returns>Valor criado.</returns>
        // ReSharper disable once UnusedParameter.Global
        public static IEnumerable<T> FixtureMany<T>(this object _, int count = 3) =>
            FixtureMany(typeof(T), count).Select(a => (T)a);
    }
}