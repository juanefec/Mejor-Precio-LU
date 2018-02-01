using System;
using System.Collections.Generic;
using mejor_precio_2.Models;
using mejor_precio_2.Persistance;
using System.Data.SqlClient;
using System.Data;
using ZXing;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace mejor_precio_2.API
{
    public class ProductHandler
    {
        public List<ProductInfo> GetAll()
        {
            var productList = new List<ProductInfo>();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Products WHERE state = 1", con1))
                {
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            productList.Add(Converter.getProductInfo(dr));
                        }
                    }
                }
            }
            return productList;
        }

        public List<ProductInfo> GetAllByName(string nombre)
        {
            var productList = new List<ProductInfo>();
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Products WHERE name LIKE @wordString AND state=1", con1))
                {
                    cmd1.Parameters.Add("@wordString", SqlDbType.VarChar);
                    cmd1.Parameters["@wordString"].Value = "%" + nombre + "%";

                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            productList.Add(Converter.getProductInfo(dr));
                        }
                    }
                }
            }
            return productList;
        }

        public Product GetOneById(Guid id)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Products WHERE id=@id", con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier);
                    cmd1.Parameters["@id"].Value = id;

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

        public string GetNameByBarcode(string barcode)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT name from Products WHERE barcode=@barcode", con1))
                {
                    cmd1.Parameters.Add("@barcode", SqlDbType.VarChar);
                    cmd1.Parameters["@barcode"].Value = barcode;

                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return dr.GetString(dr.GetOrdinal("name"));
                        }
                    }
                }
            }

            throw new Exception("No existe ese id");
        }

        public Guid Create(Product productItem)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "INSERT INTO Products (id, name, barcode, image_name, route, state)";
                insertString += "VALUES (@id, @name, @barcode, @image_name, @route, 1)";
                insertString += ";SELECT SCOPE_IDENTITY();";
                Guid uuid = Guid.NewGuid();
                using(var cmd1 = new SqlCommand(insertString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = uuid;
                    cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = productItem.Name;
                    cmd1.Parameters.Add("@barcode", SqlDbType.VarChar).Value = productItem.Barcode;
                    cmd1.Parameters.Add("@image_name", SqlDbType.VarChar).Value = "hola";
                    cmd1.Parameters.Add("@route",SqlDbType.VarChar).Value = productItem.Route;
                    cmd1.ExecuteNonQuery();
                    return uuid;
                }
            }
        }

        public string Update(Product productItem)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var updateString = "UPDATE Products SET name=@name, barcode=@barcode WHERE id=@id";


                using(var cmd1 = new SqlCommand(updateString, con1)){
                    cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = productItem.Name;
                    cmd1.Parameters.Add("@barcode", SqlDbType.VarChar).Value = productItem.Barcode;
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = productItem.Id;

                    Console.WriteLine(updateString);

                    cmd1.ExecuteNonQuery();
                }
            }
            return "Usuario updateado: " + productItem.Name + " " + productItem.Barcode;
        }

        public string Delete(Guid id)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var deleteString = "UPDATE Products SET state=0 WHERE id=@id;";

                using(var cmd1 = new SqlCommand(deleteString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                    cmd1.ExecuteNonQuery();
                }

                
            }

            return "producto borrado";
        }

        public string GetBarcodeFromImage(IFormFile file)
        {
            string decoded = null;
            Bitmap barcode = null;
            try
            {
            var barcodeReader = new BarcodeReader();
            barcodeReader.Options.TryHarder = true;

            barcode = (Bitmap)Bitmap.FromStream(file.OpenReadStream()); 

            decoded = barcodeReader.Decode(barcode).Text;
            }catch (Exception ex){

                return ex.Message ;
            }
            finally
            {
                if (barcode != null) {
                    barcode.Dispose();
                }
            }
            return decoded;
        }

        
    }
}