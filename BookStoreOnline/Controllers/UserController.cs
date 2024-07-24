using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Areas.Admin.Models;
using BookStoreOnline.Core;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class UserController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();
        // GET: User
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
        public ActionResult Login(KHACHHANG taikhoan)
        {
            //return RedirectToAction("Index", "Home");
            string matKhau = Extension.GetMd5Hash(taikhoan.MatKhau);
            var account = db.KHACHHANGs.FirstOrDefault(k => k.Email == taikhoan.Email && k.MatKhau == matKhau);
            if (account != null)
            {
                Session["TaiKhoan"] = account;
                return RedirectToAction("Index", "Home");
            }
            else
            {

                ViewBag.ThongBao = "Email hoặc mật khẩu không chính xác";
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session["TaiKhoan"] = null;
            Session["GioHang"] = null;

            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(KHACHHANG cus, string rePass)
        {
            if (ModelState.IsValid)
            {
                var checkEmail = db.KHACHHANGs.FirstOrDefault(c => c.Email == cus.Email);
                if (checkEmail != null)
                {
                    ViewBag.ThongBaoEmail = "Đã có tài khoản đăng nhập bằng Email này";
                    return View();
                }
                if (cus.MatKhau == rePass)
                {
                    cus.NgayTao = DateTime.Now;
                    cus.TrangThai = true;
                    cus.MatKhau = Extension.GetMd5Hash(cus.MatKhau);
                    db.KHACHHANGs.Add(cus);
                    db.SaveChanges();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.ThongBao = "Mật khẩu xác nhận không chính xác";
                    return View();
                }
            }
            return View();
        }

        [HttpGet]

        // Chỉ cho phép người dùng đã đăng nhập mới truy cập
        public ActionResult Edit()
        {
            var currentUserId = GetCurrentUserID(); // Lấy ID của người dùng hiện tại
            var customer = db.KHACHHANGs.FirstOrDefault(c => c.MaKH == currentUserId);

            if (customer == null)
            {
                return HttpNotFound();
            }

            var model = new EditCustomerViewModel
            {
                Id = customer.MaKH,
                FullName = customer.Ten,
                Address = customer.DiaChi,
                PhoneNumber = customer.SoDienThoai,
                Email = customer.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = GetCurrentUserID(); // Lấy ID của người dùng hiện tại
                var customer = db.KHACHHANGs.FirstOrDefault(c => c.MaKH == currentUserId);

                if (customer == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin khách hàng
                customer.Ten = model.FullName;
                customer.DiaChi = model.Address;
                customer.SoDienThoai = model.PhoneNumber;
                customer.Email = model.Email;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.ThongBao = "Lưu thành công";
                return View();
            }

            /* db.SaveChanges();*/
            return View(model);
        }

        private int GetCurrentUserID()
        {
            // Lấy ID của người dùng hiện tại (ví dụ từ thông tin đăng nhập)
            KHACHHANG khachHangDaDangNhap = Session["TaiKhoan"] as KHACHHANG;
            return khachHangDaDangNhap.MaKH;
        }
    }
}