using Blog.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        DataBloggerDataContext data = new DataBloggerDataContext();
  
        public ActionResult Index()
        {
              
            
            var item = data.Contents.OrderByDescending(n=>n.DateCreated).ToList();
            return View(item);
           
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            var content = data.Contents.Where(n => n.Title.Contains(search)).OrderByDescending(n => n.DateCreated).ToList();
            return View(content);
        }

        [HttpPost]
        public ActionResult Search(FormCollection f)
        {
    
            if (f["type"].Equals("baiviet"))
            {
                //var content = data.Contents.Where(n => n.Title.Contains(f["search"])).OrderByDescending(n => n.DateCreated).ToList();
                //return View(content);
                return RedirectToAction("Search", "Home", new { search = f["search"].ToString() });

            }
            else
            {
              
                return RedirectToAction("SearchByAuthor", "Home", new { search = f["search"].ToString() });
            }    
        }

        public ActionResult SearchByAuthor(string search)
        {
            var tacgia = data.Users.Where(n => n.UserName.Contains(search)).ToList();
            return View(tacgia);
        }

        public ActionResult HeaderPartial()
        {
            return PartialView();
        }


        public ActionResult InforPartial(string idUser)
        {
            Session["IDGuest"] = idUser;
            var user = data.Users.Where(n => n.IDUser == idUser).SingleOrDefault();
            var usermode = data.User_modes.SingleOrDefault(n=>n.IDUser == idUser);
            List<InforUser> item = new List<InforUser>();
            item.Add(new InforUser(user, usermode));
            return PartialView(item);             
        }

        

        public ActionResult FooterPartial()
        {
            return PartialView();
        }


        [ChildActionOnly]
        public ActionResult NavPartial()
        {
            List<Blog.Models.Menu> lst = new List<Blog.Models.Menu>();
            lst = data.Menus.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count(); i++)
            {
                var l = data.Menus.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView(lst);
        }


        [ChildActionOnly]
        public ActionResult LoadChildMenu(int parentId)
        {
            List<Blog.Models.Menu> lst = new List<Blog.Models.Menu>();
            lst = data.Menus.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = data.Menus.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("LoadChildMenu", lst);
        }


        public ActionResult ViewComment(string idOwner, int idPost)
        {
            var content = data.Comments.Where(n => n.IDOwner == idOwner && n.IDPost == idPost).OrderByDescending(n => n.DateCreated);
            return PartialView(content);
        }

        public ActionResult GetChildComment(string idParent, string idOwner, int idPost ,int Level)
        {
            var content = data.Comments.Where(n => n.IDOwner == idOwner && n.IDPost == idPost && n.IDParent == idParent && n.Level == Level +1 ).OrderBy(n => n.DateCreated);
            return PartialView(content);
        }


        public ActionResult Filter(string type)
        {
            var content = data.Contents.Where(n => n.Type.Contains(type)).OrderByDescending( n => n.DateCreated).ToList();
            return View(content);
        }

        [HttpPost]
        public ActionResult EditAvatar(FormCollection f, HttpPostedFileBase avatarfile)
        {
            try
            {
                if (avatarfile == null)
                {
                    ViewData["ErrAvt"] = "Vui lòng  tải ảnh lên!";
                    return View();
                }
                var user = data.Users.SingleOrDefault(n => n.IDUser == Session["IDUser"].ToString());
                var sFileName = Path.GetFileName(avatarfile.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images"), sFileName);
                if (System.IO.File.Exists(path))
                {
                    avatarfile.SaveAs(path);
                }
                user.Avatar = sFileName;
                data.SubmitChanges();
                return View();
            }
            catch(Exception ex)
            {
                Session["TestErr"] = "Lỗi! " + ex;
                return View();
            }
        }
    }
}