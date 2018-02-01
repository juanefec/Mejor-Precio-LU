using System;
using System.Collections.Generic;

namespace mejor_precio_2.Models{
    public class UserInfo{    
        public UserInfo(string name, string lastName, int age , string neighborhood, string gender, string mail, int state){
            this.Name = name;
            this.LastName = lastName;
            this.Age = age;
            this.Neighborhood = neighborhood;
            this.Gender = gender;
            this.Mail = mail;
            this.State = state;
        }

        public UserInfo(){}

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    
        public int Age { get; set; }
        
        public string Neighborhood { get; set; }
        
        public string Gender { get; set; }

        public string Mail { get; set; }

        public int State { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }
    }
}