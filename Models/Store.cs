using System;

namespace mejor_precio_2.Models {
    public class Store
    {
        public Store(string name, string address, string latitude, string longitude, Price price, int state){
            this.Name = name;
            this.Address = address;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Price = price;
            this.State = state;
        }
        public Store(){}
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        
        public string Latitude { get; set; }
        
        public string Longitude { get; set; }

        public Price Price { get; set; }
        
        public int State { get; set; }
        
        public DateTime Date { get; set; }
        
    }
}