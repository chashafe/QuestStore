using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;
using QuestStoreNAT.web.ViewModels;

namespace QuestStoreNAT.web.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentDAO _studentDAO;
        private readonly QuestDAO _questDAO;
        private readonly OwnedQuestStudentDAO _ownedQuestStudentDAO;
        private readonly ClassroomDAO _classroomDAO;
        private readonly GroupDAO _groupDAO;
        private static int studentId;

        public StudentController(StudentDAO studentDAO,
                                QuestDAO questDAO,
                                OwnedQuestStudentDAO ownedQuestStudentDAO,
                                ClassroomDAO classroomDAO,
                                GroupDAO groupDAO)
        {
            _studentDAO = studentDAO;
            _questDAO = questDAO;
            _ownedQuestStudentDAO = ownedQuestStudentDAO;
            _classroomDAO = classroomDAO;
            _groupDAO = groupDAO;
        }
        public IActionResult Index() => View(_studentDAO.FetchAllRecords().OrderBy(s => s.Id).ToList());

        public IActionResult Create(int id)
        {
            var Id = _studentDAO.AddStudentByCredentialsReturningID(id);
            return RedirectToAction("Edit" , "Student" , new { Id });
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            studentId = Id;
            var studentEditVM = PrepareEditModel(Id);
            return View(studentEditVM); 
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel studentVM)
        {
            if (ModelState.IsValid)
            {
                var student = _studentDAO.FetchAllRecords().SingleOrDefault(s => s.Id == studentVM.Id);

                student.Id = studentVM.Id;
                student.ClassID = studentVM.ClassID;
                student.GroupID = studentVM.GroupID;
                student.FirstName = studentVM.FirstName;
                student.LastName = studentVM.LastName;

                _studentDAO.UpdateRecord(student);
                return RedirectToAction("Index", "Student");
            }
            return View(studentVM);
        }

        public IActionResult Delete(int id)
        {
            var student = _studentDAO.FetchAllRecords().SingleOrDefault(s=>s.Id == id);
            if (student == null)
            {
                Response.StatusCode = 404;
                return View($"NotFound", id);
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult DeleteStudent([FromForm] Student student)
        {
            _studentDAO.DeleteRecord(student.Id);
            return RedirectToAction("Index" , "Student");
        }

        public IActionResult Details(int Id) => View(GetStudentViewModel(Id));


        public IActionResult Confirmation(int questId)
        {
            ConfirmQuestAndUpdateWallet(questId);
            return RedirectToAction("Details", "Student", new { Id = studentId });
        }



        #region priv

        private void ConfirmQuestAndUpdateWallet(int questId)
        {
            var questToConfirm = _ownedQuestStudentDAO.FetchAllRecords().SingleOrDefault(q => q.Id == questId);
            questToConfirm.CompletionStatus = CompletionStatus.Finished;
            _ownedQuestStudentDAO.UpdateRecord(questToConfirm);
            AddCoolCoinsIfAccomplished(questToConfirm);
        } 

        private StudentEditViewModel GetStudentViewModel(int Id)
        {
            studentId = Id;
            var student = _studentDAO.FetchAllRecords().SingleOrDefault(s => s.Id == studentId);
            var studentQuests = _ownedQuestStudentDAO.FetchAllRecords().Where(q => q.StudentId == student.Id).ToList();
            var quests = _questDAO.FetchAllRecords();

             var ownedStudentQuests = quests
                                         .Join(studentQuests, q => q.Id, s => s.QuestId, (q, s) => new OwnedQuestStudent
                                         { Id = s.Id, StudentId = s.StudentId, QuestId = s.QuestId, CompletionStatus = s.CompletionStatus, Name = q.Name, Cost = q.Cost })
                                          .OrderBy(q => q.CompletionStatus)
                                          .ToList();

            var classrooms = _classroomDAO.FetchAllRecords().Where(c => c.Id == student.ClassID).ToList();
            var groups = _groupDAO.FetchAllRecords().Where(g => g.Id == student.GroupID).ToList();


            var studentEVM = new StudentEditViewModel
            {
                Id = student.Id,
                CredentialID = student.CredentialID,
                ClassID = student.ClassID,
                GroupID = student.GroupID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Wallet = student.Wallet,
                Classrooms = classrooms,
                Groups = groups,
                OwnedStudentQuests = ownedStudentQuests
            };
            return studentEVM;
        }


        private StudentEditViewModel PrepareEditModel(int studentId)
        {
            var student = _studentDAO.FetchAllRecords().SingleOrDefault(s => s.Id == studentId);
            var allGroups = _groupDAO.FetchAllRecords().OrderBy(g => g.Name);
            var allClasses = _classroomDAO.FetchAllRecords().OrderBy(c => c.Name);
            return new StudentEditViewModel
            {
                Id = student.Id,
                CredentialID = student.CredentialID,
                ClassID = student.ClassID,
                GroupID = student.GroupID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Wallet = student.Wallet,
                Classrooms = allClasses,
                Groups = allGroups
            };
        }


        private void AddCoolCoinsIfAccomplished(OwnedQuestStudent ownedQuestStudent)
        {
            if (ownedQuestStudent.CompletionStatus == CompletionStatus.Finished)
            {
                var studentToUpdate = _studentDAO.FetchAllRecords().SingleOrDefault(s => s.Id == studentId);
                var studentGroupToBeUpdated = _groupDAO.FetchAllRecords().SingleOrDefault(g => g.Id == studentToUpdate.GroupID);
                var quest = _questDAO.FetchAllRecords().SingleOrDefault(q => q.Id == ownedQuestStudent.QuestId);

                studentToUpdate.Wallet += quest.Cost;
                studentToUpdate.OverallWalletLevel += quest.Cost;
                studentGroupToBeUpdated.GroupWallet += quest.Cost;

                _groupDAO.UpdateRecord(studentGroupToBeUpdated);
                _studentDAO.UpdateRecord(studentToUpdate);
            }
        }
        #endregion
    }
}
