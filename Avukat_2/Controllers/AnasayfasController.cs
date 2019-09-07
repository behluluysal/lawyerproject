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
    public class AnasayfasController : Controller
    {
        int x = 1;
        private OurDbContext db = new OurDbContext();

        // GET: Anasayfas
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Index()
        {
            return View(db.anasayfa.ToList());
        }





        // GET: Anasayfas/Edit/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Edit()
        {
            
            Anasayfa anasayfa = db.anasayfa.Find(x);
            if (anasayfa == null)
            {
                return HttpNotFound();
            }
            return View(anasayfa);
        }

        // POST: Anasayfas/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "anasayfaid,bir,iki,uc,dort,bes,alti,yedi,sekiz,dokuz,on,onbir,oniki,onuc,ondort,onbes,onalti,onyedi,onsekiz,ondokuz,yirmi,yirmibir,yirmiiki,yirmiuc,yirmidort,yirmibes,yirmialti,yirmiyedi,yirmisekiz,yirmidokuz,otuz,otuzbir,otuziki,otuzuc,otuzdort,otuzbes,otuzalti,otuzyedi,otuzsekiz,otuzdokuz,kirk,kirkbir,kirkiki,kirkuc,kirkdort,kirkbes,kirkalti,kirkyedi,kirksekiz,kirkdokuz,elli,ellibir,elliiki,elliuc,ellidort,ellibes,ellialti,elliyedi,ellisekiz,ellidokuz,altmis,altmisbir,altmisiki,altmisuc,altmisdort")] Anasayfa anasayfa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(anasayfa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(anasayfa);
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
