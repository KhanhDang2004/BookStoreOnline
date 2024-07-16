using BookStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class PromotionController : Controller
    {
        private NhaSachEntities db = new NhaSachEntities();

        // GET: Admin/Promotion
        public ActionResult Index()
        {
            return View(db.KHUYENMAIs.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenChuongTrinh")] KHUYENMAI khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KHUYENMAIs.Add(khuyenMai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khuyenMai);
        }
    }
}