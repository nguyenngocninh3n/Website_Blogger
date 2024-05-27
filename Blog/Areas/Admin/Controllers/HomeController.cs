using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DataBloggerDataContext data = new DataBloggerDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HeaderPartial()
        {
            return PartialView();
        }

        public ActionResult FooterPartial()
        {
            return PartialView();
        }

        public ActionResult NavparPartial()
        {
            List<Blog.Models.MenuAdmin> lst = new List<Blog.Models.MenuAdmin>();
           lst = data.MenuAdmins.OrderBy(n => n.OrderNumber).ToList();
            return PartialView(lst);
        }

    }
}