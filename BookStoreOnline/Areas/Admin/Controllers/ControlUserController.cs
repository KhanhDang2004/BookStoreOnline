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
    }
}