using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Blog.Areas.Admin.Controllers
{
    public class Menu2Controller : Controller
    {
        // GET: Admin/Menu2
        DataBloggerDataContext data = new DataBloggerDataContext();

        [HttpGet]
        public ActionResult Index()
        {
            var lstMenu = data.Menus.Where(n => n.ParentId == null).OrderBy(n => n.OrderNumber).ToList();
            int[] a = new int[lstMenu.Count];
            for (int i = 0; i < lstMenu.Count; i++)
            {
                var l = data.Menus.Where(m => m.ParentId == lstMenu[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
          
            return View(lstMenu);
        }

        [ChildActionOnly]
        public ActionResult ChildMenu(int parentId)
        {
            List<Menu> lst = new List<Menu>();
            lst = data.Menus.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = data.Menus.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("ChildMenu", lst);
        }


        [ChildActionOnly]
        public ActionResult ChildMenu1(int parentId)
        {
            List<Menu> lst = new List<Menu>();
            lst = data.Menus.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = data.Menus.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("ChildMenu1", lst);
        }



        [HttpPost]
        public ActionResult AddMenu(FormCollection f)
        {          
                Menu m = new Menu();
                 if (!String.IsNullOrEmpty(f["ThemLink"]))
            {
              
                m.MenuName = f["TenMenu"];
                m.MenuLink = f["Link"];
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number2"]);
                data.Menus.InsertOnSubmit(m);
                data.SubmitChanges();
            }
            return Redirect("~/Admin/Menu2/Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            List<Menu> submn = data.Menus.Where(m => m.ParentId == id).ToList();
            if (submn.Count > 0)
            {
                return Json(new { code = 500, msg = " Còn Menu con, không xóa được" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var mn = data.Menus.SingleOrDefault(m => m.Id == id);
                data.Menus.DeleteOnSubmit(mn);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Update(int id)
        {
            try
            {
                var mn = (from m in data.Menus
                          where (m.Id == id)
                          select new
                          {
                              Id = m.Id,
                              MenuName = m.MenuName,
                              MenuLink = m.MenuLink,
                              OrderNumber = m.OrderNumber
                          }).SingleOrDefault();
                return Json(new { code = 200, mn = mn, msg = "Lấy thông tin thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(int id, string strTenMenu, string strLink, int STT)
        {
            try
            {
                var mn = data.Menus.SingleOrDefault(m => m.Id == id);
                mn.MenuName = strTenMenu;
                mn.MenuLink = strLink;
                mn.OrderNumber = STT;
                data.SubmitChanges();
                return Json(new { code = 200, mn = mn, msg = "Sửa Menu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa Menu thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}