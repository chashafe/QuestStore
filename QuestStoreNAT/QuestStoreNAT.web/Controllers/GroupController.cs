using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;

namespace QuestStoreNAT.web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IDB_GenericInterface<Group> _groupDAO;

        public GroupController(IDB_GenericInterface<Group> groupDAO)
        {
            _groupDAO = groupDAO;
        }

        public IActionResult Index()
        {
            return View(_groupDAO.FetchAllRecords().OrderBy(g => g.Name));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm] Group group)
        {
            group.GroupWallet = 0;
            _groupDAO.AddRecord(group);
            return RedirectToAction("Index", "Group");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var group = _groupDAO.FindOneRecordBy(id);
            return View(group);
        }
        [HttpPost]
        public IActionResult Edit(Group group)
        {
            _groupDAO.UpdateRecord(group);
            return RedirectToAction("Index", "Group");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_groupDAO.FindOneRecordBy(id));
        }
        [HttpPost]
        public IActionResult Delete(Group groupToDelete)
        {
            _groupDAO.DeleteRecord(groupToDelete.Id);
            return RedirectToAction("Index", "Group");
        }
    }
}
