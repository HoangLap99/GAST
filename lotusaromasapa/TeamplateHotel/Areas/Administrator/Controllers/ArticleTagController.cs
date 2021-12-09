using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using TeamplateHotel.Areas.Administrator.EntityModel;
namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class ArticleTagController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang quản lý tag bài viết";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<ArticleTag> list = db.ArticleTags.Where(a => a.LanguageID == Request.Cookies["lang_client"].Value).ToList();
                    var records = list.Select(a => new
                    {
                        a.ID,
                        a.TagName,
                    }).OrderByDescending(a => a.ID).Skip(jtStartIndex).Take(jtPageSize).ToList();
                    //Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = list.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.TagName = "Thêm phòng";
            var ArticleTag = new ArticleTag();
            return View(ArticleTag);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ArticleTag model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    //if (string.IsNullOrEmpty(model.Alias))
                    //{
                    //    model.Alias = StringHelper.ConvertToAlias(model.TagName);
                    //}
                    try
                    {
                        var room = new ArticleTag
                        {
                            LanguageID = Request.Cookies["lang_client"].Value,
                            TagName = model.TagName,
                            Description = model.Description,
                            Alias = model.Alias
                        };
                        db.ArticleTags.InsertOnSubmit(room);
                        db.SubmitChanges();

                        TempData["Messages"] = "Thêm tag thành công.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (var db = new MyDbDataContext())
            {
                ArticleTag detailRoom = db.ArticleTags.FirstOrDefault(a => a.ID == id);
                if (detailRoom == null)
                {
                    TempData["Messages"] = "tag không tồn tại";
                    return RedirectToAction("Index");
                }
                ViewBag.TagName = "Sửa tag";
                var room = new ArticleTag
                {
                    ID = detailRoom.ID,
                    TagName = detailRoom.TagName,
                    Description = detailRoom.Description,
                    Alias = detailRoom.Alias,
                };
                return View(room);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ArticleTag model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        ArticleTag room = db.ArticleTags.FirstOrDefault(b => b.ID == model.ID);
                        if (room != null)
                        {
                            room.TagName = model.TagName;
                            room.Description = model.Description;
                            room.Alias = model.Alias;
                            db.SubmitChanges();

                            TempData["Messages"] = "Sửa tag thành công";
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    ArticleTag del = db.ArticleTags.FirstOrDefault(c => c.ID == id);
                    if (del != null)
                    {
                        db.ArticleTags.DeleteOnSubmit(del);
                        db.SubmitChanges();
                        return Json(new { Result = "OK", Message = "Xóa phòng thành công" });
                    }
                    return Json(new { Result = "ERROR", Message = "Phòng không tồn tại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }
}