using System;
using System.Diagnostics;
using System.Reflection;
using FluentAssertions;

namespace Cabrones.Test
{
    /// <summary>
    ///     Extensões relacionadas teste de performance.
    /// </summary>
    public static class DiagnosticsExtensions
    {
        /// <summary>
        ///     Cronometra o tempo de execução de um método.
        /// </summary>
        /// <param name="action">Método de execução.</param>
        /// <typeparam name="T">Tipo de retorno.</typeparam>
        /// <returns>Tempo e valor retornado.</returns>
        public static (long, T) StopwatchFor<T>(this Func<T> action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var value = action();
            stopwatch.Stop();
            return (stopwatch.ElapsedTicks, value);
        }

        /// <summary>
        ///     Cronometra o tempo de execução de um método.
        /// </summary>
        /// <param name="action">Método de execução.</param>
        /// <returns>Tempo.</returns>
        public static long StopwatchFor(this Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedTicks;
        }

        /// <summary>
        ///     Testa se duas consultas seguidas retornam o mesmo valor,
        ///     mas a segunda execução deve ser mais rápida que a primeira.
        ///     Indicado para testar cache.
        /// </summary>
        /// <param name="action">Método de execução.</param>
        /// <typeparam name="T">Tipo de retorno.</typeparam>
        public static void AssertTheSameValueButTheSecondTimeIsFaster<T>(this Func<T> action)
        {
            var (time1, value1) = StopwatchFor(action);
            var (time2, value2) = StopwatchFor(action);

            time2.Should().BeLessThan((long) (time1 * 0.90));
            value2.Should().BeEquivalentTo(value1);
        }

        /// <summary>
        ///     Testa se duas execuções são feitas seguidamente
        ///     mas a segunda execução deve ser mais rápida que a primeira.
        ///     Indicado para testar cache.
        /// </summary>
        /// <param name="action">Método de execução.</param>
        public static void AssertTheSameValueButTheSecondTimeIsFaster(this Action action)
        {
            var time1 = StopwatchFor(action);
            var time2 = StopwatchFor(action);

            time2.Should().BeLessThan((long) (time1 * 0.99));
        }

        /// <summary>
        ///     Testa se duas consultas seguidas retornam o mesmo valor,
        ///     mas a segunda execução deve ser mais rápida que a primeira.
        ///     Indicado para testar cache.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <param name="propertyName">Propriedade estática.</param>
        public static void AssertTheSameValueButTheSecondTimeIsFaster(this Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName,
                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static) ??
                           throw new NullReferenceException();

            Func<object?> getValue = () => property.GetValue(null);

            getValue.AssertTheSameValueButTheSecondTimeIsFaster();
        }

        /// <summary>
        ///     Testa se duas consultas seguidas retornam o mesmo valor,
        ///     mas a segunda execução deve ser mais rápida que a primeira.
        ///     Indicado para testar cache.
        /// </summary>
        /// <param name="instance">Instância.</param>
        /// <param name="propertyName">Propriedade.</param>
        public static void AssertTheSameValueButTheSecondTimeIsFaster(this object instance, string propertyName)
        {
            var property = instance.GetType().GetProperty(propertyName,
                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) ??
                           throw new NullReferenceException();

            Func<object?> getValue = () => property.GetValue(instance);

            getValue.AssertTheSameValueButTheSecondTimeIsFaster();
        }
    }
}