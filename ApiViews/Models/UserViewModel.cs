using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class UserViewModel : User
    {
        public UserViewModel(User user){
            this.Id = user.Id;
            this.Name = user.Name;
            this.LastName = user.LastName;
            this.Age = user.Age;
            this.Neighborhood = user.Neighborhood;
            this.Mail = user.Mail;
            this.Password = user.Password;
            this.Gender = user.Gender;
            this.Date = user.Date;
            this.Type = user.Type;
        }

        public UserViewModel(){}
    }
}