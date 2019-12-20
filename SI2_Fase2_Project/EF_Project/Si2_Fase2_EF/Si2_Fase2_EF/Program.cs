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
            Triplos triplos = new Triplos()
            {
                Identificacao = "US0253985401",
                Dia = new DateTime(2019, 12, 20, 13, 2, 0),
                Valor = 500,
                Observado = false
            };
            using (var ctx = new PilimEntities())
            {
                ctx.Triplos.Add(triplos);
                ctx.SaveChanges();

                /*ctx.Triplos.Attach(triplos);
                ctx.Triplos.Remove(triplos);
                ctx.SaveChanges();*/
                Registo reg = ctx.Registo.Find(triplos.Identificacao, new DateTime(2019, 12, 20));
                Console.WriteLine(reg.ToString());
                ctx.p_actualizaValorDiario();
                ctx.SaveChanges();
                Registo regAfter = ctx.Registo.Find(triplos.Identificacao, new DateTime(2019, 12, 20));
                Console.WriteLine("Procedure called");
                Console.WriteLine(regAfter.ToString());
                Console.ReadLine();
            }
        }
    }
}
