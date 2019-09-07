using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Avukat_2.Models;
using Avukat_2.Security;

namespace Avukat_2.Controllers
{
    public class superadminController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: superadmin
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Index()
        {
            return View(db.userAccounts.ToList());
        }

        // GET: superadmin/Details/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.userAccounts.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: superadmin/Create
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: superadmin/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Create([Bind(Include = "AdminID,Username,Password,Roletr,email,AktivasyonKodu")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.userAccounts.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: superadmin/Edit/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.userAccounts.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: superadmin/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Edit([Bind(Include = "AdminID,Username,Password,Roletr,email,AktivasyonKodu")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: superadmin/Delete/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.userAccounts.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: superadmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(Roles = "superadmin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.userAccounts.Find(id);
            if(! admin.Roletr.Contains("superadmin"))
            {
                db.userAccounts.Remove(admin);
                db.SaveChanges();
            }
            else
            {
                Session["modsilme"] = "modhatasi";
                return RedirectToAction("Index");
            }
            
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
    }
}
