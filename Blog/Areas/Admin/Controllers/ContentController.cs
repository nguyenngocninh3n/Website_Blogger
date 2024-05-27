using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Blog.Areas.Admin.Controllers
{
    public class ContentController : Controller
    {
        DataBloggerDataContext data = new DataBloggerDataContext();
        // GET: Admin/Content
        public ActionResult Index()
        {
            var item = data.Contents.OrderBy(n=>n.IDUser).OrderBy(n=>n.IDPost).ToList();
            return View(item);
        }

        [HttpGet]
        public ActionResult Edit(string idUser, string idPost)
        {
            Session["IDUser"] = idUser;
            Session["IDPost"] = idPost;
            var item = data.Contents.Where(n => n.IDUser == idUser && n.IDPost == Convert.ToInt32(idPost)).ToList();
            return View(item);
        }

        [HttpPost]

        public ActionResult Edit(FormCollection f)
        {
            try
            {
                var content = data.Contents.SingleOrDefault(n => n.IDUser == Session["IDUser"].ToString() && n.IDPost == Convert.ToInt32(Session["IDPost"].ToString()));
                content.Title = f["title"];
                content.Contents = f["content"];
                data.SubmitChanges();
                return View(data.Contents.Where(n => n.IDUser == Session["IDUser"].ToString() && n.IDPost == Convert.ToInt32(Session["IDPost"].ToString())).ToList());
            }
            catch (Exception ex)
            {
                ViewData["err"] = ex;
                return View(data.Contents.Where(n => n.IDUser == Session["IDUser"].ToString() && n.IDPost == Convert.ToInt32(Session["IDPost"].ToString())).ToList());

            }
        }




        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            var sIdUser = f["iduser"];
            var sTitle = f["title"];
            var sContent = f["content"];
            if (String.IsNullOrEmpty(sIdUser) || data.Users.SingleOrDefault(n=>n.IDUser == sIdUser) == null)
            {
                ViewData["err"] = "Mã IDUser sai không tồn tại!";
            }
            else if (String.IsNullOrEmpty(sTitle))
            {
                ViewData["err"] = "Vui lòng nhập tiêu đề!";
            }
            else if (String.IsNullOrEmpty(sContent))
            {
                ViewData["err"] = "Vui lòng nhập nội dung";
            }
            else
            {
                Blog.Models.Content content = new Blog.Models.Content();            
                content.IDUser = sIdUser;
                var sIDP = data.KeyIndexUsers.SingleOrDefault(n => n.IDUser == sIdUser);
                content.IDPost = sIDP.IDPost + 1;
                content.Title = sTitle;
                content.UserName = data.Users.SingleOrDefault(n=>n.IDUser == sIdUser).UserName;
                content.Contents = sContent;
                content.DateCreated = DateTime.Now;
                content.HeartCount = 0;
                content.CommentCount = 0;
                content.SharedCount = 0;


                //string typeName = null;
                //var item = data.Menus;
                //foreach (var m in item)
                //{
                //    ViewData[m.Id.ToString()] = m.MenuName;
                //    if (f[m.Id.ToString()] == "true")
                //        if (m.Id > 2)
                //            if (f[m.Id.ToString()] == "on")
                //        typeName = typeName + "_" + m.MenuName + " ";

                //}
                //content.Type = typeName;


                data.Contents.InsertOnSubmit(content);
                data.SubmitChanges();
                return View();
            }
            return View();
        }

        public void CreateContentKeyOrigin(string IDUser)
        {
            KeyIndexUser key = new KeyIndexUser();
            key.IDUser = IDUser;
            key.IDPost = 0;
            data.KeyIndexUsers.InsertOnSubmit(key);
            data.SubmitChanges();
        }




        [HttpGet]
        public ActionResult Detail(string idUser, string idPost)
        {
            var item = data.Contents.Where(n => n.IDUser == idUser && n.IDPost == Convert.ToInt32(idPost)).ToList();
            return View(item);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }


    }
}