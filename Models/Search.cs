using System;

namespace mejor_precio_2.Models{
    public class Search{
        public Guid Id { get; set; }
        
        public DateTime Date { get; set; }
        
        public Product Product { get; set; }
    }
}