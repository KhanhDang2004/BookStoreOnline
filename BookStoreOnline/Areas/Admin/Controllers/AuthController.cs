using BookStoreOnline.Areas.Admin.Models;
using BookStoreOnline.Core;
using BookStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private NhaSachEntities db = new NhaSachEntities();
        // GET: Admin/Login
        public ActionResult Login()
        {
            if (Session["TaiKhoan"] != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel taikhoan)
        {
            if (ModelState.IsValid)
            {
                string matKhau = Extension.GetMd5Hash(taikhoan.Password);

                var account = db.NHANVIENs.FirstOrDefault(k => k.Email == taikhoan.Email && k.MatKhau == matKhau);
                if (account != null)
                {
                    Session["TaiKhoan"] = account;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {

                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác!");
                }
            }

            // If we got this far, something failed, redisplay the form
            return View(taikhoan);
        }
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Login", "Auth");
        }
    }
}