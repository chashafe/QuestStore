using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;

namespace QuestStoreNAT.web.Controllers
{
    public class QuestController : Controller
    {
        private readonly ILogger<QuestController> _logger;
        private readonly Student _loggedStudent;
        private readonly ICurrentSession _session;
        private readonly IDB_GenericInterface<Quest> _questDAO;
        private readonly QuestManagement _questManager;

        public QuestController(
            ILogger<QuestController> logger,
            ICurrentSession session,
            IDB_GenericInterface<Quest> questDAO
            )
        {
            _logger = logger;
            _session = session;
            _loggedStudent = _session.LoggedUser as Student;
            _questDAO = questDAO;
            _questManager = new QuestManagement();
        }

        [HttpGet]
        public IActionResult ViewAllQuests()
        {
            var model = _questDAO.FetchAllRecords();
            if (model == null)
            {
                ViewBag.ErrorMessage = "Sorry, we could not retrieve the list of all Quests.";
                _logger.LogError($"Could not retrieve the Quests from Db.");
                return View($"Error");
            }
            return View($"ViewAllQuests", model);
        }

        [HttpGet]
        public IActionResult AddQuest()
        {
            return View($"AddQuest");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddQuest(Quest questToAdd)
        {
            if (ModelState.IsValid && questToAdd != null)
            {
                _questDAO.AddRecord(questToAdd);
                TempData["QuestMessage"] = $"You have successfully added the \"{questToAdd.Name}\" Quest!";
                return RedirectToAction($"ViewAllQuests", $"Quest");
            }
            Response.StatusCode = 406;
            ViewBag.ErrorMessage = "Sorry, adding new Quest failed.";
            _logger.LogError($"Could not add the Quest to Db. Quest was null or Invalid");
            return View($"Error");
        }

        [HttpGet]
        public IActionResult EditQuest(int id)
        {
            var model = _questDAO.FindOneRecordBy(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                ViewBag.ErrorMessage = "Sorry, you cannot edit this Quest!";
                return View($"NotFound", id);
            }
            return View($"EditQuest", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditQuest(Quest questToEdit)
        {
            if (ModelState.IsValid && questToEdit != null)
            {
                _questDAO.UpdateRecord(questToEdit);
                TempData["QuestMessage"] = $"You have updated the \"{questToEdit.Name}\" Quest!";
                return RedirectToAction($"ViewAllQuests", $"Quest");
            }
            Response.StatusCode = 406;
            ViewBag.ErrorMessage = "Sorry, editing this Quest failed.";
            _logger.LogError($"Could not add the Quest to Db. Quest was null or Invalid");
            return View($"Error");
        }

        [HttpGet]
        public IActionResult DeleteQuest(int id)
        {
            var model = _questDAO.FindOneRecordBy(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                ViewBag.ErrorMessage = "Sorry, you cannot delete this Quest!";
                return View($"NotFound", id);
            }
            return View($"DeleteQuest", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteQuest(Quest questToDelete)
        {
            if (questToDelete == null)
            {
                ViewBag.ErrorMessage = "Sorry, there was an error in communication.";
                return View($"Error");
            }

            var questToDeleteFromDb = _questDAO.FindOneRecordBy(questToDelete.Id);
            _questDAO.DeleteRecord(questToDelete.Id);
            TempData["QuestMessage"] = $"You have deleted the \"{questToDeleteFromDb.Name}\" Quest!";
            return RedirectToAction($"ViewAllQuests", $"Quest");
        }

        public IActionResult ClaimQuest(int id)
        {
            var claimedQuest = _questDAO.FindOneRecordBy(id);

            switch (claimedQuest.Type)
            {
                case TypeClassification.Individual:
                    var ownedIndividualQuest = new OwnedQuestStudent()
                    {
                        StudentId = _loggedStudent.Id,
                        QuestId = claimedQuest.Id,
                        CompletionStatus = CompletionStatus.Unfinished,
                    };
                    _questManager.ClaimIndividualQuest(ownedIndividualQuest);
                    break;
                case TypeClassification.Group:
                    var ownedGroupQuest = new OwnedQuestGroup()
                    {
                        GroupId = _loggedStudent.GroupID,
                        QuestId = claimedQuest.Id,
                        CompletionStatus = CompletionStatus.Unfinished,
                    };
                    _questManager.ClaimGroupQuest(ownedGroupQuest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            TempData["QuestMessage"] = $"You have claimed the \"{claimedQuest.Name}\" Quest!";
            return RedirectToAction($"ViewAllQuests", $"Quest");
        }

        public IActionResult DeclaimIndividualQuest(int id)
        {
            _questManager.DeclaimIndividualQuest(id);
            return RedirectToAction($"StudentQuestView", $"Quest");
        }
        public IActionResult DeclaimGroupQuest(int id)
        {
            _questManager.DeclaimGroupQuest(id);
            return RedirectToAction($"StudentQuestView", $"Quest");
        }

        public IActionResult StudentQuestView()
        {
            var model = _questManager.ReturnListOfAllQuest(_loggedStudent.Id, _loggedStudent.GroupID);
            if (model != null)
            {
                return View($"StudentQuestView", model);
            }
            TempData["Message"] = $"You don't have any quests";
            return RedirectToAction($"Welcome", $"Profile");
        }
    }
}
