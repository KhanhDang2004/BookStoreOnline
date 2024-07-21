using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;
using PayPal.Api;

namespace BookStoreOnline.Controllers
{
    public class HomeController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();
        public ActionResult Index()
        {
            var book = db.SANPHAMs.ToList().Take(8);
            return View(book);
        }
        public ActionResult TopSeller()
        {
            var topseller = db.SANPHAMs
             .OrderByDescending(p => p.SoLuongBan)
             .Take(4)
             .ToList();
            return View(topseller);
        }
        public ActionResult NewBook()
        {
            var newBook = db.SANPHAMs
             .OrderByDescending(p => p.NgayTao)
             .Take(3)
             .ToList();
            return View(newBook);
        }
    }
}