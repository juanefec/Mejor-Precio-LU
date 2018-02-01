using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mejor_precio_2.Models{
    public class User{    
        public User(string name, string lastName, int age , string neighborhood, string gender, string mail, string password, int state){
            this.Name = name;
            this.LastName = lastName;
            this.Age = age;
            this.Neighborhood = neighborhood;
            this.Gender = gender;
            this.Mail = mail;
            this.Password = password;
            this.State = state;
        }    
        public User(){}

        public UserInfo UserInfoCreate () {
            var user = new UserInfo();
            user.Id = Id;
            user.Type = Type;
            user.Name = Name;
            user.LastName = LastName;
            user.Age = Age;
            user.Neighborhood = Neighborhood;
            user.Gender = Gender;
            user.Mail = Mail;
            user.State = State;
            return user;
        }
        
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }
    
        [Required]
        public int Age { get; set; }
        
        [Required]
        public string Neighborhood { get; set; }
        
        [Required]
        public string Gender { get; set; }

        [Required]
        public string Mail { get; set; }
        
        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public int State { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Type { get; set; }
        public List<Search> SearchList { get; set; }

    }
}