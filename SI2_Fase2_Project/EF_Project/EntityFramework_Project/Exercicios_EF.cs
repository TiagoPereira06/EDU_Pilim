using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Si2_Fase2_EF
{
    public class CustomSqlException : Exception
    {
        public CustomSqlException() { }
        public CustomSqlException(string message, SqlException innerException) : base(message, innerException) { }
    }

    public class Exercicios_EF {
        public static void ExercicioF(String isin, decimal valor, DateTime data) 
        {
            Registo registo;
            Triplos triplo = new Triplos() {
                Identificacao = isin,
                Dia = data,
                Valor = valor,
                Observado = false
            };
            
            using (PilimEntities ctx = new PilimEntities())
            {
                CheckISINExists(ctx, isin);
                try {
                    ctx.Triplos.Add(triplo);
                    ctx.SaveChanges();
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro! Triplo criado ja existe. ", ex);
                }
                try {
                    registo = ctx.Registo.Find(triplo.Identificacao, data);
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro! Registo procurado nao existe. ", ex);
                }
                Console.WriteLine(registo.ToString());
                try {
                    ctx.p_actualizaValorDiario();
                    ctx.SaveChanges();
                    Console.WriteLine("Valor diario atualizado.");
                } catch (SqlException ex) {
                    throw new CustomSqlException("Erro ao atualizar valor diario ", ex);
                }
                
            }

            using (PilimEntities ctx = new PilimEntities())
            {
                try {
                    registo = ctx.Registo.Find(triplo.Identificacao, data);
                }
                catch (SqlException ex) {
                    throw new CustomSqlException("Erro! Registo criado ja existe. ", ex);
                }
                Console.WriteLine(registo.ToString());
            }
        }

        public static void ExercicioG(string isin)
        {
            using (PilimEntities ctx = new PilimEntities())
            {
                CheckISINExists(ctx, isin);
                
                //....TODO
            }
           
        }
        public static void ExercicioH(string isin, DateTime data, decimal valor)
        {
            Instrumento_Financeiro inst = null;
           
            using (PilimEntities ctx = new PilimEntities())
            {
                CheckISINExists(ctx, isin);
                Console.WriteLine("Valor Actual do Instrumento<{0}> : {1}", isin, inst.ValorAtual);
                
                Triplos novoTriplo = new Triplos()
                {
                    Identificacao = isin,
                    Dia = data,
                    Valor = valor,
                    Observado = false
                };
                try {
                    ctx.Triplos.Add(novoTriplo);
                    ctx.SaveChanges();
                    Console.WriteLine("Novo triplo criado.");
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro! Triplo criado ja existe. ", ex);
                }
                try {
                    ctx.p_actualizaValorDiario();
                    ctx.SaveChanges();
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro ao atualizar o valor diario.", ex);
                } 
            }
            
            using (PilimEntities ctx = new PilimEntities()) 
            {
                Console.WriteLine("Instrumento Depois do Procedimento:");
                inst = CheckISINExists(ctx, isin); 
                Console.WriteLine("Valor Actual do Instrumento<{0}> : {1}", isin, inst.ValorAtual);
            }
        }
        public static void ExercicioI(string nomePort, string isin, string cc, string nif, string nomeC, int valTot, int quant) {
            Cliente cliente = new Cliente() {
                CC = cc,                        //"123824538674",
                NIF = nif,                      //"156368578",
                NomeCliente = nomeC,            //"Eusebio Ferreira",
                NomePortfolio = nomePort,
                ValorTotalPortfolio = valTot    //5000
            };
            Posicao posicao = new Posicao() {
                ISIN = isin,        //"FR0013340973",
                CC = cc,            //"123824538674",
                Quantidade = quant  //120,
            };
            
            using (PilimEntities ctx = new PilimEntities())
            {
                ShowPortfolios(ctx);
                
                try {
                    ctx.Cliente.Add(cliente);
                    ctx.Posicao.Add(posicao);
                    ctx.SaveChanges();
                    Console.WriteLine("Novo portefolio adicionado");
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro! Portefolio inserido ja existe. ", ex);
                }
            }

            using (PilimEntities ctx = new PilimEntities())
            {
                ShowPortfolios(ctx);
            }
        }
        public static void ExercicioJ(string cc, string isin, double valor)
        {
            using (PilimEntities ctx = new PilimEntities())
            {
                Posicao posicao = null;
                try {
                    posicao = ctx.Posicao.Find(isin, cc);
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro! Posicao procurada inexistente. ", ex);
                }
                Console.WriteLine("Quantidade sobre a posicao com: ");
                Console.WriteLine("\n\tInstrumento: {0}\n\tCC: {1}\n\tQuantidade: {2}", isin, cc, posicao.Quantidade);
                try {
                    posicao.Quantidade += valor;
                    ctx.SaveChanges();
                    Console.WriteLine("Nova quantidade sobre a posicao com: ");
                    Console.WriteLine("\n\tInstrumento: {0}\n\tCC: {1}\n\tQuantidade: {2}", isin, cc, posicao.Quantidade);
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro ao adicionar quantidade a posicao. ", ex);
                }
            }   
        }

        public static void ExercicioK(string nomePort)
        {
            using (PilimEntities ctx = new PilimEntities()) 
            {
                try {
                    var portfolio = ctx.listar_portfolio(nomePort)
                        .Select(port => new {
                            port.isdn,
                            port.quantidade,
                            port.ValorAtual,
                            port.PercentagemVariacao
                        });
                    Console.WriteLine("Detalhes do portfolio " + nomePort);
                    foreach (var p in portfolio)
                    {
                        Console.WriteLine("ISDN: {0}\nQuantidade: {1}\nValor Atual: {2}\nPercentagem de Variacao: {3}",
                            p.isdn, p.quantidade, p.ValorAtual, p.PercentagemVariacao);
                    }
                } catch(SqlException ex) {
                    throw new CustomSqlException("Erro! Portfolio nao encontrado. ", ex);
                }
            }
        }

        private static void ShowPortfolios(PilimEntities ctx)
        {
            List<Cliente> clientes;
            try {
                clientes = ctx.Cliente.ToList();
            } catch (SqlException ex) {
                throw new CustomSqlException("Erro! A procura da lista de clientes falhou. ", ex);
            }
            Console.WriteLine("Portefólios existentes:");
            foreach (Cliente c in clientes) {
                Console.WriteLine(c.NomePortfolio);
            }
        }

        private static Instrumento_Financeiro CheckISINExists(PilimEntities ctx, string isin)
        {
            try {
                return ctx.Instrumento_Financeiro.Find(isin);
            } catch(SqlException ex) {
                throw new CustomSqlException("Erro! O instrumento indicado nao existe.", ex);
            }
        }
    }
}