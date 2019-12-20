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

            //ExercicioF("FR0004548873"); //Aceder procedimento p_actualizaValorDiario
            //ExercicioG("FR0004548873"); //CalcularMedia6Meses Instrumento
            //ExercicioH("FR0004548873"); //Actualizar dados instrumento
            //ExercicioI("Description7");
            //ExercicioJ("125236521474", "FR0004548873", 200);
            ExercicioK("Description7");
            Console.ReadLine();
            
        }

        private static void ExercicioF(String isin)
        {
            Registo reg;
            var triplo = new Triplos()
            {
                Identificacao = isin,
                Dia = new DateTime(2019, 12, 28, 14, 6, 0),
                Valor = 600,
                Observado = false
            };
            using (var ctx = new PilimEntities())
            {
                ctx.Triplos.Add(triplo);
                ctx.SaveChanges();
                reg = ctx.Registo.Find(triplo.Identificacao, new DateTime(2019, 12, 28));
                if (reg != null) Console.WriteLine(reg.ToString());
                ctx.p_actualizaValorDiario();
                ctx.SaveChanges();
            }
            using (var ctx = new PilimEntities())
            {
                reg = ctx.Registo.Find(triplo.Identificacao, new DateTime(2019, 12, 28));
                Console.WriteLine("Procedure called");
                if (reg != null) Console.WriteLine(reg.ToString());
            }
        }

        private static void ExercicioG(string instrumentoIsin)
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
        private static void ExercicioI(string nomePort)
        {
            Cliente cliente = new Cliente()
            {
                CC = "123824538674",
                NIF = "156368578",
                NomeCliente = "Eusebio Ferreira",
                NomePortfolio = nomePort,
                ValorTotalPortfolio = 5000
            };
            Posicao posicao = new Posicao()
            {
                ISIN = "FR0013340973",
                CC = "123824538674",
                Quantidade = 120,
            };
            List<Cliente> clientes;
            using (var ctx = new PilimEntities())
            {
                clientes = ctx.Cliente.ToList();
                Console.WriteLine("Portefólios existentes:");
                foreach (Cliente c in clientes)
                {
                    Console.WriteLine(c.NomePortfolio);
                }
                ctx.Cliente.Add(cliente);
                ctx.Posicao.Add(posicao);
                ctx.SaveChanges();
            }
            Console.WriteLine("Novo portefolio adicionado");
            using (var ctx = new PilimEntities())
            {
                clientes = ctx.Cliente.ToList();
                Console.WriteLine("Portefólios existentes:");
                foreach (Cliente c in clientes)
                {
                    Console.WriteLine(c.NomePortfolio);
                }
            }
        }
        private static void ExercicioJ(string cc, string isin, double valor)
        {
            using (var ctx = new PilimEntities())
            {
                Posicao pos = ctx.Posicao.Find(isin, cc);
                
                Console.WriteLine("Quantidade sobre a posicao com: ");
                Console.WriteLine("\n\tInstrumento: " + isin + "\n\tCC: " + cc +"\n\tQuantidade: " + pos.Quantidade);
                
                pos.Quantidade += valor;
                ctx.SaveChanges();
                
                Console.WriteLine("Nova quantidade sobre a posicao com: ");
                Console.WriteLine("\n\tInstrumento: " + isin + "\n\tCC: " + cc + "\n\tQuantidade: " + pos.Quantidade);
            }
        }

        private static void ExercicioK(string nomePort)
        {
            using (var ctx = new PilimEntities())
            {
                var portfolio = ctx.listar_portfolio(nomePort)
                     .Select(port => new
                     {
                         port.isdn,
                         port.quantidade,
                         port.ValorAtual,
                         port.PercentagemVariacao
                     });
                Console.WriteLine("Detalhes do portfolio " + nomePort);
                foreach(var p in portfolio)
                {
                    Console.WriteLine("ISDN: {0}\nQuantidade: {1}\nValor Atual: {2}\nPercentagem de Variacao: {3}",
                        p.isdn, p.quantidade, p.ValorAtual, p.PercentagemVariacao);
                }
            }
        }
    }
}