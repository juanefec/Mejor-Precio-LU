using System;
namespace mejor_precio_2.Models {
    public class Price
    {
        public Guid Id { get; set; }
        
        public decimal Cost { get; set; }
        
        public string Report { get; set; }
    }
}