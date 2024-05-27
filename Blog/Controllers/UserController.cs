using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Antlr.Runtime.Tree;
using Blog.Models;
using Microsoft.Ajax.Utilities;

namespace Blog.Controllers
{
    public class UserController : Controller
    {
        DataBloggerDataContext data = new DataBloggerDataContext();
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            return View();
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
                User user = data.Users.SingleOrDefault(n => n.PhoneNumber == sUsername && n.Password == sPassword);
                if (user != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công.";
                    Session["User"] = user;
                    Session["IDUser"] = user.IDUser.ToString();
                    Session["UserName"] = user.UserName.ToString();
                    Session["IDGuest"] = user.IDUser.ToString();

                }
                else
                {
                    ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không chính xác.";
                }
            }
            if (Session["User"] != null)
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


        public ActionResult LoginToComment(string IDGuest)
        {
            if (String.IsNullOrEmpty(IDGuest))
            {
                return RedirectToAction("Login", "User");
            }
            Session["LoginToComment"] = true;
            Session["IDGuestComment"] = IDGuest;
            return RedirectToAction("Login", "User");
        }


        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SignUp(FormCollection f)
        {
            var sName = f["form_name"];
            var sPhoneNumber = f["form_phonenumber"];
            var sPassword = f["form_password"];
            var sPasswordRepeat = f["form_password-repeat"];
            var iDay = f["form_date-day"];
            var iMonth = f["form_date-month"];
            var iYear = f["form_date-year"];
            var sGT = f["GT_selected"];

            if (String.IsNullOrEmpty(sName))
            {
                ViewData["Err0"] = "* Vui lòng nhập họ tên!";
            }
            else if (String.IsNullOrEmpty(sPhoneNumber))
            {
                ViewData["Err0"] = "* Vui lòng nhập số điện thoại!";
            }
            else if (data.Users.SingleOrDefault(n => n.PhoneNumber == sPhoneNumber) != null)
            {
                ViewData["Err0"] = "* Số điện thoại đã được sử dụng";
            }
            else if (String.IsNullOrEmpty(sPassword))
            {
                ViewData["Err0"] = "* Vui lòng nhập mật khẩu!";
            }
            else if (String.IsNullOrEmpty(sPasswordRepeat))
            {
                ViewData["Err0"] = "* Vui lòng nhập lại mật khẩu!";
            }
            else if (sPassword != sPasswordRepeat)
            {
                ViewData["Err0"] = "* Mật khẩu không trùng khớp!";
            }
            else if (String.IsNullOrEmpty(iDay))
            {
                ViewData["Err0"] = "* Vui lòng chọn ngày sinh!";
            }
            else if (String.IsNullOrEmpty(iMonth))
            {
                ViewData["Err0"] = "* Vui lòng chọn tháng sinh!";
            }
            else if (String.IsNullOrEmpty(iYear))
            {
                ViewData["Err0"] = "* Vui lòng chọn năm sinh!";
            }
            else if (String.IsNullOrEmpty(sGT))
            {
                ViewData["Err0"] = "* Vui lòng chọn giới tính";
            }
            else
            {
                User user = new User();
                user.UserName = sName;
                user.PhoneNumber = sPhoneNumber;
                user.Password = sPassword;
                DateTime dt = new DateTime();
                dt = dt.AddDays(Convert.ToDouble(iDay)+1);
                dt = dt.AddMonths(Convert.ToInt32(iMonth)+1);
                dt = dt.AddYears(Convert.ToInt32(iYear) + 1);
                user.Birthday = dt;
                user.Sex = sGT;
                var sNum = data.KeyIndexBlogs.SingleOrDefault(n => n.IDKeyIndex == 1);
                user.IDUser = CreateID("U", Convert.ToInt32(sNum.UserIndex));
                user.DateCreated = DateTime.Now;
                data.Users.InsertOnSubmit(user);

                User_mode usermode = new User_mode();
                usermode.IDUser = user.IDUser;
              
                data.User_modes.InsertOnSubmit(usermode);

                data.SubmitChanges();
                Console.WriteLine("Đăng ký thành công!");
                Session["User"] = user;
                Session["IDUser"] = user.IDUser;
                Session["UserName"] = user.UserName;
                CreateContentKeyOrigin(user.IDUser);
                return RedirectToAction("Index", "Home");
            }
            ViewData["Name"] = sName;
            ViewData["PhoneNumber"] = sPhoneNumber;
            ViewData["DateDay"] = iDay;
            ViewData["DateMonth"] = iMonth;
            ViewData["DateYear"] = iYear;
            return this.View();
        }



