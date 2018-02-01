using System;

namespace mejor_precio_2.Models{
    public class Notification{
        
        public Guid Id { get; set; }
        
        public DateTime Date { get; set; }

        public int State { get; set; }
        
        public Price OldPrice { get; set; }

        public Price NewPrice { get; set; }
        
        public User User { get; set; }

        public Product Product { get; set; }
        
        public Store Store { get; set; }
    }
}