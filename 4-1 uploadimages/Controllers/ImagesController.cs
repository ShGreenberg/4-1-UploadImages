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
            DbManager mgr = new DbManager(Properties.Settings.Default.ConStr);
            Image image = mgr.GetImage(id);
            if(image == null)
            {
                return Redirect("/");
            }
            if (password == image.Password || (fromSession != null && fromSession.Contains(id)) 
                    || image.UserId == mgr.GetByEmail(User.Identity.Name).Id)
            {
                mgr.UpdateImage(image.Id);
                image.TimesViewed++;
                List<int> correct = new List<int>();
                if (Session["CorrectPasswords"] != null)
                {
                    correct = (List<int>)Session["CorrectPasswords"];
                }
                correct.Add(id);
                Session["CorrectPasswords"] = correct;
                return View(image);
            }
            if (password == null)
            {
                return View(new Image { Id = id });
            }
            return View(new Image { Id = id, Password = "error" });

          

        }
    }
}