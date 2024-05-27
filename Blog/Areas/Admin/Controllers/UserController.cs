using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        DataBloggerDataContext data = new DataBloggerDataContext();
        // GET: Admin/User
        [HttpGet]
        public ActionResult Index()
        {
            var item = data.Users.ToList();
            return View(item);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            Session["User"] = null;
            var sUsername = collection["Username"];
            var sPassword = collection["Password"];
            if (String.IsNullOrEmpty(sUsername))
            {
                ViewData["Err1"] = "* Vui lòng nhập tên tài khoản";
            }
            else if (String.IsNullOrEmpty(sPassword))
            {
                ViewData["Err2"] = "* Vui lòng nhập mật khẩu";
            }
            else
            {
                var user = data.Admins.SingleOrDefault(n => n.PhoneNumber == sUsername && n.Password == sPassword);
                if (user != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công.";
                    Session["Admin"] = user;
                    Session["IDAdmin"] = user.IDAdmin.ToString();
                    Session["AdminName"] = user.AdminName.ToString();
                    Session["IDGuest"] = user.IDAdmin.ToString();

                }
                else
                {
                    ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không chính xác.";
                }
            }
            if (Session["Admin"] != null)
            {
                //if(String.IsNullOrEmpty(Session["LoginToComment"].ToString()))
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                //if (String.IsNullOrEmpty(Session["LoginToComment"].ToString()))
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {

            if (f["type"] == "IDUser")
            {
                var item = data.Users.Where(n => n.IDUser.Contains(f["search"].ToString())).ToList();
                return View(item);
            }
            if (f["type"] == "UserName")
            {
                var item = data.Users.Where(n => n.UserName.Contains(f["search"].ToString())).ToList();
                return View(item);
            }
            if (f["type"] == "PhoneNumber")
            {
                var item = data.Users.Where(n => n.PhoneNumber.Contains(f["search"].ToString())).ToList();
                return View(item);
            }
            if (f["type"] == "Email")
            {
                var item = data.Users.Where(n => n.Email.Contains(f["search"].ToString())).ToList();
                return View(item);
            }
            if (f["type"] == "Address")
            {
                var item = data.Users.Where(n => n.Address.Contains(f["search"].ToString())).ToList();
                return View(item);
            }
            if (f["type"] == "Job")
            {
                var item = data.Users.Where(n => n.Job.Contains(f["search"].ToString())).ToList();
                return View(item);
            }
            if (f["type"] == "Degree")
            {
                var item = data.Users.Where(n => n.Degree.Contains(f["search"].ToString())).ToList();
                return View(item);
            }
            

            return View(data.Users.Where(n=>n.IDUser=="-1").ToList());


        }

        [HttpGet]
        public ActionResult Detail(string idUser)
        {
            var item = data.Users.Where(n => n.IDUser == idUser).ToList();
            return View(item);
        }

        [HttpGet]
        public ActionResult Edit(string idUser)
        {
            Session["IDUser"] = idUser;
            var item = data.Users.Where(n => n.IDUser == idUser).ToList();
            return View(item);
        }


        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            try
            {
                var user = data.Users.SingleOrDefault(n => n.IDUser == Session["IDUser"].ToString());
                if (String.IsNullOrEmpty(f["username"]))
                    ViewData["err"] = "UserName không được để trống";
                else
                    user.UserName = f["username"];
                if (String.IsNullOrEmpty(f["phonenumber"]))
                    ViewData["err"] = "Số điện thoại không được để trống";
                else
                    user.PhoneNumber = f["phonenumber"].ToString();
                if (String.IsNullOrEmpty(f["password"]))
                    ViewData["err"] = "Mật khẩu không được để trống";
                else
                    user.Password = f["password"].ToString();

                user.Sex = f["sex"];
                user.Email = f["email"];
                var iDay = f["birthdayday"];
                var iMonth = f["birthdaymonth"];
                var iYear = f["birthdayyear"];

                DateTime dt = new DateTime();
                dt = dt.AddDays(Convert.ToDouble(iDay) - 1);
                dt = dt.AddMonths(Convert.ToInt32(iMonth) - 1);
                dt = dt.AddYears(Convert.ToInt32(iYear) - 1);
                user.Birthday = dt;

                user.Address = f["address"];
                user.Degree = f["degree"];
                user.Job = f["job"];
                user.Facebook = f["facebook"];
                user.Instagram = f["instagram"];
                user.Youtube = f["youtube"];
                user.LinkedIn = f["linkedin"];
                data.SubmitChanges();
                return View(data.Users.Where(n => n.IDUser == Session["IDUser"].ToString()).ToList());
            }
            catch (Exception ex)
            {
                ViewData["err"] = ex;
                return View(data.Users.Where(n => n.IDUser == Session["IDUser"].ToString()).ToList());
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
            //try
            //{
                User user = new User();
                var sNum = data.KeyIndexBlogs.SingleOrDefault(n => n.IDKeyIndex == 1);
                user.IDUser = CreateID("U", Convert.ToInt32(sNum.UserIndex));


                if (String.IsNullOrEmpty(f["username"]))
                    ViewData["err"] = "UserName không được để trống";
                else if (String.IsNullOrEmpty(f["phonenumber"]))
                    ViewData["err"] = "Số điện thoại không được để trống";
                else if (String.IsNullOrEmpty(f["password"]))
                    ViewData["err"] = "Mật khẩu không được để trống";
                else
                {
                    user.Password = f["password"].ToString();
                    user.PhoneNumber = f["phonenumber"].ToString();
                    user.UserName = f["username"];
                    user.Sex = f["sex"];
                    user.Email = f["email"];
                    var iDay = f["birthdayday"];
                    var iMonth = f["birthdaymonth"];
                    var iYear = f["birthdayyear"];

                    DateTime dt = new DateTime();
                    dt = dt.AddDays(Convert.ToDouble(iDay) - 1);
                    dt = dt.AddMonths(Convert.ToInt32(iMonth) - 1);
                    dt = dt.AddYears(Convert.ToInt32(iYear) - 1);
                    user.Birthday = dt;

                    user.Address = f["address"];
                    user.Degree = f["degree"];
                    user.Job = f["job"];
                    user.Facebook = f["facebook"];
                    user.Instagram = f["instagram"];
                    user.Youtube = f["youtube"];
                    user.LinkedIn = f["linkedin"];
                    data.Users.InsertOnSubmit(user);
                    data.SubmitChanges();
                    CreateContentKeyOrigin(user.IDUser);
                }
          
            return View();
            //}
            //catch (Exception ex)
            //{
            //    ViewData["err"] = ex;
            //    return RedirectToAction("Create", "User");
            //}
        }


        [HttpGet]
        public ActionResult Delete(string idUser)
        {
            try
            {
                var t = data.Users.SingleOrDefault(n => n.IDUser == idUser);
                data.Users.DeleteOnSubmit(t);
                data.SubmitChanges();
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                ViewData["err"] = ex;
                return RedirectToAction("Index", "User");
            }
        }

        public string CreateID(string type, int num)
        {
            string sID;
            if (num < 10)
            {
                sID = type + "000" + num.ToString();
            }
            else if (num < 100)
            {
                sID = type + "00" + num.ToString();
            }
            else if (num < 1000)
            {
                sID = type + "0" + num.ToString();
            }
            else if (num < 10000)
            {
                sID = type + num.ToString();
            }
            else
            {
                sID = null;
            }
            return sID;
        }

        public void CreateContentKeyOrigin(string IDUser)
        {
            KeyIndexUser key = new KeyIndexUser();
            key.IDUser = IDUser;
            key.IDPost = 0;
            data.KeyIndexUsers.InsertOnSubmit(key);
            data.SubmitChanges();
        }


    }
}