using LoginForm.Services;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using System;

namespace QuestStoreNAT.web.Services
{
    public class LoginValidatorService : ILoginValidatorService
    {
        private IDB_GenericInterface<Credentials> _CredentialsDAO;
        private Role UserRole { get; set; }
        private int CredentialId { get; set; }

        public LoginValidatorService(IDB_GenericInterface<Credentials> credentialsDAO)
        {
            _CredentialsDAO = credentialsDAO ?? throw new ArgumentNullException(nameof(credentialsDAO)); 
        }

        public Role GetUserRole()
        {
            return UserRole;
        }

        public int GetUserCredentialId()
        {
            return CredentialId;
        }

        public bool IsValidPasswordHASH(Credentials enteredCredentials)
        {
            if (enteredCredentials == null) return false;
            if (enteredCredentials.Email == null) return false;
            if (enteredCredentials.Password == null) return false;

            Credentials userCredentialsInDb = _CredentialsDAO.FindOneRecordBy(enteredCredentials.Email);

            if (userCredentialsInDb == null) return false;
            if (userCredentialsInDb.Password == null) return false;
            if (userCredentialsInDb.SALT == null) return false;

            string passwordFromDb = userCredentialsInDb.Password;
            byte[] passwordFromForm = EncryptPassword.CreateHASH(enteredCredentials.Password, userCredentialsInDb.SALT);

            if (SlowEquals(passwordFromDb.ConvertStringToByte(), passwordFromForm))
            {
                SetUserRoleAndCredentialId(userCredentialsInDb);
                return true;
            }
            return false;
        }

        private void SetUserRoleAndCredentialId(Credentials userCredentialsInDb)
        {
            UserRole = userCredentialsInDb.Role;
            CredentialId = userCredentialsInDb.Id;
        }

        private bool SlowEquals(byte[] a, byte[] b)
        {
            //Byte comparison to prevent timing attacks
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}
