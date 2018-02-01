using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class RegisterViewModel : User
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        public string ConfirmPassword { get; set; }
    }
}