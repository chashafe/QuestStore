using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;

namespace QuestStoreNAT.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginValidatorService _loginValidatorService;
        private readonly ICurrentSession _session;
        private readonly IUserFinderService _userFinderService;

        public HomeController(ILogger<HomeController> logger, 
                              ILoginValidatorService loginValidatorService, 
                              ICurrentSession session,
                              IUserFinderService userFinderService)
        {
            _logger = logger;
            _loginValidatorService = loginValidatorService;
            _session = session;
            _userFinderService = userFinderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _session.LoggedUserRole = Role.None;
            _session.LoggedUser = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(Credentials enterdCredentials)
        {
            if (!_loginValidatorService.IsValidPasswordHASH(enterdCredentials)) return RedirectToAction("FailedLogin");
            
            _session.LoggedUserRole = _loginValidatorService.GetUserRole();
            var credentialId = _loginValidatorService.GetUserCredentialId();
            try
            {
                _session.LoggedUser = _userFinderService.RetrieveUser(_session.LoggedUserRole, credentialId);
                if (_session.LoggedUser.Equals(null))
                {
                    _logger.LogWarning($"Could not retrieve the user with CredentialId = {credentialId}. Such user does not exist.");
                    return RedirectToAction("FailedLogin");
                }
                if (_session.LoggedUserRole == Role.Mentor)
                {
                    var classEnrolmentDAO = new ClassEnrolmentDAO();
                    var mentorDetails = new MentorDetails(classEnrolmentDAO);
                    mentorDetails.GetMentorClassrooms((Mentor)_session.LoggedUser);
                }
                return RedirectToAction("Welcome", "Profile");
            }
            catch(ArgumentException e)
            {
                _logger.LogWarning(e.Message);
                return RedirectToAction("FailedLogin");
            }
        }

        [HttpGet]
        public IActionResult Contact()
        {
            //throw new Exception("Error in Contacts"); for testing ErrorController
            return View();
        }

        [HttpPost]
        public IActionResult Create(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                //Send us an Email or Store the message in the database
                TempData["Message"] = "You have sent us a message. Give us a minute and we will get back to you shortly.";
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult FailedLogin()
        {
            TempData["Message"] = "Login failed. Either e-mail or password was incorect. Try again or contact us.";
            return View("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
