using System.Collections.Generic;

using mejor_precio_2.Models;
using mejor_precio_2.Persistance;
using System.Data.SqlClient;
using System.Data;
using System;

namespace mejor_precio_2.API
{
    public class SearchHandler
    { 
        public Product GetFullProduct(Guid id)
        {
            var product = new ProductHandler().GetOneById(id);
            
           
            
            
            List<Store> storeList = new List<Store>();
            
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var queryProduct = @"SELECT * from Prices INNER JOIN Stores ON Prices.idStore = Stores.id 
                                    WHERE Prices.idProduct=@id AND Prices.report='Aceptado' ORDER BY price ASC";
    
                using (var cmd1 = new SqlCommand(queryProduct, con1))
                {
                    cmd1.Parameters.Add("@id",SqlDbType.UniqueIdentifier);
                    cmd1.Parameters["@id"].Value = id;
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {   
                            Store store = Converter.getStore(dr);
                            store.Price = new Price();
                            store.Price.Id = (Guid)dr.GetValue(dr.GetOrdinal("id"));
                            store.Price.Cost = (decimal)dr.GetValue(dr.GetOrdinal("price"));
                            store.Price.Report = (string)dr.GetValue(dr.GetOrdinal("report"));
                            storeList.Add(store);
                        }
                    }
                }
            }
            product.StoreList = storeList;
            return product;
        }

        public int CreateSearch(Guid idProduct, Guid idUser){

            var searchGuid = Guid.NewGuid();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "INSERT INTO Searches (id, idUser, idProduct)";
                insertString += "VALUES (@id, @idUser, @idProduct)";

                using(var cmd1 = new SqlCommand(insertString, con1)){
                    cmd1.Parameters.Add("@idUser",SqlDbType.UniqueIdentifier).Value = idUser;
                    cmd1.Parameters.Add("@idProduct",SqlDbType.UniqueIdentifier).Value = idProduct;
                    cmd1.Parameters.Add("@id",SqlDbType.UniqueIdentifier).Value = searchGuid;
                    return Convert.ToInt32(cmd1.ExecuteScalar());
                }
            }             
        }

        public List<Search> GetSearchRecord(Guid idUser){
            var searchList = new List<Search>();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var queryProduct =  @"SELECT TOP 5 * 
                                            FROM (SELECT *,
                                                    ROW_NUMBER() OVER(PARTITION BY idProduct ORDER BY ID DESC) rn
                                                    FROM Searches
                                                    WHERE idUser=@idUser) a
                                            WHERE rn=1";
                using (var cmd1 = new SqlCommand(queryProduct, con1))
                {
                    cmd1.Parameters.Add("@idUser",SqlDbType.UniqueIdentifier).Value = idUser;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {   
                            var search = Converter.getSearch(dr);
                            var idProduct = (Guid)dr.GetValue(dr.GetOrdinal("idProduct"));
                            search.Product = new ProductHandler().GetOneById(idProduct);
                            searchList.Add(search);
                        }
                    }
                }
            }
            return searchList;
        }
    }
}

