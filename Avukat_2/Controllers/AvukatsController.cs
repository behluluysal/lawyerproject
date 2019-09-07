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
    public class AvukatsController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: Avukats
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Index()
        {
            return View(db.avukats.ToList());
        }

        // GET: Avukats/Details/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avukat avukat = db.avukats.Find(id);
            if (avukat == null)
            {
                return HttpNotFound();
            }
            return View(avukat);
        }

        // GET: Avukats/Create
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Avukats/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [CustomAuthorize(Roles = "superadmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AvukatID,AvukatAd,AvukatSoyad,telno,mail")] Avukat avukat)
        {
            if (ModelState.IsValid)
            {
                db.avukats.Add(avukat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(avukat);
        }

        // GET: Avukats/Edit/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avukat avukat = db.avukats.Find(id);
            if (avukat == null)
            {
                return HttpNotFound();
            }
            return View(avukat);
        }

        // POST: Avukats/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [CustomAuthorize(Roles = "superadmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AvukatID,AvukatAd,AvukatSoyad,telno,mail")] Avukat avukat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avukat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(avukat);
        }

        // GET: Avukats/Delete/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avukat avukat = db.avukats.Find(id);
            if (avukat == null)
            {
                return HttpNotFound();
            }
            return View(avukat);
        }


        [CustomAuthorize(Roles = "superadmin")]
        // POST: Avukats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Avukat avukat = db.avukats.Find(id);
            db.avukats.Remove(avukat);
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
    }
}
