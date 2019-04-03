using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4_1_uploadimages.Models;
using UploadImages.data;
namespace _4_1_uploadimages.Controllers
{
    public class ImagesController : Controller
    {

        public ActionResult ViewImage(int id, string password)
        {
            var fromSession = (List<int>)Session["CorrectPasswords"];
            if (password == null && (Session["CorrectPasswords"] == null || fromSession.FirstOrDefault(c => c == id) == 0))
            {
                return View(new Image { Id = id });
            }
            DbManager mgr = new DbManager(Properties.Settings.Default.ConStr);
            Image image = mgr.GetImage(id);

            if(image.Password != password)
            {
                return View(new Image { Id = id, Password = "error" });

            }
            mgr.UpdateImage(image.Id);
            image.TimesViewed++;

            List<int> correct = new List<int>();
            correct.Add(id);
            Session["CorrectPasswords"] = correct;
            return View(image);
        }
    }
}