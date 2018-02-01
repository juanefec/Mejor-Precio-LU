using System;
using System.Collections.Generic;
using mejor_precio_2.Models;
using mejor_precio_2.Persistance;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace mejor_precio_2.API
{
    public class UserHandler
    {
        public List<UserInfo> GetAll()
        {
            var userList = new List<UserInfo>();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Users WHERE state = 1", con1))
                {
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            userList.Add(Converter.getUserInfo(dr));
                        }
                    }
                }
            }
            return userList;
        }
        public List<UserInfo> GetAllForVerify()
        {
            var userList = new List<UserInfo>();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Users", con1))
                {
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            userList.Add(Converter.getUserInfo(dr));
                        }
                    }
                }
            }
            return userList;
        }

        public List<UserApiAuth> getUsersTokens()
        {
            var userList = new List<UserApiAuth>();
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT u.name, u.type, t.token FROM User u INNER JOIN Tokens t ON u.id = t.userId AND t.type='apikey' WHERE state=1", con1))
                {
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            userList.Add(Converter.getUserAuthInfo(dr));
                        }
                    }
                }
            }
            return userList;

        }

        public User GetOneById(Guid id)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Users WHERE id=@id", con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return Converter.getUser(dr);
                        }
                    }
                }
            }
            throw new Exception("No existe usuario con ese id");
        }
        public User GetOneByEmail(string email)
        {
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("SELECT * from Users WHERE mail=@mail", con1))
                {
                    cmd1.Parameters.Add("@mail", SqlDbType.VarChar).Value = email;
                    using (var dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return Converter.getUser(dr);
                        }
                    }
                }
            }
            throw new Exception("No existe usuario con ese id");
        }

        public Guid Create(User userItem)
        {
            var loginHandler = new LoginHandler();
            if (loginHandler.MailAlreadyExists(userItem.Mail))
            {
                throw new Exception("El mail ingresado ya está registrado");
            }

            var salt = getSalt();
            var hash = getHash(userItem.Password, salt);
            Guid uuid = Guid.NewGuid();

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                var insertString = "INSERT INTO Users (id, name, lastname, age, neighborhood, gender, mail, password, salt, state, type)";
                insertString += "VALUES (@id, @name, @lastname, @age, @neighborhood, @gender, @mail, @password, @salt, @state, @type)";

                using (var cmd1 = new SqlCommand(insertString, con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = uuid;
                    cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = userItem.Name;
                    cmd1.Parameters.Add("@lastname", SqlDbType.VarChar).Value = userItem.LastName;
                    cmd1.Parameters.Add("@age", SqlDbType.Int).Value = userItem.Age;
                    cmd1.Parameters.Add("@neighborhood", SqlDbType.VarChar).Value = userItem.Neighborhood;
                    cmd1.Parameters.Add("@gender", SqlDbType.VarChar).Value = userItem.Gender;
                    cmd1.Parameters.Add("@mail", SqlDbType.VarChar).Value = userItem.Mail;
                    cmd1.Parameters.Add("@type", SqlDbType.VarChar).Value = userItem.Type;
                    cmd1.Parameters.Add("@password", SqlDbType.VarChar).Value = hash;
                    cmd1.Parameters.Add("@salt", SqlDbType.VarChar).Value = salt;
                    cmd1.Parameters.Add("@state", SqlDbType.Int).Value = userItem.State;
                    cmd1.ExecuteNonQuery();
                }
            }
            new MailHandler().SendRegisterMail(uuid, userItem.Mail);
            return uuid;

        }

        public string Update(User userItem)
        {
            var oldUser = new UserHandler().GetOneById(userItem.Id);
            var salt = "";
            var hash = "";

            if(!String.IsNullOrEmpty(userItem.Password)){
                salt = getSalt();
                hash = getHash(userItem.Password, salt);
            }else{
                salt = oldUser.Salt;
                hash = oldUser.Password;
            }

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var updateString = "UPDATE Users SET name=@name, lastname=@lastname, age=@age, neighborhood=@neighborhood, ";
                updateString += "gender=@gender, password=@password, salt=@salt WHERE id=@id";

                using (var cmd1 = new SqlCommand(updateString, con1))
                {
                    cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = userItem.Name;
                    cmd1.Parameters.Add("@lastname", SqlDbType.VarChar).Value = userItem.LastName;
                    cmd1.Parameters.Add("@age", SqlDbType.Int).Value = userItem.Age;
                    cmd1.Parameters.Add("@neighborhood", SqlDbType.VarChar).Value = userItem.Neighborhood;
                    cmd1.Parameters.Add("@gender", SqlDbType.VarChar).Value = userItem.Gender;
                    cmd1.Parameters.Add("@password", SqlDbType.VarChar).Value = hash;
                    cmd1.Parameters.Add("@salt", SqlDbType.VarChar).Value = salt;
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = userItem.Id;
                    cmd1.Parameters.Add("@type", SqlDbType.VarChar).Value = userItem.Type;
                    cmd1.ExecuteNonQuery();
                }
            }
            return "Usuario updateado: " + userItem.Name + " " + userItem.Mail;
        }
        public bool InfoUpdate(User userItem)
        {

            var loginHandler = new LoginHandler();
            if (userItem.Type == "Moderador")
            {
                if (loginHandler.VerifyCommonUser())
                {
                    throw new AccessViolationException();
                }
            }
            var prevUser = GetOneById(userItem.Id);
            if (userItem.Mail != prevUser.Mail)
            {
                new MailHandler().SendRegisterMail(prevUser.Id, userItem.Mail);
                userItem.State = 0;
            }
            else 
            {
                userItem.State = 1;
            }

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var updateString = "UPDATE Users SET name=@name, lastname=@lastname, age=@age, neighborhood=@neighborhood, ";
                updateString += "gender=@gender, mail=@mail, state=@state WHERE id=@id";

                using (var cmd1 = new SqlCommand(updateString, con1))
                {
                    cmd1.Parameters.Add("@name", SqlDbType.VarChar).Value = userItem.Name;
                    cmd1.Parameters.Add("@lastname", SqlDbType.VarChar).Value = userItem.LastName;
                    cmd1.Parameters.Add("@age", SqlDbType.Int).Value = userItem.Age;
                    cmd1.Parameters.Add("@neighborhood", SqlDbType.VarChar).Value = userItem.Neighborhood;
                    cmd1.Parameters.Add("@gender", SqlDbType.VarChar).Value = userItem.Gender;
                    cmd1.Parameters.Add("@mail", SqlDbType.VarChar).Value = userItem.Mail;
                    cmd1.Parameters.Add("@state", SqlDbType.Int).Value = userItem.State;
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = userItem.Id;
                    cmd1.ExecuteNonQuery();
                }
            }

            return Convert.ToBoolean(userItem.State);
        }

        public string Delete(Guid id)
        {
            var loginHandler = new LoginHandler();
            if (loginHandler.VerifyCommonUser())
            {
                var user = this.GetOneById(id);
                if (user.Id != loginHandler.loggedInUser().Id)
                {
                    throw new AccessViolationException();
                }
            }

            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();

                var deleteString = "UPDATE Users SET state=0 WHERE id=@id;";

                using (var cmd1 = new SqlCommand(deleteString, con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;

                    cmd1.ExecuteNonQuery();
                }
            }

            return "usuario borrado " + id;
        }

        public bool UpdatePassword(Guid id, string pass)
        {
            var salt = getSalt();
            Console.WriteLine(salt);
            var hash = getHash(pass, salt);
            var idUser = id;
            using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
            {
                con1.Open();
                using (var cmd1 = new SqlCommand("UPDATE Users SET salt=@salt, password=@password WHERE id=@id", con1))
                {
                    cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = idUser;
                    cmd1.Parameters.Add("@password", SqlDbType.VarChar).Value = hash;
                    cmd1.Parameters.Add("@salt", SqlDbType.VarChar).Value = salt;
                    cmd1.ExecuteNonQuery();
                    return true;

                }
            }
            throw new Exception();
        }

        public string getHash(string text, string salt)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text + salt));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public string getSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
