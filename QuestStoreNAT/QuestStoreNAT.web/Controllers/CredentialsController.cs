using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginForm.Services;
using Microsoft.AspNetCore.Mvc;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;

namespace QuestStoreNAT.web.Controllers
{
    public class CredentialsController : Controller
    {
        private readonly CredentialsDAO _credentialsDAO;

        public CredentialsController(CredentialsDAO credentialsDAO)
        {
            _credentialsDAO = credentialsDAO;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCredentials()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCredentialsAsAdmin([FromForm] Credentials newCredentials)
        {
            newCredentials.SALT = EncryptPassword.CreateSALT();
            string hashedPassword = Convert.ToBase64String(EncryptPassword.CreateHASH(newCredentials.Password, newCredentials.SALT));
            newCredentials.Password = hashedPassword;

            var id = _credentialsDAO.AddRecordReturningID(newCredentials);
            switch ( newCredentials.Role )
            {
                case Role.Admin:
                    return RedirectToAction("Index" , "Admin" , new { id });
                case Role.Mentor:
                    return RedirectToAction("Create" , "Mentor" , new { id });
                case Role.Student:
                    return RedirectToAction("Create" , "Student" , new { id });
                default:
                    return View();
            }
        }
    }
}
