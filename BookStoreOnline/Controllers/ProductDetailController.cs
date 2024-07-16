using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;
using BookStoreOnline.Models.Custom;
using WebGrease;

namespace BookStoreOnline.Controllers
{
    public class ProductDetailController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();
        // GET: ProductDetail
        public ActionResult Index(int id)
        {
            ViewBag.Book = db.SANPHAMs.FirstOrDefault(book => book.MaSanPham == id);
            List<DanhGiaItem> danhGia = (from d in db.DANHGIAs
                                         join k in db.KHACHHANGs on d.MaKH equals k.MaKH
                                         where d.MaSanPham == id
                                         orderby d.NgayTao descending
                                         select new DanhGiaItem
                                         {
                                             danhGia = d,
                                             nguoiDanhGia = k.Ten
                                         }).Take(4).ToList();
            ViewBag.DanhSachDanhGia = danhGia;
            ViewBag.MaSanPham = id;
            ViewBag.MoreBook = db.SANPHAMs.ToList().Take(4);
            return View();
        }

        [HttpPost]
        public ActionResult PostDanhGia(DanhGiaContent model)
        {
            if (ModelState.IsValid)
            {
                KHACHHANG khachHangDaDangNhap = Session["TaiKhoan"] as KHACHHANG;
                DANHGIA danhGiaMoi = new DANHGIA()
                {
                    MaKH = khachHangDaDangNhap.MaKH,
                    MaSanPham = model.MaSanPham,
                    NgayTao = DateTime.Now,
                    NoiDung = model.NoiDungDanhGia
                };

                db.DANHGIAs.Add(danhGiaMoi);
                db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = model.MaSanPham });
            }

            return RedirectToAction("Index", new { id = model.MaSanPham });
        }
    }
}