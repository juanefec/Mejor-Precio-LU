using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace mejor_precio_2.Models {
    public class Product
    {
        public Product(string name, string barcode, int state, List<Store> storeList, string route){
            this.Name = name;
            this.Barcode = barcode;
            this.State = state;
            this.StoreList = storeList;
            this.Route = route;
        }

        public Product(){}

        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Barcode { get; set; }

        public byte[] PhotoBytes { get; set; }

        public int State { get; set; }

        public List<Store> StoreList { get; set; }

        public string Route { get; set; }

        public DateTime Date { get; set; }
    }
}