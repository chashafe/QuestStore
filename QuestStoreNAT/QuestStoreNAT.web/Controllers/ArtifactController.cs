using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;

namespace QuestStoreNAT.web.Controllers
{
    public class ArtifactController : Controller
    {
        private readonly StudentDAO _studentDAO;
        private Student Student { get; set; }
        private readonly ArtifactDAO _artifactDao;
        private readonly ICurrentSession _session;
        private int CredentialId { get; }
        public ArtifactManagement ArtifactManagmenet { get; set; }

        public ArtifactController(ICurrentSession session)
        {
            _artifactDao = new ArtifactDAO();
            _studentDAO = new StudentDAO();
            _session = session;
            CredentialId = _session.LoggedUser.CredentialID;
            ArtifactManagmenet = new ArtifactManagement();
            Student = _studentDAO.FindOneRecordBy(CredentialId);
        }

        [HttpGet]
        public IActionResult ViewAllArtifacts()
        {
            var model = _artifactDao.FetchAllRecords();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddArtifact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddArtifact(Artifact artifactToAdd)
        {
            if (ModelState.IsValid)
            {
                _artifactDao.AddRecord(artifactToAdd);
                TempData["ArtifactMessage"] = $"You have succesfully added the \"{artifactToAdd.Name}\" Artifact!";
                return RedirectToAction("ViewAllArtifacts", "Artifact");
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public IActionResult EditArtifact(int id)
        {
            var model = _artifactDao.FindOneRecordBy(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                ViewBag.ErrorMessage = "Sorry, you cannot edit this Artifact!";
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditArtifact(Artifact artifactToEdit)
        {
            if (ModelState.IsValid)
            {
                _artifactDao.UpdateRecord(artifactToEdit);
                TempData["ArtifactMessage"] = $"You have updated the \"{artifactToEdit.Name}\" Artifact!";
                return RedirectToAction("ViewAllArtifacts", "Artifact");
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public IActionResult DeleteArtifact(int id)
        {
            var model = _artifactDao.FindOneRecordBy(id);
            if (model == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteArtifact(Artifact artifactToDelete)
        {
            var questToDeleteFromDB = _artifactDao.FindOneRecordBy(artifactToDelete.Id);
            _artifactDao.DeleteRecord(questToDeleteFromDB.Id);
            TempData["ArtifactMessage"] = $"You have deleted the \"{artifactToDelete.Name}\" Artifact!";
            return RedirectToAction("ViewAllArtifacts", "Artifact");
        }

        [HttpGet]
        public IActionResult BuyArtifact(int id)
        {
            var model = _artifactDao.FindOneRecordBy(id);
            if (model == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult BuyArtifact(Artifact artifact)
        {
            var artifactInDb = _artifactDao.FindOneRecordBy(artifact.Id);

            if (new ArtifactManagement().CheckigStudentWallet(CredentialId, artifact.Id) == false)
            {
                TempData["ArtifactMessage"] = $"You don't have enough money. Sorry!";
            }
            else
            {
                new ArtifactManagement().BuyIndiviudalArtifact(CredentialId, artifact.Id);
                TempData["ArtifactMessage"] = $"You bought \"{artifactInDb.Name}\" Artifact!";
            }
            return RedirectToAction("ViewAllArtifacts", "Artifact");
        }

        public IActionResult BuyGroupArtifact(int id)
        {
            if (ArtifactManagmenet.CheckigGroupWallet(Student.GroupID, id, CredentialId) == false)
            {
                TempData["ArtifactMessage"] = $"Your group don't have enough money or maybe You don't have enough coolcoins to share costs. Sorry!";
            }
            else if(ArtifactManagmenet.CheckingIfTransactionForBoughtGroupArtifactExist(Student.GroupID) == false)
            {
                TempData["ArtifactMessage"] = $"Transactions for purchase the new group artifact exist. You can't make another transaction!";
            }
            else
            {
                ArtifactManagmenet.CreateNewGroupTransaction(id, Student.GroupID);
                ArtifactManagmenet.CreateRecordForAcceptance(CredentialId, Student.GroupID, id);
                TempData["ArtifactMessage"] = $"Your group will receive information!";
            }
            return RedirectToAction("ViewAllArtifacts", "Artifact");
        }
    }
}

