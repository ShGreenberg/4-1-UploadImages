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
        public ActionResult Index()
        {
            return View();
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
                Password = password
            });
            UploadedViewModel vm = new UploadedViewModel
            {
                Id = id,
                Password = password
            };
            var counts = new Dictionary<int, int>();
            return View(vm);
        }


    }
}