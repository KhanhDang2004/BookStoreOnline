using BookStoreOnline.Core;
using BookStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class ControlUserController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();
        // GET: Admin/ControlUser
        public ActionResult Index()
        {
            var users = db.Customers.ToList();
            return View(users);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/Nguoidungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer nguoidung = db.Customers.Find(id);

            db.Customers.Remove(nguoidung);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ResetPassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id)
        {
            Customer nguoidung = db.Customers.Find(id);

            nguoidung.Password = Extension.GetMd5Hash("123456789");
               
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DisableAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("DisableAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableAccountConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            customer.IsActive = false; 
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}