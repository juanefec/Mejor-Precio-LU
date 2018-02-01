using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using mejor_precio_2.Models;
using mejor_precio_2.Persistance;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace mejor_precio_2.Persistance
{
    public class Converter
    {
        public static Product getProduct(SqlDataReader dr)
        {
            var product = new Product();
            //var foo = dr["id"];
            //var asString = foo.ToString();
            //var asGuid = Guid.Parse(asString)
            //product.Id = asGuid;

            product.Id = dr.GetGuid(dr.GetOrdinal("id"));
            product.Name = dr.GetString(dr.GetOrdinal("name"));
            product.Barcode = dr.GetString(dr.GetOrdinal("barcode"));
            product.State = dr.GetInt32(dr.GetOrdinal("state"));
            product.Route = dr.GetString(dr.GetOrdinal("route"));
            product.Date = dr.GetDateTime(dr.GetOrdinal("date"));
            return product;
        }

        public static ProductInfo getProductInfo(SqlDataReader dr)
        {
            var product = new ProductInfo();
            product.Id = dr.GetGuid(dr.GetOrdinal("id"));
            product.Name = dr.GetString(dr.GetOrdinal("name"));
            product.Barcode = dr.GetString(dr.GetOrdinal("barcode"));
            product.Route = dr.GetString(dr.GetOrdinal("route"));
            return product;
        }

        public static Store getStore(SqlDataReader dr){
            var store = new Store();
            store.Id = dr.GetGuid(dr.GetOrdinal("id"));
            store.Name = dr.GetString(dr.GetOrdinal("name"));
            store.Address = dr.GetString(dr.GetOrdinal("address"));
            store.Latitude = dr.GetString(dr.GetOrdinal("latitude"));
            store.Longitude = dr.GetString(dr.GetOrdinal("longitude"));
            store.State = dr.GetInt32(dr.GetOrdinal("state"));
            store.Date = dr.GetDateTime(dr.GetOrdinal("date"));
            return store;
        } 

        public static User getUser(SqlDataReader dr){
            var user = new User();
            user.Id = dr.GetGuid(dr.GetOrdinal("id"));
            user.Name = dr.GetString(dr.GetOrdinal("name"));
            user.LastName = dr.GetString(dr.GetOrdinal("lastname"));
            user.Age = dr.GetInt32(dr.GetOrdinal("age"));
            user.Neighborhood = dr.GetString(dr.GetOrdinal("neighborhood"));
            user.Gender = dr.GetString(dr.GetOrdinal("gender"));
            user.Mail = dr.GetString(dr.GetOrdinal("mail"));
            user.Password = dr.GetString(dr.GetOrdinal("password"));
            user.Salt = dr.GetString(dr.GetOrdinal("salt"));
            user.State = dr.GetInt32(dr.GetOrdinal("state"));
            user.Date = dr.GetDateTime(dr.GetOrdinal("date"));
            user.Type = dr.GetString(dr.GetOrdinal("type"));
            return user;
        }

        public static UserInfo getUserInfo(SqlDataReader dr){
            var user = new UserInfo();
            user.Id = dr.GetGuid(dr.GetOrdinal("id"));
            user.Name = dr.GetString(dr.GetOrdinal("name"));
            user.LastName = dr.GetString(dr.GetOrdinal("lastname"));
            user.Age = dr.GetInt32(dr.GetOrdinal("age"));
            user.Neighborhood = dr.GetString(dr.GetOrdinal("neighborhood"));
            user.Gender = dr.GetString(dr.GetOrdinal("gender"));
            user.Mail = dr.GetString(dr.GetOrdinal("mail"));
            user.State = dr.GetInt32(dr.GetOrdinal("state"));
            user.Date = dr.GetDateTime(dr.GetOrdinal("date"));
            user.Type = dr.GetString(dr.GetOrdinal("type"));
            return user;
        }
        public static UserApiAuth getUserAuthInfo(SqlDataReader dr){
            var user = new UserApiAuth();
           
            user.Name = dr.GetString(dr.GetOrdinal("name"));
            user.LastName = dr.GetString(dr.GetOrdinal("lastname"));
            user.Mail = dr.GetString(dr.GetOrdinal("mail"));
            user.Token = dr.GetGuid(dr.GetOrdinal("token"));
            user.Type = dr.GetString(dr.GetOrdinal("type"));
            return user;
        }

        public static Search getSearch(SqlDataReader dr){
            var search = new Search();
            search.Id = dr.GetGuid(dr.GetOrdinal("id"));
            search.Date = dr.GetDateTime(dr.GetOrdinal("date"));
            return search;
        }

        public static Notification getNotification(SqlDataReader dr){
            var notification = new Notification();
            notification.Id = dr.GetGuid(dr.GetOrdinal("id"));
            notification.Date = dr.GetDateTime(dr.GetOrdinal("date"));
            notification.State = dr.GetInt32(dr.GetOrdinal("state"));
            
            var newPrice = new Price();
            newPrice.Id = dr.GetGuid(dr.GetOrdinal("idPrice"));
            newPrice.Cost = dr.GetDecimal(dr.GetOrdinal("price"));
            newPrice.Report = dr.GetString(dr.GetOrdinal("report"));
            notification.NewPrice = newPrice;
            
            return notification;
        }

        public static Price getPrice(SqlDataReader dr){
            var price = new Price();
            price.Id = dr.GetGuid(dr.GetOrdinal("id"));
            price.Cost = dr.GetDecimal(dr.GetOrdinal("price"));
            price.Report = dr.GetString(dr.GetOrdinal("report"));
            return price;
        }
    }
}