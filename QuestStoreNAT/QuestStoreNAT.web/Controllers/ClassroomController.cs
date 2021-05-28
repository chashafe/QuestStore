using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.Controllers
{

    public class ClassroomController : Controller
    {
        private readonly ClassroomDAO _classroomDAO;

        public ClassroomController( ClassroomDAO classroomDAO)
        {
            _classroomDAO = classroomDAO;
        }

        public IActionResult Index()
        {
            return View(_classroomDAO.FetchAllRecords().OrderBy(c => c.Id));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create( [FromForm] Classroom classroom )
        {
            _classroomDAO.AddRecord(classroom);
            return RedirectToAction("Index" , "Classroom");
        }

        public IActionResult Edit( int id )
        {
            var classroom = _classroomDAO.FindOneRecordBy(id);
            return View(classroom);
        }

        [HttpPost]
        public IActionResult Edit( Classroom classroom )
        {
            _classroomDAO.UpdateRecord(classroom);
            return RedirectToAction("Index" , "Classroom");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_classroomDAO.FindOneRecordBy(id));
        }
        [HttpPost]
        public IActionResult Delete( Classroom classroomToDelete)
        {
            _classroomDAO.DeleteRecord(classroomToDelete.Id);
            return RedirectToAction("Index" , "Classroom");
        }
    }
}
