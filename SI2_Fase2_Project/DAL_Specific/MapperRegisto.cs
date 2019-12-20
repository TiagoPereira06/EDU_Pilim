using DAL_Interfaces;
using Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace DAL_Specific
{
    class MapperRegisto : IMapperRegisto
    {
        private string cs;

        public MapperRegisto()
        {
            cs = ConfigurationManager.ConnectionStrings["TL51N_11"].ConnectionString;
        }
        public void Create(Registo registo)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Required))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "INSERT INTO Registo (ISIN, Dia, ValorAbertura, ValorFecho, ValorMaximo, ValorMinimo) " +
                    "VALUES (@isin, @dia, @valabert, @valfecho, @valmax, @valmin)";
                SqlParameter isin = new SqlParameter("@isin", registo.Isin);
                SqlParameter dia = new SqlParameter("@dia", registo.Dia);
                SqlParameter valabert = new SqlParameter("@valabert", registo.ValorAbertura);
                SqlParameter valfecho = new SqlParameter("@valfecho", registo.ValorFecho);
                SqlParameter valmax = new SqlParameter("@valmax", registo.ValorMaximo);
                SqlParameter valmin = new SqlParameter("@valmin", registo.ValorMinimo);
                command.Parameters.Add(isin);
                command.Parameters.Add(dia);
                command.Parameters.Add(valabert);
                command.Parameters.Add(valfecho);
                command.Parameters.Add(valmax);
                command.Parameters.Add(valmin);

                using (var con = new SqlConnection(cs))
                {
                    command.Connection = con;

                    con.Open();

                    command.ExecuteNonQuery();

                }
                ts.Complete();
            }
        }

        public void Delete(Registo entity)
        {
            throw new NotImplementedException();
        }

        public Registo Read(RegistoKey id)
        {
            throw new NotImplementedException();
        }

        public void Update(Registo entity)
        {
            throw new NotImplementedException();
        }
    }
}
