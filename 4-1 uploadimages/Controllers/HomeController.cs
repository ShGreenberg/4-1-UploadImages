using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadImages.data;
using System.IO;
using _4_1_uploadimages.Models;
namespace _4_1_uploadimages.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            DbManager mgr = new DbManager(Properties.Settings.Default.ConStr);
            User user = mgr.GetByEmail(User.Identity.Name);
            TempData["userId"] = user;
            return View(user);
        }

        [HttpPost]
        public ActionResult Upload(string password, HttpPostedFileBase image)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            string fullPath = $"{Server.MapPath("/UploadedImages")}\\{fileName}";
            image.SaveAs(fullPath);
            DbManager mgr = new DbManager(Properties.Settings.Default.ConStr);
            int id = mgr.AddImage(new Image
            {
                FileName = fileName,
                Password = password,
                UserId = int.Parse(TempData["userId"].ToString())
            });
            UploadedViewModel vm = new UploadedViewModel
            {
                Id = id,
                Password = password
            };
            return View(vm);
        }


    }
}