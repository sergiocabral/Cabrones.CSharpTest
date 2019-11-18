 using System;
 using System.Collections.Generic;
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
        string Interface1Método();

        int Interface1Propriedade { get; set; }

        float PropriedadeConcorrente { get; set; }
    }

    internal interface IInterface2
    {
        string Interface2Método();

        int Interface2Propriedade { get; set; }

        int PropriedadeConcorrente { get; set; }
    }

    internal interface IInterface3 : IInterface1
    {
        string Interface3Método();

        int Interface3Propriedade { get; set; }

        new byte PropriedadeConcorrente { get; set; }
    }

    internal abstract class ClassePai : IInterface1, IInterface2
    {
        public string Interface1Método() => string.Empty;

        public int Interface1Propriedade { get; set; }
        public string Interface2Método() => string.Empty;

        public int Interface2Propriedade { get; set; }
        
        public int PropriedadeConcorrente { get; set; }

        float IInterface1.PropriedadeConcorrente { get; set; }

        public virtual string ClassePaiMétodoPúblicoInstância() => string.Empty;
        
        private string ClassePaiMétodoPrivateInstância() => string.Empty;

        public static string ClassePaiMétodoPúblicoEstático() => string.Empty;
        
        private static string ClassePaiMétodoPrivateEstático() => string.Empty;
        
        protected abstract string ClassePaiMétodoAbstrato();
        
        public abstract string ClassePaiMétodoAbstratoPúblico();
    }

    internal class ClasseFilha : ClassePai, IInterface3
    {
        public string Interface3Método() => string.Empty;

        public int Interface3Propriedade { get; set; }
        
        public new byte PropriedadeConcorrente { get; set; }

        protected override string ClassePaiMétodoAbstrato() => string.Empty;
        
        public override string ClassePaiMétodoAbstratoPúblico() => string.Empty;
        
        public int ClasseFilhaPropriedadePúblicaInstância { get; set; }
        
        public static int ClasseFilhaPropriedadePúblicaEstática { get; set; }
        
        private int ClasseFilhaPropriedadePrivadaInstância { get; set; }
        
        private static int ClasseFilhaPropriedadePrivadaEstática { get; set; }
    }

    internal class ClasseNeta : ClasseFilha
    {
        public int ClasseNetaPropriedadePúblicaInstância
        {
            set => throw new NotImplementedException();
        }

        private static int _classeNetaPropriedadePúblicaComCache;
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
                
                for (var i = 0; i < 1000000; i++)
                {
                    value = new Random(DateTime.Now.Millisecond).Next(1000, 1999);
                }

                return value;
            }
        }
        
        private static int _classeNetaPropriedadePúblicaComCacheEstática;
        public static int ClasseNetaPropriedadePúblicaComCacheEstática
        {
            get
            {
                if (_classeNetaPropriedadePúblicaComCacheEstática > 0) return _classeNetaPropriedadePúblicaComCacheEstática;
                return _classeNetaPropriedadePúblicaComCacheEstática = ClasseNetaPropriedadePúblicaSemCacheEstática;
            }
        }

        private int ClasseNetaPropriedadePrivadaInstância { get; set; }
        
        private static int ClasseNetaPropriedadePrivadaEstática { get; set; }

        public TTipo[] MétodoGeneric<TTipo>(string param1, TTipo param2, TTipo[] param3) => default;
        
        public static IDictionary<string[,],string[][][]> PropriedadeComplicada { get; set; }
    }

    internal class ClasseSozinha
    {
        
    }
}