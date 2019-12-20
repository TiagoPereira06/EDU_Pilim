using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Si2_Fase2_EF
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new PilimEntities())
            {
                ExercicioF("FR0004548873"); //Aceder procedimento p_actualizaValorDiario
                //ExercicioG(ctx,"FR0004548873"); //CalcularMedia6Meses Instrumento
                //ExercicioH("FR0004548873"); //Actualizar dados instrumento
                Console.ReadLine();
            }
        }

        private static void ExercicioF(String isin)
        {
            Registo reg;
            var triplo = new Triplos()
            {
                Identificacao = isin,
                Dia = new DateTime(2019, 12, 28, 14, 3, 0),
                Valor = 500,
                Observado = false
            };
            using (var ctx = new PilimEntities())
            {
                ctx.Triplos.Add(triplo);
                ctx.SaveChanges();
                reg = ctx.Registo.Find(triplo.Identificacao, new DateTime(2019, 12, 20));
                if (reg != null) Console.WriteLine(reg.ToString());
                ctx.p_actualizaValorDiario();
                ctx.SaveChanges();
            }

            using (var ctx = new PilimEntities())
            {

                reg = ctx.Registo.Find(triplo.Identificacao, new DateTime(2019, 12, 20));
                Console.WriteLine("Procedure called");
                if (reg != null) Console.WriteLine(reg.ToString());
            }
        }

        private static void ExercicioG(PilimEntities ctx, string instrumentoIsin)
        {
            throw new NotImplementedException();
        }
        private static void ExercicioH(string isin)
        {
            Instrumento_Financeiro inst;
            using (var ctx = new PilimEntities())
            {
                Console.WriteLine("Instrumento Antes do Procedimento:");
                inst = ctx.Instrumento_Financeiro.Find(isin);
                if (inst != null) Console.WriteLine("Valor Actual do Instrumento<" + isin + "> :" + inst.ValorAtual);
                var novoTriplo = new Triplos()
                {
                    Identificacao = isin,
                    Dia = new DateTime(2019, 12, 28, 8, 0, 0),
                    Valor = 540,
                    Observado = false
                };
                ctx.Triplos.Add(novoTriplo);
                ctx.SaveChanges();
                Console.WriteLine("Criação de triplo");
                var rows = ctx.p_actualizaValorDiario();
                ctx.SaveChanges();
            }
            using (var ctx = new PilimEntities())
            {
                Console.WriteLine("Instrumento Depois do Procedimento:");
                inst = ctx.Instrumento_Financeiro.Find(isin);
                if (inst != null) Console.WriteLine("Valor Actual do Instrumento<" + isin + "> :" + inst.ValorAtual);
            }

        }
    }
}