using System;
using System.Net.Http;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Data.SqlClient;
using System.Data;
using mejor_precio_2.Persistance;

namespace mejor_precio_2.API
{
    public class MailHandler
    {
        public void SendRegisterMail(Guid id, string email)
        {

            var token = Guid.NewGuid();
            
            SaveTokenInDataBase(id, token, "Register");
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential("losmejorespreciosarg@gmail.com", "passMejorPrecio17");

                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress("losmejorespreciosarg@gmail.com", "El Mejor Precio");
                mail.To.Add(new MailAddress(email));
                mail.Subject = "Verificacion";
                mail.Body = string.Format(@"Para terminar el registro ingrese a este mail: http://localhost:5000/Register?token={0}", token);
                smtpClient.Send(mail);
            }
            

        }
        

        private void SaveTokenInDataBase(Guid id, Guid token, string type)
        {
            var userHandler = new UserHandler();
            var users = userHandler.GetAllForVerify();
            foreach (var user in users)
            {
                if (user.Id == id)
                {
                    using (SqlConnection con1 = new SqlConnection(DBConnection.conStr))
                    {
                        con1.Open();
                        var insertString = "INSERT INTO Tokens (token, type, id, idUser)";
                        insertString += "VALUES (@token, @type, @id, @idUser);";
                        Guid uuid = Guid.NewGuid();
                        using (var cmd1 = new SqlCommand(insertString, con1))
                        {
                            cmd1.Parameters.Add("@token", SqlDbType.UniqueIdentifier).Value = token;
                            cmd1.Parameters.Add("@type", SqlDbType.VarChar).Value = type;
                            cmd1.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = uuid;
                            cmd1.Parameters.Add("@idUser", SqlDbType.UniqueIdentifier).Value = user.Id;
                            cmd1.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
       

        public void SendRecoveryMail(Guid id)
        {
            var email = new UserHandler().GetOneById(id).Mail;
            var token = Guid.NewGuid();
            SaveTokenInDataBase(id, token, "ChangePass");

            var loginHandler = new LoginHandler();
            if (!loginHandler.MailAlreadyExists(email))
            {
                throw new Exception("El mail ingresado no está registrado");
            }           

            
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new System.Net.NetworkCredential("losmejorespreciosarg@gmail.com", "passMejorPrecio17");
                smtpClient.EnableSsl = true;

                MailMessage mail = new MailMessage();                
                mail.From = new MailAddress("losmejorespreciosarg@gmail.com", "El Mejor Precio");
                mail.To.Add(new MailAddress(email));
                mail.Subject = "Password Managment";
                mail.Body = string.Format(@"Cambie su contraseña aqui: http://localhost:5000/Register/PassChanger?passToken={0}&email={1}", token, email);
                smtpClient.Send(mail);
            }
            
        }
    }
}