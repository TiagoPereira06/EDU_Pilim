using DALTriplosInterfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SpecificDAL
{
    public class MapperTriplo : IMapperTriplo
    {
        private string cs;

        public MapperTriplo()
        {
            cs = ConfigurationManager.ConnectionStrings["TL51N_11"].ConnectionString;
        }

        public void Create(Triplo triplo)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Triplos(Identificacao, Dia, Valor) VALUES (@isin, @dia, @valor)";
                SqlParameter isin = new SqlParameter("@isin", triplo.Identificacao);
                SqlParameter dia = new SqlParameter("@dia", triplo.Dia);
                SqlParameter valor = new SqlParameter("@valor", triplo.Valor);
                command.Parameters.Add(isin);
                command.Parameters.Add(dia);
                command.Parameters.Add(valor);

                using (var con = new SqlConnection(cs))
                {
                    command.Connection = con;

                    con.Open();

                    command.ExecuteNonQuery();

                }
                ts.Complete();
            }
        }

        public void Delete(Triplo triplos)
        {
            throw new NotImplementedException();
        }

        public Triplo Read(string isin, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void Update(Triplo triplos)
        {
            throw new NotImplementedException();
        }
    }
}
