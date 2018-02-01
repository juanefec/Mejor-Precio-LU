using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using mejor_precio_2.Models;
using mejor_precio_2.Persistance;

namespace mejor_precio_2.API
{
    public class RegisterHandler
    {        
        public bool RegisterUser(Guid token)
        {
            try
            {
                var id = GetUserId(token);

                using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
                {
                    con1.Open();
                    using (var cmd1 = new SqlCommand("UPDATE Users SET state=1 WHERE id=@id", con1))
                    {
                        cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                        cmd1.ExecuteNonQuery();
                        return true;

                    }
                }
            }
            catch{
                return false;

            }
        }
        public User GetUserByToken (Guid token) {
            var id = GetUserId(token);
            return new UserHandler().GetOneById(id);
        }
        public Guid GetUserId(Guid token)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT t.idUser, t.date from Tokens t WHERE t.token=@token", con1))
                {
                    cmd1.Parameters.Add("@token", SqlDbType.UniqueIdentifier).Value = token;
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {   
                            var date = dr.GetDateTime(dr.GetOrdinal("date"));
                            var now = DateTime.Now;
                            var period = now.Subtract(date);
                            if (period.TotalMinutes > 20){
                                throw new Exception();
                            }
                            return dr.GetGuid(dr.GetOrdinal("idUser"));
                        }
                    }
                }
            }
            throw new Exception();
        }
        public bool PassTokenCheck(Guid passToken)
        {
            try
            {
                var id = GetUserId(passToken);
                return true;
            }
            catch{
                return false;

            }
        }
    }
}