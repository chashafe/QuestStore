using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.ViewModels;
using QuestStoreNAT.web.Services;

namespace QuestStoreNAT.web.Controllers
{
    public class MentorController : Controller
    {
        private readonly ICurrentSession _session;
        private int _credentialID { get; set; }
        private readonly MentorDAO _mentorDAO;
        private readonly ClassEnrolmentDAO _classEnrolmentDAO;
        private readonly GroupDAO _groupDAO;
        private readonly StudentDAO _studentDAO;

        public MentorController(MentorDAO mentorDAO, ClassEnrolmentDAO classEnrolmentDAO, GroupDAO groupDAO, StudentDAO studentDAO, ICurrentSession session)
        {
            _mentorDAO = mentorDAO;
            _classEnrolmentDAO = classEnrolmentDAO;
            _groupDAO = groupDAO;
            _studentDAO = studentDAO;
            _session = session;
            _credentialID = _session.LoggedUser.CredentialID;
        }
        public IActionResult Index()
        {
            return View(_mentorDAO.FetchAllRecords().OrderBy(m=>m.Id).ToList());
        }

        public IActionResult Create(int id)
        {
            var Id = _mentorDAO.AddMentorByCredentialsReturningID(id);
            return RedirectToAction("Edit" , "Mentor" , new { Id = Id });
        }

        public IActionResult Edit(int Id)
        {
            var mentorToEdit = _mentorDAO.FindOneRecordBy(Id);
            return View(mentorToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Mentor Mentor)
        {
            _mentorDAO.UpdateRecord(Mentor);
            return RedirectToAction("Index", "Mentor");
        }

        public IActionResult Delete(int id)
        {
            return View(_mentorDAO.FetchAllRecords().SingleOrDefault(m=>m.Id == id));
        }

        [HttpPost]
        public IActionResult Delete(Mentor mentor)
        {
            _mentorDAO.DeleteRecord(mentor.Id);
            return RedirectToAction("Index", "Mentor");
        }

        public IActionResult Details( int id )
        {
            return View(GetMentorDetails(id));
        }

        public IActionResult ShowMentorProfile()
        {
            int mentorID = _mentorDAO.FindOneRecordByCredentialId(_credentialID).Id;
            return View(GetMentorDetails(mentorID));
        }

        #region priv
        private MentorDetailsViewModel GetMentorDetails(int id)
        {
            var mentor = _mentorDAO.FetchAllRecords().FirstOrDefault(m => m.Id == id);
            var mentorClassrooms = _classEnrolmentDAO.FetchAllRecordsJoin().Where(ce => ce.MentorCE.Id == id).Select(ce => ce.ClassroomCE).ToList();
            var mentorGroups = _groupDAO.FetchAllRecordsByIdJoin(id).GroupBy(g => g.Id).Select(g => g.FirstOrDefault()).ToList();
            var mentorStudents = _studentDAO.FetchAllRecordsByIdJoin(id);
            var mentorViewModel = new MentorDetailsViewModel
            {
                Mentor = mentor,
                Classrooms = mentorClassrooms,
                Groups = mentorGroups,
                Students = mentorStudents
            };
            return mentorViewModel;
        }
        #endregion
    }
}
