using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace mejor_precio_2.Models {
    public class ProductInfo
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Barcode { get; set; }

        public string Route { get; set; }
        
    }
}