using System;
namespace mejor_precio_2.Models {
    public class PriceInfo
    {
        public Guid Id { get; set; }

        public Product Product { get; set; }

        public Store Store { get; set; }

        public decimal Cost { get; set; }

        public string Report { get; set; }

        public DateTime Date { get; set; }
    }
}