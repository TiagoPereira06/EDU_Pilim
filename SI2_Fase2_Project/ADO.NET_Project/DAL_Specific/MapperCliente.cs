using DAL_Interfaces;
using Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace DAL_Specific
{
    class MapperCliente : IMapperCliente
    {
        private string cs;

        public MapperCliente()
        {
            cs = ConfigurationManager.ConnectionStrings["TL51N_11"].ConnectionString;
        }
        public void Create(Cliente cliente)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Cliente (CC, NIF, NomeCliente, NomePortfolio) VALUES (@cc, @nif, @nomecliente, @nomeport)";
                SqlParameter cc = new SqlParameter("@cc", cliente.CC);
                SqlParameter nif = new SqlParameter("@nif", cliente.NIF);
                SqlParameter nomecliente = new SqlParameter("@nomecliente", cliente.NomeCliente);
                SqlParameter nomeport = new SqlParameter("@nomeport", cliente.NomePortfolio);
                command.Parameters.Add(cc);
                command.Parameters.Add(nif);
                command.Parameters.Add(nomecliente);
                command.Parameters.Add(nomeport);

                using (var con = new SqlConnection(cs))
                {
                    command.Connection = con;

                    con.Open();

                    command.ExecuteNonQuery();

                }
                ts.Complete();
            }
        }

        public void Delete(Cliente entity)
        {
            throw new NotImplementedException();
        }

        public Cliente Read(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Cliente entity)
        {
            throw new NotImplementedException();
        }
    }
}
