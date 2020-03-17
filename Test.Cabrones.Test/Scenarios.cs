using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable MemberCanBePrivate.Global

namespace Cabrones.Test
{
    // Este arquivo agrupa cenários envolvendo relacionados entre classes e interface para realização de testes.

    internal enum EnumList
    {
        EnumItem1 = 1,
        EnumItem2 = 2,
        EnumItem3 = 3,
        EnumItem4 = 4
    }

    internal interface IInterface1
    {
        int Interface1Propriedade { get; set; }

        float PropriedadeConcorrente { get; set; }
        string Interface1Método();
    }

    internal interface IInterface2
    {
        int Interface2Propriedade { get; set; }

        int PropriedadeConcorrente { get; set; }
        string Interface2Método();
    }

    internal interface IInterface3 : IInterface1
    {
        int Interface3Propriedade { get; set; }

        new byte PropriedadeConcorrente { get; set; }
        string Interface3Método();
    }

    [ExcludeFromCodeCoverage]
    internal abstract class ClassePai : IInterface1, IInterface2
    {
        public string Interface1Método()
        {
            return string.Empty;
        }

        public int Interface1Propriedade { get; set; }

        float IInterface1.PropriedadeConcorrente { get; set; }

        public string Interface2Método()
        {
            return string.Empty;
        }

        public int Interface2Propriedade { get; set; }

        public int PropriedadeConcorrente { get; set; }

        public virtual string ClassePaiMétodoPúblicoInstância()
        {
            return string.Empty;
        }

        private string ClassePaiMétodoPrivateInstância()
        {
            return string.Empty;
        }

        public static string ClassePaiMétodoPúblicoEstático()
        {
            return string.Empty;
        }

        private static string ClassePaiMétodoPrivateEstático()
        {
            return string.Empty;
        }

        protected abstract string ClassePaiMétodoAbstrato();

        public abstract string ClassePaiMétodoAbstratoPúblico();
    }

    [ExcludeFromCodeCoverage]
    internal class ClasseFilha : ClassePai, IInterface3
    {
        public int ClasseFilhaPropriedadePúblicaInstância { get; set; }

        public static int ClasseFilhaPropriedadePúblicaEstática { get; set; }

        private int ClasseFilhaPropriedadePrivadaInstância { get; set; }

        private static int ClasseFilhaPropriedadePrivadaEstática { get; set; }

        public string Interface3Método()
        {
            return string.Empty;
        }

        public int Interface3Propriedade { get; set; }

        public new byte PropriedadeConcorrente { get; set; }

        protected override string ClassePaiMétodoAbstrato()
        {
            return string.Empty;
        }

        public override string ClassePaiMétodoAbstratoPúblico()
        {
            return string.Empty;
        }
    }

    [ExcludeFromCodeCoverage]
    internal class ClasseNeta : ClasseFilha
    {
        private static int _classeNetaPropriedadePúblicaComCache;

        private static int _classeNetaPropriedadePúblicaComCacheEstática;

        public int ClasseNetaPropriedadePúblicaInstância
        {
            set => throw new NotImplementedException();
        }

        public int ClasseNetaPropriedadePúblicaComCache
        {
            get
            {
                if (_classeNetaPropriedadePúblicaComCache > 0) return _classeNetaPropriedadePúblicaComCache;
                return _classeNetaPropriedadePúblicaComCache = ClasseNetaPropriedadePúblicaSemCache;
            }
        }

        public int ClasseNetaPropriedadePúblicaSemCache => ClasseNetaPropriedadePúblicaSemCacheEstática;

        public static int ClasseNetaPropriedadePúblicaSemCacheEstática
        {
            get
            {
                var value = 0;

                for (var i = 0; i < 1000000; i++) value = new Random(DateTime.Now.Millisecond).Next(1000, 1999);

                return value;
            }
        }

        public static int ClasseNetaPropriedadePúblicaComCacheEstática
        {
            get
            {
                if (_classeNetaPropriedadePúblicaComCacheEstática > 0)
                    return _classeNetaPropriedadePúblicaComCacheEstática;
                return _classeNetaPropriedadePúblicaComCacheEstática = ClasseNetaPropriedadePúblicaSemCacheEstática;
            }
        }

        private int ClasseNetaPropriedadePrivadaInstância { get; set; }

        private static int ClasseNetaPropriedadePrivadaEstática { get; set; }

        public static IDictionary<string[,], string[][][]> PropriedadeComplicada { get; set; }

        public TTipo[] MétodoGeneric<TTipo>(string param1, TTipo param2, TTipo[] param3)
        {
            return default;
        }
    }

    [ExcludeFromCodeCoverage]
    internal class ClasseSozinha
    {
    }

    [ExcludeFromCodeCoverage]
    internal static class ClasseEstática
    {
        public static DateTime Agora()
        {
            return DateTime.Now;
        }
    }
}