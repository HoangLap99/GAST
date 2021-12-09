using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using ProjectLibrary.Config;
using ProjectLibrary.Database;


namespace TeamplateHotel.Controllers
{
    public class EmailMarketingController : Controller
    {
        //
        // GET: /EmailMarketing/
        [HttpPost]
        public JsonResult SaveEmail(string Email)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    EmailMarketing checkEmail = new EmailMarketing();
                    if (db.EmailMarketings.ToList().Count > 0)
                    {
                         checkEmail = db.EmailMarketings.FirstOrDefault(a => a.Email == Email);
                    }
                    
                    if (checkEmail != null && checkEmail.Email != null)
                    {
                        return Json(new { success = false});
                        //return Redirect("/Contact/Messages?status=" + status);
                    }
                    EmailMarketing marketing = new EmailMarketing
                    {
                        Email = Email,
                        //Tel = "",
                       // Note = "",
                    };
                    db.EmailMarketings.InsertOnSubmit(marketing);
                    db.SubmitChanges();
                    //return Redirect("/Contact/Messages?status=" + status); ;
                    return Json(new { success = true ,Request="Chúng tôi sẽ liên lại trong 24h" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, Request = "This Email already exits" });
            }
        }
        

    }
}
