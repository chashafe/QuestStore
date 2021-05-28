using Microsoft.AspNetCore.Mvc;

namespace QuestStoreNAT.web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction($"Welcome", $"Profile");
        }
    }
}
