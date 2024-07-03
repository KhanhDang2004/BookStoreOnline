﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class OrdersAdminController : Controller
    {
        private NhaSachEntities db = new NhaSachEntities();

        // GET: Admin/Orders
        public ActionResult Index(int? status)
        {
            List<DONHANG> donHang = new List<DONHANG>();
            switch (status)
            {
                case 0:
                    donHang = db.DONHANGs.Where(x=>x.TrangThai == 0).ToList();
                    break;
                case 1:
                    donHang = db.DONHANGs.Where(x => x.TrangThai == 1).ToList();
                    break;
                default:
                    donHang = db.DONHANGs.ToList();
                    break;
            }
            return View(donHang.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int id)
        {
            var detail = db.CHITIETDONHANGs.Where(d => d.MaDonHang == id).ToList();
            ViewBag.Detail = detail;

            decimal total = 0;
            foreach (var item in detail)
            {
                //total += item.UnitPrice.GetValueOrDefault();
            }
            ViewBag.Total = total;
            var order = db.DONHANGs.FirstOrDefault(d => d.MaDonHang == id);
            return View(order);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "NameKHACHHANGs");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOrder,Address,Status,DateOrder,IDCus")] DONHANG donHang)
        {
            if (ModelState.IsValid)
            {
                db.DONHANGs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "NameKHACHHANGs", donHang.ID);
            return View(donHang);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG donHang = db.DONHANGs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "NameKHACHHANGs", donHang.ID);
            return View(donHang);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOrder,Address,Status,DateOrder,IDCus")] DONHANG donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "NameKHACHHANGs", donHang.ID);
            return View(donHang);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG donHang = db.DONHANGs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DONHANG donHang = db.DONHANGs.Find(id);
            db.DONHANGs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ConfirmOrder(int id)
        {
            var order = db.DONHANGs.FirstOrDefault(item => item.MaDonHang == id);
            order.TrangThai = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
