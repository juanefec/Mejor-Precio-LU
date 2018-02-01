using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using mejor_precio_2.Models;

namespace mejor_precio_2.API
{
    public class LoginHandler
    {

        public bool CompareLoginCredentials(string email, string password)
        {
            List<UserInfo> userList = new UserHandler().GetAll();

            foreach (var item in userList)
            {
                var current = new UserHandler().GetOneById(item.Id);

                if (item.Mail == email)
                {
                    using (var sha256 = SHA256.Create())
                    {
                        Console.WriteLine(current.Salt);
                        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + current.Salt));
                        // Get the hashed string.  
                        var pwString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        Console.WriteLine(pwString);
                        if (current.Password == pwString)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool MailAlreadyExists(string mail)
        {
            List<UserInfo> userList = new UserHandler().GetAllForVerify();
            foreach (var item in userList)
            {
                if (item.Mail == mail)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetUserRol(string email)
        {
            var users = new UserHandler().GetAll();
            foreach (var user in users)
            {
                if (user.Mail == email)
                {
                    return user.Type;
                }
            }
            return null;
        }

        public User loggedInUser() //Se cambiara despues
        {
            var userHandler = new UserHandler();
            var user = userHandler.GetOneById(Guid.Parse("AB59D019-848F-4111-A88E-816CC30F5A97"));
            return user;
        }

        public Boolean VerifyCommonUser()
        {
            var user = loggedInUser();
            if (user.Type == "Common")
            {
                return true;
            }
            return false;
        }
    }
}


