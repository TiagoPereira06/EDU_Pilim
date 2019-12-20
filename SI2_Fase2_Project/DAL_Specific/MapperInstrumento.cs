using DAL_Interfaces;
using Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace DAL_Specific
{
    class MapperInstrumento : IMapperInstrumento
    {
        private string cs;

        public MapperInstrumento()
        {
            cs = ConfigurationManager.ConnectionStrings["TL51N_11"].ConnectionString;
        }
        public void Create(Instrumento instrumento)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Instrumento_Financeiro (ISIN, CodigoMercado, Descricao) VALUES (@isin, @codigo, @desc)";
                SqlParameter isin = new SqlParameter("@isin", instrumento.Isin);
                SqlParameter codigo = new SqlParameter("@codigo", instrumento.CodigoMercado);
                SqlParameter desc = new SqlParameter("@desc", instrumento.Descricao);
                command.Parameters.Add(isin);
                command.Parameters.Add(codigo);
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
        public void Delete(Instrumento entity)
        {
            throw new System.NotImplementedException();
        }

        public Instrumento Read(string id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Instrumento entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
