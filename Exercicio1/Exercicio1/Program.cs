﻿using DALTriplosInterfaces;
using Entidades;
using SpecificDAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace Exercicio1
{
    class Program
    {
        static void Main(string[] args)
        {

            Triplo triplo = new Triplo()
            {
                //(int year, int month, int day, int hour, int minute, int second);
                Identificacao = "IE00BXC8D038",
                Dia = new DateTime(2019, 11, 4, 10, 30, 0),
                Valor = 420
            };

            using (var ts = new TransactionScope())
            {
                IMapperTriplo map = new MapperTriplo();
                map.Create(triplo);
                ts.Complete();
            }

            Console.ReadKey();


            //String storedProcedure = "p_actualizaValorDiario";
            //String query = "SELECT * FROM REGISTO;";

            /*
            SqlConnection con = new SqlConnection(new SqlConnectionStringBuilder()
            {
                DataSource = "10.62.73.95",
                InitialCatalog = "TL51N_11",
                UserID = "TL51N_11",
                Password = "qoM15wqBzt@R"
            }.ConnectionString
            );
            
            */
            /*
            using (con) {

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        //GET REGISTOS
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        int registos = 0;
                        while (reader.Read())
                        {
                            registos++;
                            Console.WriteLine("ISIN = " + reader.GetString(0) + ", DATE = " +
                                reader.GetDateTime(1) + ", Valor Abertura = " + reader.GetSqlMoney(2) +
                                "€ , Valor Fecho = " + reader.GetSqlMoney(3) + "€ , Valor Máximo = " + reader.GetSqlMoney(4)
                                + "€ , Valor Minimo = " + reader.GetSqlMoney(5) + "€, Hora de Fecho = " + reader.GetDateTime(6));
                        }
                        Console.WriteLine(registos + " Registos!");
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                        //CALL STORED PROCEDURE
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, con) { CommandType = System.Data.CommandType.StoredProcedure })
                    {
                        try
                        {
                            //con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        //GET REGISTOS
                        SqlDataReader reader = cmd.ExecuteReader();
                        int registos = 0;
                        while (reader.Read())
                        {
                            registos++;
                            Console.WriteLine("ISIN = " + reader.GetString(0) + ", DATE = " +
                                reader.GetDateTime(1) + ", Valor Abertura = " + reader.GetSqlMoney(2) +
                                "€ , Valor Fecho = " + reader.GetSqlMoney(3) + "€ , Valor Máximo = " + reader.GetSqlMoney(4)
                                + "€ , Valor Minimo = " + reader.GetSqlMoney(5) + "€, Hora de Fecho = " + reader.GetDateTime(6));
                        }
                        Console.WriteLine(registos + " Registos!");
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                con.Close();
            }
            Console.ReadKey();
        }

    

    }
    */


            /*
            Console.WriteLine("Calling function!");
            decimal val = CallMedia6Meses(con, "IE00BXC8D038");
            Console.WriteLine("Value returned = " + val);
            Console.ReadKey();
            */

            //InsertTriplos(con);
            //InsertTr
        }

        private static void InsertTriplos(SqlConnection con)
        {

        }

        public static decimal CallMedia6Meses(SqlConnection con, String isin)
    {
        // vai varrer a conexão
        using(con)
        {
            using (var command = con.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.Media6Meses";


                command.Parameters.AddWithValue("@isin", isin);

                SqlParameter returnValue = command.Parameters.Add("@RETURN_VALUE", SqlDbType.Money);
                returnValue.Direction = ParameterDirection.ReturnValue;

                con.Open();
                command.ExecuteScalar();    

                return (decimal)returnValue.Value;
            }
        }
    }

    }
}
