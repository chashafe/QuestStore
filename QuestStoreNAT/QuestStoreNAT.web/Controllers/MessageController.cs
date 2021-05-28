using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Services;


namespace QuestStoreNAT.web.Controllers
{
    public class MessageController : Controller
    {
        private StudentDAO _studentDAO;
        private StudentAcceptanceDAO _studentAcceptanceDAO;
        private GroupTransactionDAO _groupTransactionDAO;
        private readonly ICurrentSession _session;
        private int _credentialID { get; set; }
        private ArtifactDAO _artifactDAO { get; set; }
        private Student _student { get; set; }

        public MessageController(ICurrentSession session)
        {
            _studentDAO = new StudentDAO();
            _groupTransactionDAO = new GroupTransactionDAO();
            _studentAcceptanceDAO = new StudentAcceptanceDAO();
            _session = session;
            _credentialID = _session.LoggedUser.CredentialID;
            _artifactDAO = new ArtifactDAO();
            _student = _studentDAO.FindOneRecordBy(_credentialID);
        }

        [HttpGet]
        public IActionResult Message()
        {
            var model = _studentAcceptanceDAO.FindOneRecordBy(_student.Id);
            if (model != null)
            {
                model.costArtifact = _artifactDAO.FindOneRecordBy(model.artifactID).Cost;
                model.artifactName = _artifactDAO.FindOneRecordBy(model.artifactID).Name;
                model.currentAmountOfAcceptance = _groupTransactionDAO.FindOneRecordBy(model.groupID).numberOfAcceptance;
                return View(model);
            }
            TempData["MessageX"] = $"You don't have any messages.";
            return RedirectToAction("Welcome", "Profile");
        }

        [HttpPost]
        public IActionResult Acceptance(string answear)
        {
            var currentStudentAcceptanceToUpdate = _studentAcceptanceDAO.FindOneRecordBy(_student.Id);
            var artifactToBuy = new ArtifactDAO().FindOneRecordBy(currentStudentAcceptanceToUpdate.artifactID);
            if (ModelState.IsValid)
            {
                new AcceptanceMenagement().StudentAcceptance(answear, _student.GroupID, artifactToBuy.Id,artifactToBuy.Cost, _student.Id);
                return RedirectToAction("ShowStudentProfile", "Profile");   
            }
            return RedirectToAction("Error", "Home");
        }
    }
}
