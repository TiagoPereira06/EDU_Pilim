using DAL_Specific;
using DALInterfaces;
using Entities;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Exercicio1
{
    public class ExerciciosADO
    {
        private static readonly string p_atualizaValor = "p_actualizaValorDiario";

        public static void ExercicioF(string cs, string isin, decimal valor, DateTime date)
        {
            using (var con = new SqlConnection(cs))
            {
                Triplo triplo = new Triplo()
                {
                    Identificacao = isin,
                    Dia = date,
                    Valor = valor
                };

                RegistoKey key = new RegistoKey(isin, date);


                using (var ts = new TransactionScope())
                {
                    IMapperTriplo map = new MapperTriplo();
                    IMapperRegisto registoMap = new MapperRegisto();
                    map.Create(triplo);

                    using (SqlCommand cmd = new SqlCommand(p_atualizaValor, con) { CommandType = CommandType.StoredProcedure })
                    {
                        try
                        {
                            con.Open();
                            Console.WriteLine("Chamada ao stored procedure");
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                    Console.WriteLine("Registo depois da chamada ao stored procedure");
                    Console.WriteLine(registoMap.Read(key).ToString());
                    ts.Complete();
                }
            }
        }

        public static void ExercicioG(string cs, string isin)
        {
            using (var con = new SqlConnection(cs))
            {
                using (var ts = new TransactionScope())
                {
                    Console.WriteLine("Informação de instrumento : ");

                    IMapperInstrumento mapper = new MapperInstrumento();
                    Console.WriteLine(mapper.Read(isin).ToString());

                    using (SqlCommand cmd = new SqlCommand("Media6Meses", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@isin", isin);
                        Console.WriteLine("Chamada à função que calcula a média");
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Informação de instrumento : ");
                    Console.WriteLine(mapper.Read(isin).ToString());

                    ts.Complete();
                }
            }
        }

        public static void ExercicioH(string cs, String isin, DateTime date, decimal val)
        {
            using (var con = new SqlConnection(cs))
            {
                using (var ts = new TransactionScope())
                {
                    IMapperInstrumento mapper = new MapperInstrumento();
                    IMapperTriplo mapperTriplo = new MapperTriplo();

                    Console.WriteLine("Instrumento Antes do Procedimento:");
                    Console.WriteLine(mapper.Read(isin).ToString());


                    Triplo triplo = new Triplo()
                    {
                        Identificacao = isin,
                        Dia = date,
                        Valor = val
                    };

                    Console.WriteLine("Criação de triplo");
                    mapperTriplo.Create(triplo);


                    using (SqlCommand cmd = new SqlCommand(p_atualizaValor, con) { CommandType = CommandType.StoredProcedure })
                    {
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                    Console.WriteLine("Instrumento Depois do Procedimento:");
                    Console.WriteLine(mapper.Read(isin).ToString());
                    ts.Complete();
                }
            }

        }

        public static void ExercicioI(string nomeP, string cc, string nif, string nomeCLiente)
        {
            Cliente cliente = new Cliente
            {
                CC = cc,
                NIF = nif,
                NomeCliente = nomeCLiente,
                NomePortfolio = nomeP,
            };

            IMapperCliente mapper = new MapperCliente();

            Console.WriteLine("Criação de um novo Cliente");
            mapper.Create(cliente);
            Console.WriteLine("Informação de cliente:" + mapper.Read(cc).ToString());
        }

        public static void ExercicioJ(string cc, string isin, int quantidade)
        {
            IMapperPosicao mapperPos = new MapperPosicao();
            IMapperCliente mapperCliente = new MapperCliente();

            Console.WriteLine("Informação de cliente:" + mapperCliente.Read(cc).ToString());

            var pos = mapperPos.Read(new PosicaoKey(isin, cc));
            pos.Quantidade += quantidade;

            mapperPos.Update(pos);
            Console.WriteLine("Informação de cliente:" + mapperCliente.Read(cc).ToString());
        }

        public static void ExercicioK(string cs, string nomePort)
        {
            using (var con = new SqlConnection(cs))
            {
                using (var ts = new TransactionScope())
                {
                    using (var command = con.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT * FROM listar_portfolio(@nomeP)";

                        command.Parameters.AddWithValue("@nomeP", nomePort);

                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine("Listagem do portfólio {0}", nomePort);
                            Console.WriteLine("ISDN: {0}", reader.GetString(0));
                            Console.WriteLine("Quantidade: {0}", (float)reader.GetDouble(1));
                            Console.WriteLine("Valor Atual: {0}", reader.GetSqlMoney(2));
                            Console.WriteLine("Percentagem de Variação em relação ao dia anterior: {0}", reader.GetSqlMoney(3));
                        }
                        reader.Close();
                    }
                    ts.Complete();
                }
            }
        }
    }
}
