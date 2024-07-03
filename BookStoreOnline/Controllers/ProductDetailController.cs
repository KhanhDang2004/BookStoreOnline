using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class ProductDetailController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();
        // GET: ProductDetail
        public ActionResult Index(int id)
        {
            ViewBag.Book = db.SANPHAMs.FirstOrDefault(book => book.MaSanPham == id);
            ViewBag.MoreBook = db.SANPHAMs.ToList().Take(4);
            return View();
        }
    }
}