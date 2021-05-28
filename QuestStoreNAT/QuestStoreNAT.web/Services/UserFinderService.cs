using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using System;

namespace QuestStoreNAT.web.Services
{
    public class UserFinderService : IUserFinderService
    {
        public IUser RetrieveUser(Role role, int credentialId)
        {
            if (credentialId < 0) throw new ArgumentException("Credential Id should not be less than 0.", "credentialId");

            switch (role)
            {
                case Role.Admin:
                    return FindAdminBy(credentialId);
                case Role.Mentor:
                    return FindMentorBy(credentialId);
                case Role.Student:
                    return FindStudentBy(credentialId);
                default:
                    return null;
            }
        }

        protected virtual IUser FindStudentBy(int credentialId)
        {
            var studentDAO = new StudentDAO();
            return studentDAO.FindOneRecordByCredentialId(credentialId);
        }

        protected virtual IUser FindMentorBy(int credentialId)
        {
            var mentorDAO = new MentorDAO();
            return mentorDAO.FindOneRecordByCredentialId(credentialId);
        }

        protected virtual IUser FindAdminBy(int credentialId)
        {
            var adminDAO = new AdminDAO();
            return adminDAO.FindOneRecordByCredentialId(credentialId);
        }
    }
}
