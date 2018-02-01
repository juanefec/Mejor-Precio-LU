using System;
namespace mejor_precio_2.Models {
    public class NewPriceData
    {
        public Guid Id { get; set; }

        public Guid IdProduct { get; set; }

        public Guid IdStore { get; set; }

        public Guid IdUser { get; set; }

        public decimal Cost { get; set; }

        public string Report { get; set; }
    }
}