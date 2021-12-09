using System;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Security;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class MarketingController : BaseController
    {
        public object ModulePermission { get; private set; }

        public ActionResult Index()
        { 
            ViewBag.Title = "Email marketing";
            return View();
        }
        [HttpPost]
        public JsonResult List(int jtStartIndex = 0,int role = 0,  int jtPageSize = 0, string jtSorting = null)
        {
            //End check
            try
            {
                var db = new MyDbDataContext();

                var list = db.EmailMarketings.ToList();
                //Return result to jTable
                return Json(new { Result = "OK", Records = list, TotalRecordCount = list.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", message = ex.Message });
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(EmailMarketing model)
        {
            //End check
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var emailmarketing = new EmailMarketing
                        {
                            Email = model.Email,
                           
                     
                        };
                        db.EmailMarketings.InsertOnSubmit(emailmarketing);
                        db.SubmitChanges();
                        string message = "Insert successful";
                        return Json(new { Result = "OK", Message = message, Record = model });
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return Json(new { Result = " Error", Errors = ModelState.Errors(), Message = "Input data are not correct in form" }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(EmailMarketing model)
        {
            //End check
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var edit = db.EmailMarketings.FirstOrDefault(b => b.Id == model.Id);
                        if (edit != null)
                        {
                            edit.Email = model.Email;
                            //edit.Tel = model.Tel;
                         
                            db.SubmitChanges();

                            string message = "Update successful";
                            return Json(new { Result = "OK", Message = message });
                        }
                        else
                        {
                            return Json(new { Result = "ERROR", Message = "Video is not exist" });
                        }
                    }
                    catch (Exception exception)
                    {
                        return Json(new { Result = "Error", Message = "Error: " + exception.Message });
                    }
                }
                return Json(new { Result = " Error", Errors = ModelState.Errors(), Message = "Input data are not correct in form" }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    var del = db.EmailMarketings.FirstOrDefault(c => c.Id == Id);
                    if (del != null)
                    {
                        db.EmailMarketings.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Delete successful" });
                    }
                    return Json(new { Result = "ERROR", Message ="Email is not exist" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = "Error: " + ex.Message });
            }
        }

    }
}
