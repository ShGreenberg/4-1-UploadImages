using _4_1_uploadimages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UploadImages.data;

namespace _4_1_uploadimages.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user, string password)
        {
            DbManager mgr = new DbManager(Properties.Settings.Default.ConStr);
            mgr.AddUser(user, password);
            return Redirect("/account/login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            DbManager mgr = new DbManager(Properties.Settings.Default.ConStr);
            User user = mgr.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Invalid login attempt";
                return Redirect("/Account/Login");
            }
            FormsAuthentication.SetAuthCookie(email, true);
            Session["CorrectPasswords"] = new List<int>();
            return Redirect("/");

        }

        [Authorize]
        public ActionResult MyAccount()
        {
            DbManager mgr = new DbManager(Properties.Settings.Default.ConStr);
            MyAccountViewModel vm = new MyAccountViewModel();
            vm.User = mgr.GetByEmail(User.Identity.Name);
            vm.Images = mgr.GetImgaesForUser(vm.User.Id);
            return View(vm);
        }
    }
}