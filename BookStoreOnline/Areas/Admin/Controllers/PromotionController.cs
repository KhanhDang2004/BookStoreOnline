using BookStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
        public ActionResult Create([Bind(Include = "ID,TenChuongTrinh,MaKM,MoTa,NgayTao,NgayBatDau,NgayKetThuc,SoTienKM,SoTienMuaHangToiThieu,SoLanDung,KichHoat")] KHUYENMAI khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KHUYENMAIs.Add(khuyenMai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khuyenMai);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenMai = db.KHUYENMAIs.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHUYENMAI khuyenMai = db.KHUYENMAIs.Find(id);
            db.KHUYENMAIs.Remove(khuyenMai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KHUYENMAI khuyenMai = db.KHUYENMAIs.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }

            return View(khuyenMai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenChuongTrinh,MaKM,MoTa,NgayTao,NgayBatDau,NgayKetThuc,SoTienKM,SoTienMuaHangToiThieu,SoLanDung,KichHoat")] KHUYENMAI khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khuyenMai);
        }


    }
}