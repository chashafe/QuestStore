using LoginForm.Services;
using QuestStoreNAT.web.DatabaseLayer;
using System;

namespace EncryptionPassword
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentialsDAO = new CredentialsDAO();
            var allCredentials = credentialsDAO.FetchAllRecords();

            foreach (var credential in allCredentials)
            {
                var salt = EncryptPassword.CreateSALT();

                Console.WriteLine($"{credential.Password} + " +
                                  $"{Convert.ToBase64String(EncryptPassword.CreateHASH(credential.Password, salt))} + " +
                                  $"{salt}");
            }
        }
    }
}
