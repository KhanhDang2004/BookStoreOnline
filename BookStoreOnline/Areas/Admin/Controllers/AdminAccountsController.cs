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
        private BookStoreEntities db = new BookStoreEntities();

        // GET: Admin/AdminAccounts
        public ActionResult Index()
        {
            return View(db.AdminAccounts.ToList());
        }

        // GET: Admin/AdminAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            if (adminAccount == null)
            {
                return HttpNotFound();
            }
            return View(adminAccount);
        }

        // GET: Admin/AdminAccounts/Create
        public ActionResult Create()
        {
            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Id = (int)e, Name = e.GetDescription() };

            ViewBag.Role = new SelectList(roles, "Id", "Name");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Email,Role")] AdminAccount adminAccount)
        {
            if (ModelState.IsValid)
            {
                adminAccount.Password = Extension.GetMd5Hash("123456789");
                adminAccount.CreatedOn = DateTime.Now;
                db.AdminAccounts.Add(adminAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminAccount);
        }

        // GET: Admin/AdminAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            if (adminAccount == null)
            {
                return HttpNotFound();
            }
            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Id = (int)e, Name = e.GetDescription() };

            ViewBag.Role = new SelectList(roles, "Id", "Name");
            return View(adminAccount);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminID, Name,Email,Role")] AdminAccount adminAccount)
        {
            if (ModelState.IsValid)
            {
                AdminAccount admAccount = db.AdminAccounts.Find(adminAccount.AdminID);
                admAccount.Name = adminAccount.Name;
                admAccount.Role = adminAccount.Role;
                admAccount.Email = adminAccount.Email;
                db.Entry(admAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminAccount);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            if (adminAccount == null)
            {
                return HttpNotFound();
            }
            return View(adminAccount);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            db.AdminAccounts.Remove(adminAccount);
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
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            if (adminAccount == null)
            {
                return HttpNotFound();
            }
            return View(adminAccount);
        }

        [HttpPost, ActionName("DisableAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableAccountConfirmed(int id)
        {
            AdminAccount adminAccount = db.AdminAccounts.Find(id);
            if (adminAccount == null)
            {
                return HttpNotFound();
            }

            adminAccount.IsActive = false; // Assuming there is a property 'IsActive' in the AdminAccount model to indicate if the account is active or not
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
