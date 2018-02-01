using System;
using System.Collections.Generic;

namespace mejor_precio_2.Models{
    public class UserApiAuth{    
        public UserApiAuth(string name, string lastName, string mail){
            this.Name = name;
            this.LastName = lastName;            
            this.Mail = mail;
        }

        public UserApiAuth(){}
        
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Mail { get; set; }
        
        public string Type { get; set; }

        public Guid Token { get; set; }
    }
}