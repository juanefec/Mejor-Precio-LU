using System;
using System.Collections.Generic;
using System.Linq;
using mejor_precio_2.Models;
using mejor_precio_2.Persistance;
using System.Data.SqlClient;
using System.Data;
using Google.Maps;
using System.Drawing;
using Google.Maps.Geocoding;

namespace mejor_precio_2.API
{
    public class StoreHandler
    {
        public List<Store> GetAll()
        {
            var storeList = new List<Store>();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Stores WHERE state=1", con1))
                {
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            storeList.Add(Converter.getStore(dr));
                        }
                    }
                }
            }
            return storeList;
        }

        public Store GetOneById(Guid id)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Stores WHERE id=@id", con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return Converter.getStore(dr);
                        }
                    }
                }
            }
            throw new Exception("No encontrado");

        }

        public Guid Create(Store storeItem)
        {
            storeItem.Id = Guid.NewGuid();
            var coordinates = getCoordinates(storeItem.Address);
            storeItem.Latitude = coordinates[0];
            storeItem.Longitude = coordinates[1];
            var cityService = new CityService();
            var ubication = new PointF(float.Parse(storeItem.Latitude),float.Parse(storeItem.Longitude));
            if (!cityService.IsInBsAs(ubication)){
                throw new Exception("Los locales deben estar dentro de Capital Federal");
            }
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "INSERT INTO Stores (id, name, address, latitude, longitude, state)";
                insertString += "VALUES (@id, @name, @address, @latitude, @longitude, 1)";
                insertString += ";SELECT SCOPE_IDENTITY();";
                using(var cmd1 = new SqlCommand(insertString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = storeItem.Id;
                    cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = storeItem.Name;
                    cmd1.Parameters.Add("@address", SqlDbType.VarChar).Value = storeItem.Address;
                    cmd1.Parameters.Add("@latitude", SqlDbType.VarChar).Value = storeItem.Latitude;
                    cmd1.Parameters.Add("@longitude", SqlDbType.VarChar).Value = storeItem.Longitude;
                    cmd1.ExecuteNonQuery();
                    return storeItem.Id;
                }
            }
        }

        public string Update(Store storeItem)
        {
            var coordinates = getCoordinates(storeItem.Address);
            storeItem.Latitude = coordinates[0];
            storeItem.Longitude = coordinates[1];
            var cityService = new CityService();
            var ubication = new PointF(float.Parse(storeItem.Latitude),float.Parse(storeItem.Longitude));
            if (!cityService.IsInBsAs(ubication)){
                throw new Exception("Los locales deben estar dentro de Capital Federal");
            }
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var updateString = "UPDATE Stores SET name=@name, address=@address, latitude=@latitude, longitude=@longitude, state=@state WHERE id=@id";

                using(var cmd1 = new SqlCommand(updateString, con1)){
                    cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = storeItem.Name;
                    cmd1.Parameters.Add("@address", SqlDbType.VarChar).Value = storeItem.Address;
                    cmd1.Parameters.Add("@latitude", SqlDbType.VarChar).Value = storeItem.Latitude;
                    cmd1.Parameters.Add("@longitude", SqlDbType.VarChar).Value = storeItem.Longitude;
                    cmd1.Parameters.Add("@state", SqlDbType.Int).Value = storeItem.State;
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = storeItem.Id;

                    cmd1.ExecuteNonQuery();
                }
            }
            return "store updateado: " + storeItem.Address + " " + storeItem.Longitude;
        }

        public string Delete(Guid id)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var deleteString = "UPDATE Stores SET state=0 WHERE id=@id;";

                using(var cmd1 = new SqlCommand(deleteString, con1)){
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;

                    cmd1.ExecuteNonQuery();
                }

            }

            return "Store borrado";
        }
        public string[] getCoordinates(string address)
        {

            var coordinates = new string[2];

            //always need to use YOUR_API_KEY for requests.  Do this in App_Start.
            GoogleSigned.AssignAllServices(new GoogleSigned("AIzaSyDqDz8FA7VorskMFq1IL0py_p0q954dG1k"));

            var request = new GeocodingRequest();
            request.Address = address;
            var response = new GeocodingService().GetResponse(request);

            //The GeocodingService class submits the request to the API web service, and returns the
            //response strongly typed as a GeocodeResponse object which may contain zero, one or more results.

            //Assuming we received at least one result, let's get some of its properties:
            if (response.Status == ServiceResponseStatus.Ok && response.Results.Count() > 0)
            {
                var result = response.Results.First();

                //Console.WriteLine("Full Address: " + result.FormattedAddress);         // "1600 Pennsylvania Ave NW, Washington, DC 20500, USA"
                //Console.WriteLine("Latitude: " + result.Geometry.Location.Latitude);   // 38.8976633
                //Console.WriteLine("Longitude: " + result.Geometry.Location.Longitude); // -77.0365739
                //Console.WriteLine();

                coordinates[0] = result.Geometry.Location.Latitude.ToString();
                coordinates[1] = result.Geometry.Location.Longitude.ToString();
            }
            else
            {
                Console.WriteLine("Unable to geocode.  Status={0} and ErrorMessage={1}", response.Status, response.ErrorMessage);
            }

            return coordinates;
        }
    }
}
