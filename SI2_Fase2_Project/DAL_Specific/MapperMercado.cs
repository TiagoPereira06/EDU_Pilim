using DAL_Interfaces;
using Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace DAL_Specific
{
    class MapperMercado : IMapperMercado
    {
        private string cs;

        public MapperMercado()
        {
            cs = ConfigurationManager.ConnectionStrings["TL51N_11"].ConnectionString;
        }
        public void Create(Mercado mercado)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Mercado_Financeiro (Codigo, Nome, Descricao) VALUES (@codigo, @nome, @desc)";
                SqlParameter codigo = new SqlParameter("@codigo", mercado.Codigo);
                SqlParameter nome = new SqlParameter("@nome", mercado.Nome);
                SqlParameter desc = new SqlParameter("@desc", mercado.Descricao);
                command.Parameters.Add(codigo);
                command.Parameters.Add(nome);
                command.Parameters.Add(desc);

                using (var con = new SqlConnection(cs))
                {
                    command.Connection = con;

                    con.Open();

                    command.ExecuteNonQuery();

                }
                ts.Complete();
            }
        }

        public void Delete(Mercado entity)
        {
            throw new NotImplementedException();
        }

        public Mercado Read(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Mercado entity)
        {
            throw new NotImplementedException();
        }
    }
}
