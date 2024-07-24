using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;
using static BookStoreOnline.Areas.Admin.Constants.Constants;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class OrdersAdminController : Controller
    {
        private NhaSachEntities db = new NhaSachEntities();

        // GET: Admin/Orders
        public ActionResult Index(string status)
        {
            List<DONHANG> donHang;

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse(status, out StatusOrder parsedStatusOrder))
                {
                    switch (parsedStatusOrder)
                    {
                        case StatusOrder.NoInform:
                            donHang = db.DONHANGs.Where(x => x.TrangThai == StatusOrder.NoInform.ToString()).ToList();
                            break;
                        case StatusOrder.Informed:
                            donHang = db.DONHANGs.Where(x => x.TrangThai == StatusOrder.Informed.ToString()).ToList();
                            break;
                        case StatusOrder.Shipping:
                            donHang = db.DONHANGs.Where(x => x.TrangThai == StatusOrder.Shipping.ToString()).ToList();
                            break;
                        case StatusOrder.Received:
                            donHang = db.DONHANGs.Where(x => x.TrangThai == StatusOrder.Received.ToString()).ToList();
                            break;
                        case StatusOrder.Canceled:
                            donHang = db.DONHANGs.Where(x => x.TrangThai == StatusOrder.Canceled.ToString()).ToList();
                            break;
                        default:
                            donHang = db.DONHANGs.ToList();
                            break;
                    }
                }
                else
                {
                    donHang = db.DONHANGs.ToList();
                }
            }
            else
            {
                // If no status is provided, return all orders
                donHang = db.DONHANGs.ToList();
            }
            return View(donHang.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int id)
        {
            var detail = db.CHITIETDONHANGs.Where(d => d.MaDonHang == id).ToList();
            ViewBag.Detail = detail;
            var order = db.DONHANGs.FirstOrDefault(d => d.MaDonHang == id);
            ViewBag.Total = order.TongTien;
            return View(order);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "Ten");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDonHang,DiaChi,TrangThai,NgayDat,ID")] DONHANG donHang)
        {
            if (ModelState.IsValid)
            {
                db.DONHANGs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "Ten", donHang.ID);
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
            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "Ten", donHang.ID);
            return View(donHang);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonHang,DiaChi,TrangThai,NgayDat,ID")] DONHANG donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCus = new SelectList(db.KHACHHANGs, "ID", "Ten ", donHang.ID);
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
            order.TrangThai = StatusOrder.Informed.ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CancelOrder(int id)
        {
            var order = db.DONHANGs.FirstOrDefault(item => item.MaDonHang == id);
            order.TrangThai = StatusOrder.Canceled.ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Shipping(int id)
        {
            var order = db.DONHANGs.FirstOrDefault(item => item.MaDonHang == id);
            order.TrangThai = StatusOrder.Shipping.ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ShippingSuccess(int id)
        {
            var order = db.DONHANGs.FirstOrDefault(item => item.MaDonHang == id);
            order.TrangThai = StatusOrder.Received.ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
