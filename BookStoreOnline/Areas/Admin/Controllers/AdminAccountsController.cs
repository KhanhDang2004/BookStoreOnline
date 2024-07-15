using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Areas.Admin.Constants;
using BookStoreOnline.Core;
using BookStoreOnline.Models;
using static BookStoreOnline.Areas.Admin.Constants.Constants;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class AdminAccountsController : Controller
    {
        private NhaSachEntities db = new NhaSachEntities();

        // GET: Admin/NHANVIENs.
        public ActionResult Index()
        {
            return View(db.NHANVIENs.ToList());
        }

        // GET: Admin/NHANVIENs.Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: Admin/NHANVIENs.Create
        public ActionResult Create()
        {
            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Value = e.ToString(), Text = e.GetDescription() };

            ViewBag.Quyen = new SelectList(roles, "Value", "Text");
            return View();
        }

        // POST: Admin/NHANVIENs.Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ten,Email,Quyen")] NHANVIEN nhanVienMoi)
        {
            if (ModelState.IsValid)
            {
                nhanVienMoi.MatKhau = Extension.GetMd5Hash("123456789");
                nhanVienMoi.NgayTao = DateTime.Now;
                nhanVienMoi.TrangThai = true;
                db.NHANVIENs.Add(nhanVienMoi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhanVienMoi);
        }

        // GET: Admin/NHANVIENs.Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Value = e.ToString(), Text = e.GetDescription() };

            ViewBag.Quyen = new SelectList(roles, "Value", "Text");
            return View(nhanVien);
        }

        // POST: Admin/NHANVIENs.Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,Ten,Email,Quyen")] NHANVIEN nhanVien)
        {
            if (ModelState.IsValid)
            {
                NHANVIEN admAccount = db.NHANVIENs.Find(nhanVien.MaNV);
                admAccount.Ten = nhanVien.Ten;
                admAccount.Quyen = nhanVien.Quyen;
                admAccount.Email = nhanVien.Email;
                db.Entry(admAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhanVien);
        }

        // GET: Admin/NHANVIENs.Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NHANVIENs.Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            db.NHANVIENs.Remove(nhanVien);
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


        public ActionResult DisableAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        [HttpPost, ActionName("DisableAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableAccountConfirmed(int id)
        {
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            nhanVien.TrangThai = false; // Assuming there is a property 'IsActive' in the AdminAccount model to indicate if the account is active or not
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        public ActionResult EnableAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        [HttpPost, ActionName("EnableAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult EnableAccountAccountConfirmed(int id)
        {
            NHANVIEN nhanVien = db.NHANVIENs.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            nhanVien.TrangThai = true; // Assuming there is a property 'IsActive' in the AdminAccount model to indicate if the account is active or not
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
