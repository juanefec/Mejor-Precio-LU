using System;
using System.Data;
using System.Data.SqlClient;
using mejor_precio_2.Persistance;
using mejor_precio_2.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace mejor_precio_2.API
{
    public class PriceHandler
    {
        public List<PriceInfo> GetPriceDataList()
        {
            var priceDataList = new List<PriceInfo>();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var queryProduct = "SELECT * from Prices";
    
                using (var cmd1 = new SqlCommand(queryProduct, con1))
                {
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {   
                            var priceData = new PriceInfo();
                            priceData.Id = dr.GetGuid(dr.GetOrdinal("id"));
                            priceData.Cost = dr.GetDecimal(dr.GetOrdinal("price"));
                            priceData.Report = dr.GetString(dr.GetOrdinal("report"));
                            priceData.Date = dr.GetDateTime(dr.GetOrdinal("date"));
                            var idProduct = dr.GetGuid(dr.GetOrdinal("idProduct"));
                            priceData.Product = new ProductHandler().GetOneById(idProduct);
                            var idStore = dr.GetGuid(dr.GetOrdinal("idStore"));
                            priceData.Store = new StoreHandler().GetOneById(idStore);
                            priceDataList.Add(priceData);
                        }
                    }
                }
            }
            return priceDataList;
        }
        
         public PriceInfo GetOneById(Guid id)
        {   
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var queryProduct = "SELECT * from Prices WHERE id=@id";
    
                using (var cmd1 = new SqlCommand(queryProduct, con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {   
                            var priceData = new PriceInfo();
                            priceData.Id = dr.GetGuid(dr.GetOrdinal("id"));
                            priceData.Cost = dr.GetDecimal(dr.GetOrdinal("price"));
                            priceData.Report = dr.GetString(dr.GetOrdinal("report"));
                            priceData.Date = dr.GetDateTime(dr.GetOrdinal("date"));
                            var idProduct = dr.GetGuid(dr.GetOrdinal("idProduct"));
                            priceData.Product = new ProductHandler().GetOneById(idProduct);
                            var idStore = dr.GetGuid(dr.GetOrdinal("idStore"));
                            priceData.Store = new StoreHandler().GetOneById(idStore);
                            return priceData;
                        }
                    }
                }
            }
            throw new Exception("Error no existe ese id");
        }

        public Guid Create(NewPriceData data)
        {
            Guid guid = Guid.NewGuid();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "INSERT INTO Prices (id,idProduct, idStore, price, report)";
                insertString += "VALUES (@id, @idProduct, @idStore, @price, @report)";
                insertString += ";SELECT SCOPE_IDENTITY();";

                using(var cmd1 = new SqlCommand(insertString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = guid;
                    cmd1.Parameters.Add("@idProduct", SqlDbType.UniqueIdentifier).Value = data.IdProduct;
                    cmd1.Parameters.Add("@idStore", SqlDbType.UniqueIdentifier).Value = data.IdStore;
                    cmd1.Parameters.Add("@price", SqlDbType.Decimal).Value = data.Cost;
                    cmd1.Parameters.Add("@report", SqlDbType.VarChar).Value = data.Report;
                    cmd1.ExecuteNonQuery();
                }
            }

            if(new NotificationHandler().EvaluatePriceIncoherence(data.IdProduct,data.IdStore,data.Cost)){
                new NotificationHandler().CreateNotification(guid,data.IdUser);
            }
            return guid;
        }

        public Guid Update(Price data)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "UPDATE Prices SET report=@report WHERE id=@id;";

                using(var cmd1 = new SqlCommand(insertString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = data.Id;
                    cmd1.Parameters.Add("@report", SqlDbType.VarChar).Value = data.Report;
                    cmd1.ExecuteNonQuery();
                    return data.Id;
                }
            }
        }

        public Guid SetStateZeroToOldPrices(Price data)
        {
            var priceInfo = this.GetOneById(data.Id);

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "UPDATE Prices SET report='Rechazado' WHERE idProduct=@idProduct AND idStore=@idStore AND id<>@id;";

                using(var cmd1 = new SqlCommand(insertString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = data.Id;
                    cmd1.Parameters.Add("@idProduct", SqlDbType.UniqueIdentifier).Value = priceInfo.Product.Id;
                    cmd1.Parameters.Add("@idStore", SqlDbType.UniqueIdentifier).Value = priceInfo.Store.Id;
                    cmd1.ExecuteNonQuery();
                    return data.Id;
                }
            }
        }

        public string Delete(Guid id)
        {
            if (new LoginHandler().loggedInUser().Type != "Moderador")
            {
                throw new AccessViolationException();
            }
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var deleteString = "UPDATE Prices SET report='Borrado' WHERE id=@id;";

                using(var cmd1 = new SqlCommand(deleteString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                    cmd1.ExecuteNonQuery();
                }
            }
            return "Price borrado";
        }
    }
}