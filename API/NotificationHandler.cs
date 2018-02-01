using System;
using System.Data;
using System.Data.SqlClient;
using mejor_precio_2.Persistance;
using mejor_precio_2.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace mejor_precio_2.API
{
    public class NotificationHandler
    {
        public Notification GetOneById(Guid id)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand(@"SELECT * FROM Notifications N
                                                    INNER JOIN Users U ON N.idUser=U.id
                                                    INNER JOIN Prices P ON N.idPrice=P.id
                                                    WHERE N.id=@id", con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                    cmd1.Parameters["@id"].Value = id;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var notification = Converter.getNotification(dr);
                            var idNewPrice = dr.GetGuid(dr.GetOrdinal("idPrice"));
                            notification.OldPrice = GetOldPrice(idNewPrice);
                            notification.Product = GetProductByIdPrice(idNewPrice);
                            notification.Store = GetStoreByIdPrice(idNewPrice);
                            return notification;
                        }
                    }
                }
            }
            throw new Exception("No existe ese id");
        }

        public bool EvaluatePriceIncoherence(Guid idProduct, Guid idStore, decimal newPrice)
        {
            var incoherence = false;
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand(@"SELECT price FROM Prices 
                                                    WHERE date=(SELECT MAX(date) FROM Prices
                                                        WHERE report='Verificado'
                                                            AND idProduct=@idProduct 
                                                            AND idStore=@idStore)", con1))
                {
                    cmd1.Parameters.Add("@idProduct", SqlDbType.UniqueIdentifier).Value = idProduct;
                    cmd1.Parameters.Add("@idStore", SqlDbType.UniqueIdentifier).Value = idStore;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        Console.WriteLine(dr.HasRows);
                        while (dr.Read())
                        {
                            var oldPrice = dr.GetDecimal(dr.GetOrdinal("price"));
                            if(newPrice>oldPrice*2){
                                incoherence = true;
                            }
                            break;
                        }
                    }
                }
            }
            return incoherence;
        }

        public void CreateNotification(Guid idPrice, Guid idUser){
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "INSERT INTO Notifications (id, idUser, idPrice, state)";
                insertString += "VALUES (@id, @idUser, @idPrice, 1)";
                Guid uuid = Guid.NewGuid();
                using(var cmd1 = new SqlCommand(insertString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = uuid;
                    cmd1.Parameters.Add("@idUser", SqlDbType.UniqueIdentifier).Value = idUser;
                    cmd1.Parameters.Add("@idPrice", SqlDbType.UniqueIdentifier).Value = idPrice;
                    cmd1.ExecuteNonQuery();
                }
            }
        }

        public List<Notification> GetAllNotifications(){
            var notificationList = new List<Notification>();
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand(@"SELECT * FROM Notifications N
                                                    INNER JOIN Users U ON N.idUser=U.id
                                                    INNER JOIN Prices P ON N.idPrice=P.id
                                                    WHERE N.state = 1", con1))
                {
                    using (var dr = cmd1.ExecuteReader())
                    {
                        Console.WriteLine(dr.HasRows);
                        while (dr.Read())
                        {
                            var notification = Converter.getNotification(dr);
                            var idNewPrice = dr.GetGuid(dr.GetOrdinal("idPrice"));
                            notification.OldPrice = GetOldPrice(idNewPrice);
                            notification.Product = GetProductByIdPrice(idNewPrice);
                            notification.Store = GetStoreByIdPrice(idNewPrice);
                            notificationList.Add(notification);
                        }
                    }
                }
            }
            return notificationList;
        }

        public Price GetOldPrice(Guid idNewPrice)
        {
            var oldPrice = new Price();
            Guid idProduct;
            Guid idStore;

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand(@"SELECT idProduct,idStore FROM Prices 
                                                    WHERE id=@idNewPrice", con1))
                {
                    cmd1.Parameters.Add("@idNewPrice", SqlDbType.UniqueIdentifier).Value = idNewPrice;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        Console.WriteLine(dr.HasRows);
                        while (dr.Read())
                        {
                            idProduct = dr.GetGuid(dr.GetOrdinal("idProduct"));
                            idStore = dr.GetGuid(dr.GetOrdinal("idStore"));
                        }
                    }
                }
            }

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand(@"SELECT * FROM Prices 
                                                    WHERE date=(SELECT MAX(date) FROM Prices
                                                        WHERE report='Verificado'
                                                            AND idProduct=@idProduct 
                                                            AND idStore=@idStore)", con1))
                {
                    cmd1.Parameters.Add("@idProduct", SqlDbType.UniqueIdentifier).Value = idProduct;
                    cmd1.Parameters.Add("@idStore", SqlDbType.UniqueIdentifier).Value = idStore;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        Console.WriteLine(dr.HasRows);
                        while (dr.Read())
                        {
                            oldPrice = Converter.getPrice(dr);
                        }
                    }
                }
            }
            return oldPrice;
        }

        public Product GetProductByIdPrice(Guid idPrice)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT pro.* from Products pro INNER JOIN Prices pri ON pri.idProduct=pro.id WHERE Pri.id=@id", con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                    cmd1.Parameters["@id"].Value = idPrice;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return Converter.getProduct(dr);
                        }
                    }
                }
            }

            throw new Exception("No existe ese id");
        }

        public Store GetStoreByIdPrice(Guid idPrice)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT s.* from Stores s INNER JOIN Prices pri ON pri.idStore=s.id WHERE Pri.id=@id", con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                    cmd1.Parameters["@id"].Value = idPrice;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return Converter.getStore(dr);
                        }
                    }
                }
            }

            throw new Exception("No existe ese id");
        }

        public string Delete(Guid id)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var deleteString = "UPDATE Notifications SET state=0 WHERE id=@id;";

                using(var cmd1 = new SqlCommand(deleteString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                    cmd1.ExecuteNonQuery();
                }

                
            }

            return "producto borrado";
        }
    }
}