        public ActionResult LogOut()
        {
            Session.Remove("IDUser");
            Session.Remove("User");
            Session.Remove("UserName");
            Session.Remove("IDGuest");
            return RedirectToAction("Index", "Home");
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

        public void UpdateIDPost(string IDUser)
        {
            var item = data.KeyIndexUsers.Where(n=>n.IDUser == IDUser).SingleOrDefault();
            item.IDPost = item.IDPost + 1;
            data.SubmitChanges();
        }
     

        public ActionResult Reaction(string idUser, string idPost)
        {
            var item = data.Reactions.Where(n => n.IDUser == Session["IDUser"].ToString() && n.IDOwner == idUser && n.IDPost == Convert.ToInt32(idPost)).ToList();
                return View(item);

           
            
        }

        public ActionResult ViewContentShared(string idUser, int idPost)
        {
            var item = data.Shares.Where(n => n.IDUser == idUser && n.IDUserPost == idPost).SingleOrDefault();
            var content = data.Contents.Where(n => n.IDUser == item.IDOwner && n.IDPost == item.IDPost).OrderByDescending(n=>n.DateCreated).ToList();
            return View(content);
        }
        public ActionResult ViewShared(string idUser, int idPost)
        {
            var item = data.Contents.Where(n => n.IDUser  == idUser && n.IDPost == idPost).ToList();
            return View(item);
        }

        public ActionResult ViewContent(string idUser)
        {
            Session["IDGuest"] = idUser;
            var content = data.Contents.Where(n => n.IDUser == idUser).OrderByDescending(n => n.DateModified).OrderByDescending(n => n.DateCreated);         
            return View(content);
        }


        [HttpGet]
        public ActionResult ViewProfile(string idUser)
        {
            Session["IDGuest"] = idUser;
            return RedirectToAction("ViewContent", "User", new { idUser = idUser });
        }

        [HttpGet]
        public ActionResult CreateContent()
        {
            return View(data.Menus.ToList());
        }

        [HttpPost]
        public ActionResult CreateContent(FormCollection f)
        {
            var sTitle = f["title"];
            var sContent = f["content"];
            if (String.IsNullOrEmpty(sTitle))
            {
                ViewData["ErrTitle"] = "Vui lòng nhập tiêu đề!";
            }
            else if (String.IsNullOrEmpty(sContent))
            {
                ViewData["ErrContent"] = "Vui lòng nhập nội dung";
            }           
            else
            {
                Blog.Models.Content content = new Blog.Models.Content();
                var sIDU = Session["IDUser"].ToString();
                content.IDUser = sIDU;
                var sIDP = data.KeyIndexUsers.SingleOrDefault(n => n.IDUser == sIDU);
                content.IDPost = sIDP.IDPost + 1;
                
;                content.Title = sTitle;
                content.UserName = Session["UserName"].ToString();
                content.Contents = sContent;
                content.DateCreated = DateTime.Now;
                content.HeartCount = 0;
                content.CommentCount = 0;
                content.SharedCount = 0;

                string typeName = null;
                var item = data.Menus;
                foreach (var m in item)
                {
                    //ViewData[m.Id.ToString()] = m.MenuName;
                    //if (f[m.Id.ToString()] == "true")
                    //  if (m.Id > 2)
                           if (f[m.Id.ToString()] == "on")
                               typeName = typeName + "_" + m.MenuName + " ";

                }

                content.Type = typeName;
                data.Contents.InsertOnSubmit(content);
                data.SubmitChanges();
               
                return RedirectToAction("ViewProfile", "User", new { idUser = Session["IDUser"] });
            }
            return RedirectToAction("ViewProfile", "User", new { idUser = Session["IDUser"].ToString() });
        }


        [HttpGet]
        public ActionResult EditContent(string idOwner, int idPost)
        {
            var content = data.Contents.Where(n => n.IDUser == idOwner && n.IDPost == idPost).ToList();
            return PartialView(content);
        }




        [HttpPost]
        public ActionResult EditContent(FormCollection f, string idOwner, int idPost)
        {
            try
            {
                var sTitle = f["title"];
                var sContent = f["content"];
                if (String.IsNullOrEmpty(sTitle))
                {
                    ViewData["ErrTitle"] = "Vui lòng nhập tiêu đề!";
                }
                else if (String.IsNullOrEmpty(sContent))
                {
                    ViewData["ErrContent"] = "Vui lòng nhập nội dung";
                }
                else
                {
                    var content = data.Contents.SingleOrDefault(n => n.IDUser == idOwner && n.IDPost == idPost);
                    content.Title = sTitle;
                    content.Contents = sContent;
                    content.DateModified = DateTime.Now;
                    data.SubmitChanges();
                    return RedirectToAction("ViewProfile", "User", new { idUser = idOwner });
                }
                // return RedirectToAction("ViewProfile", "User", new { idUser = Session["IDUser"].ToString() });
                return RedirectToAction("EditContent", "User", new { idOwner = idOwner, idPost = idPost });
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }


        [HttpGet]
        public ActionResult CreateComment()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CreateComment(string idUser, string idOwner, string idPost, string commentString, string idParent, string Level, string idComment)
        {
            try
            {
                var cmtCount = data.Contents.SingleOrDefault(p => p.IDUser == idOwner && p.IDPost == Convert.ToInt32(idPost));
                var parentItem = data.Comments.SingleOrDefault(p => p.IDComment == Convert.ToInt32(idComment));
                cmtCount.CommentCount += 1;
                Comment comment = new Comment();
                comment.IDComment = 1;
                comment.IDUser = idUser;
                comment.UserName = Session["UserName"].ToString();
                comment.IDPost = Convert.ToInt32(idPost);
                comment.IDOwner = idOwner;
                comment.CommentString = commentString;
                     
                if (String.IsNullOrEmpty(Level))
                {
                    comment.Level = 0;
                    comment.NumChild = 0;
                }
                else
                {
                    comment.Level = Convert.ToInt32(Level.ToString()) + 1;
                    parentItem.NumChild++;
                }
              
                if(String.IsNullOrEmpty(idParent))
                {
                    idParent = idOwner;
                }
                else
                {
                    comment.IDParent = idParent;
                }

                comment.DateCreated = DateTime.Now;
                data.Comments.InsertOnSubmit(comment);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Thêm bình luận thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Thêm bình luận thất bại" + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SavedContent()
        {
            var item = from n in data.SavedContents from m in data.Contents where n.IDUser == Session["IDUser"].ToString() where n.IDOwner == m.IDUser where n.IDPost == m.IDPost select m ;
            return View(item.OrderByDescending(n=> n.DateCreated));
        }
        [HttpPost]
        public JsonResult SavedContent(string idOwner, string idPost)
        {
          try
            {
                SavedContent item = new SavedContent();
                item.IDUser = Session["IDUser"].ToString();
                item.IDPost = Convert.ToInt32(idPost);
                item.IDOwner = idOwner;
                item.DateCreated = DateTime.Now;
                data.SavedContents.InsertOnSubmit(item);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Lưu bài viết thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Bài viết đã lưu trước đó" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Update(string idUser, string idOwner, int idPost)
        {
            try
            {
                var isValue = true;
                var post = data.Contents.SingleOrDefault(p => p.IDUser == idOwner && p.IDPost == Convert.ToInt32(idPost));
                var isLiked = data.Reactions.SingleOrDefault(p => p.IDUser == idUser && p.IDPost == idPost && p.IDOwner == idOwner);
                if (isLiked == null)
                {
                    post.HeartCount += 1;
                    Reaction react = new Reaction();
                    react.IDPost = idPost;
                    react.IDOwner = idOwner;
                    react.IDUser = idUser;
                    react.IsLiked = 1;
                    data.Reactions.InsertOnSubmit(react);
                }
                else if (isLiked.IsLiked == 1)
                {
                    post.HeartCount -= 1;
                    isLiked.IsLiked = 0;
                    isValue = false;
                }
                else
                {
                    post.HeartCount += 1;
                    isLiked.IsLiked = 1;
                }

                data.SubmitChanges();
                if (isValue == true)
                {
                    return Json(new { code = 200, msg = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 200, msg = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Like thất bại " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Share(string idUser, string idOwner, int idPost)
        {
            try
            {
                Blog.Models.Content content = new Blog.Models.Content();
                content.IDUser = idUser;
                content.UserName = Session["UserName"].ToString();
                var sIDP = data.KeyIndexUsers.SingleOrDefault(n => n.IDUser == idUser);
                content.IDPost = sIDP.IDPost + 1;
               
                content.DateCreated = DateTime.Now;
                content.HeartCount = 0;
                content.CommentCount = 0;
                content.SharedCount = 0;
                data.Contents.InsertOnSubmit(content);
                data.SubmitChanges();
                

                var post = data.Contents.SingleOrDefault(p => p.IDUser == idOwner && p.IDPost == Convert.ToInt32(idPost));
                var isLiked = data.Reactions.SingleOrDefault(p => p.IDUser == idUser && p.IDPost == idPost && p.IDOwner == idOwner);
                if (isLiked == null)
                {
                    post.SharedCount += 1;
                    Reaction react = new Reaction();
                    react.IDPost = idPost;
                    react.IDOwner = idOwner;
                    react.IDUser = idUser;                 
                    data.Reactions.InsertOnSubmit(react);
                }
              
                else
                {
                    post.SharedCount += 1;
                    isLiked.IsLiked = 1;
                }
                data.SubmitChanges();

                Blog.Models.Share share = new Blog.Models.Share();
                share.IDUser = idUser;
                share.IDUserPost = content.IDPost;
                share.IDOwner = idOwner;
                share.IDPost = idPost;
                share.UserName = content.UserName;
                share.DateCreated = DateTime.Now;
                share.DateModified = DateTime.Now;
                data.Shares.InsertOnSubmit(share);


                data.SubmitChanges();
                    return Json(new { code = 200, msg = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Chia sẻ thất bại " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult EditInfor(string idUser)
        {
            Session["ErrFull"] = "0";
            Session["ErrJob"] = "0";
            Session["ErrJobMode"] = "0";
            var user = data.Users.Where(n => n.IDUser == idUser).SingleOrDefault();
            var usermode = data.User_modes.Where(n => n.IDUser == idUser).SingleOrDefault();
            List<InforUser> item = new List<InforUser>();
            item.Add(new InforUser(user, usermode));
            Session["IDEdit"] = idUser;
            return View(item);
        }

        [HttpPost]
        public ActionResult EditInfor(FormCollection f)
        {
            var user = data.Users.SingleOrDefault(n => n.IDUser == Session["IDEdit"].ToString());
            var usermode = data.User_modes.SingleOrDefault(n => n.IDUser == Session["IDEdit"].ToString());
            try
            {

                if (String.IsNullOrEmpty(f["UserName"]))
                {
                    ViewData["errUserName"] = "UserName không được để trống";
                }
                user.UserName = f["UserName"].ToString();

                //gioi tinh
                if (String.IsNullOrEmpty(f["Sex"]))
                    usermode.SexMode = false;
                else
                {
                    user.Sex = f["Sex"].ToString();
                    if (f["SexMode"].ToString() == "on")
                        usermode.SexMode = true;
                    else
                        usermode.SexMode = false;
                }
                //công việc
                if (String.IsNullOrEmpty(f["Job"]))
                    usermode.JobMode = false;
                else
                {
                    user.Job = f["Job"].ToString();
                    if (f["JobMode"].ToString() == "on")
                        usermode.JobMode = true;
                    else
                        usermode.JobMode = false;
                }
                //email
                if (String.IsNullOrEmpty(f["Email"]))
                    usermode.EmailMode = false;
                else
                {
                    user.Email = f["Email"].ToString();
                    if (f["EmailMode"].ToString() == "on")
                        usermode.EmailMode = true;
                    else
                        usermode.EmailMode = false;
                }

                //birthday
                var iDay = f["birthdayday"];
                var iMonth = f["birthdaymonth"];
                var iYear = f["birthdayyear"];

                DateTime dt = new DateTime();
                dt = dt.AddDays(Convert.ToDouble(iDay) - 1);
                dt = dt.AddMonths(Convert.ToInt32(iMonth) - 1);
                dt = dt.AddYears(Convert.ToInt32(iYear) - 1);
                user.Birthday = dt;
                if (f["BirthdayMode"].ToString() == "on")
                    usermode.BirthdayMode = true;
                else
                    usermode.BirthdayMode = false;

                //address
                if (String.IsNullOrEmpty(f["Address"]))
                    usermode.AddressMode = false;
                else
                {
                    user.Address = f["Address"].ToString();
                    if (f["AddressMode"].ToString() == "on")
                        usermode.AddressMode = true;
                    else
                        usermode.AddressMode = false;
                }

                if (String.IsNullOrEmpty(f["Degree"]))
                    usermode.DegreeMode = false;
                else
                {
                    user.Degree = f["Degree"].ToString();
                    if (f["DegreeMode"].ToString() == "on")
                        usermode.DegreeMode = true;
                    else
                        usermode.DegreeMode = false;
                }

                if (String.IsNullOrEmpty(f["Facebook"]))
                    usermode.FacebookMode = false;
                else
                {
                    user.Facebook = f["Facebook"].ToString();
                    if (f["FacebookMode"].ToString() == "on")
                        usermode.FacebookMode = true;
                    else
                        usermode.FacebookMode = false;
                }

                if (String.IsNullOrEmpty(f["Instagram"]))
                    usermode.InstagramMode = false;
                else
                {
                    user.Instagram = f["Instagram"].ToString();
                    if (f["InstagramMode"].ToString() == "on")
                        usermode.InstagramMode = true;
                    else
                        usermode.InstagramMode = false;
                }

                if (String.IsNullOrEmpty(f["Youtube"]))
                    usermode.YoutubeMode = false;
                else
                {
                    user.Youtube = f["Youtube"].ToString();
                    if (f["YoutubeMode"].ToString() == "on")
                        usermode.YoutubeMode = true;
                    else
                        usermode.YoutubeMode = false;
                }

                if (String.IsNullOrEmpty(f["LinkedIn"]))
                    usermode.LinkedInMode = false;
                else
                {
                    user.LinkedIn = f["LinkedIn"].ToString();
                    if (f["LinkedInMode"].ToString() == "on")
                        usermode.LinkedInMode = true;
                    else
                        usermode.LinkedInMode = false;
                }
                data.SubmitChanges();
            }
            catch (Exception ex)
            {
                Session["ErrFull"] = "Error " + ex.Message;
                Session["ErrJob"] = "2";
                Session["ErrJobMode"] = "2";
            }
            List<InforUser> itemm = new List<InforUser>();
            itemm.Add(new InforUser(user, usermode));
            Session["ErrJobMode"] = "item" + itemm[0].JobMode.ToString();
            return View(itemm);

        }








        [HttpPost]
        public JsonResult DeleteContent(string idOwner, int idPost)
        {
            if (Session["IDUser"] == null)
            {
                return Json(new { code = 500, msg = "Xóa bài viết thất bại. Bạn không có quyền xóa bài viết này." }, JsonRequestBehavior.AllowGet);
            }
            else if (Session["IDUser"].ToString() != idOwner)
            {
                return Json(new { code = 500, msg = "Xóa bài viết thất bại. Bạn không có quyền xóa bài viết của người khác." }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var content = data.Contents.SingleOrDefault(n => n.IDUser == idOwner && n.IDPost == idPost);
                var item = data.Comments.Where(n => n.IDOwner == idOwner && n.IDPost == idPost).ToList();
                var react = data.Reactions.Where(n => n.IDOwner == idOwner && n.IDPost == idPost).ToList();
                data.Contents.DeleteOnSubmit(content);
                data.Comments.DeleteAllOnSubmit(item);
                data.Reactions.DeleteAllOnSubmit(react);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa bài viết thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Xóa bài viết thất bại" + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}