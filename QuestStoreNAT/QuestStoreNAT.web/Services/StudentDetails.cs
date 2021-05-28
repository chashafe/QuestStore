using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.DatabaseLayer;

namespace QuestStoreNAT.web.Services
{
    public class StudentDetails
    {
        private StudentDAO _student { get; set; }
        private ArtifactDAO _artifact { get; set; }

        public StudentDetails()
        {
            _student = new StudentDAO();
            _artifact = new ArtifactDAO();
        }

        public Student ShowStudentDetails(int CredentialID)
        {
            var targetStudent = _student.FindOneRecordBy(CredentialID);
            targetStudent.level = new LevelStudent().levelStudent(targetStudent.OverallWalletLevel);
            targetStudent.StudentArtifacts = _artifact.FetchAllRecords(targetStudent.Id, (int)CompletionStatus.Unfinished);
            targetStudent.UsedStudentArtifacts = _artifact.FetchAllRecords(targetStudent.Id, (int)CompletionStatus.Finished);
            targetStudent.GroupArtifacts = _artifact.FetchAllGroupArtifacts(targetStudent.GroupID, (int)CompletionStatus.Unfinished);
            targetStudent.UsedGroupArtifacts = _artifact.FetchAllGroupArtifacts(targetStudent.GroupID, (int)CompletionStatus.Finished);
            return (targetStudent);
        }
    }
}
