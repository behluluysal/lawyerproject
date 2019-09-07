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
using Avukat_2.ViewModels;
using System.Globalization;

namespace Avukat_2.Controllers
{

    public class AdminController : Controller
    {
        private OurDbContext db = new OurDbContext();

        // GET: Accounts
       
        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string name,string password)
        {
            using (OurDbContext db = new OurDbContext())
            {
                var usr = db.userAccounts.Where(u => u.Username == name && u.Password == password).FirstOrDefault();
                if (usr != null)
                {
                    SessionPersister.Username = name;
                    if (usr.Roletr == "superadmin")
                    {
                        Session["modtype"] = "Yönetici";
                        return RedirectToAction("panel", "Admin");
                    }
                    else
                    {
                        Session["modtype"] = "Yardımcı";
                        return RedirectToAction("davakayit", "Admin");
                    }
                        
                }
                else
                    ModelState.AddModelError("", "Hatali giris");
            }
            return View();
        }

       

        // GET: Accounts/Edit/5
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin account = db.userAccounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Edit([Bind(Include = "UserID,Username,Password,Roletr,email,AktivasyonKodu")] Admin account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }


        public ActionResult Logout(AccountViewModel avm)
        {
            SessionPersister.Username = string.Empty;
            Session["modtype"] = null;
            return RedirectToAction("Login", "Admin");
        }






        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult panel()
        {


            panelViewModel pnl = new panelViewModel();
            pnl.musterilers = db.musterilers.ToList();
            pnl.davalars = db.davalars.ToList();
            pnl.bdavalars = db.bitendavalars.ToList();
            pnl.avukats = db.avukats.ToList();
            ViewBag.buay = db.bitendavalars.Where(x => x.BitisTarihi.Month == DateTime.Now.Month);
            if(DateTime.Now.Month == 1)
            {
                ViewBag.oncekiay = db.bitendavalars.Where(x => (x.BitisTarihi.Month == 12) && x.BitisTarihi.Year == DateTime.Now.Year-1);
            }
            else
            {
                ViewBag.oncekiay = db.bitendavalars.Where(x => (x.BitisTarihi.Month == DateTime.Now.Month-1) && x.BitisTarihi.Year == DateTime.Now.Year);
            }

            
            
            return View(pnl);
        }

        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult bireyseldavalar()
        {
            OurDbContext db = new OurDbContext();
            DavakayitViewModel dkb = new DavakayitViewModel();
            dkb.bitenDavalars = db.bitendavalars.ToList();
            dkb.davalars = db.davalars.ToList();
            dkb.avukats = db.avukats.ToList();

            return View(dkb);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult bireyseldavalar(string davaId, string durum)
        {
            OurDbContext db = new OurDbContext();
            DavakayitViewModel dkb = new DavakayitViewModel();
            try
            {
                dkb.bitenDavalars = db.bitendavalars.ToList();
                dkb.davalars = db.davalars.ToList();
                dkb.avukats = db.avukats.ToList();
                Object mst;
                if (durum == "aktif")
                {
                    mst = db.davalars.Where(x => x.DavaId.ToString() == davaId).FirstOrDefault();
                }
                else
                {
                    mst = db.bitendavalars.Where(x => x.DavaId.ToString() == davaId).FirstOrDefault();

                }
                string yol = "bireysel";

                Davalar test = null;
                BitenDavalar testb = null;
                try
                {
                    test = (Davalar)mst;
                }
                catch (Exception)
                {
                    testb = (BitenDavalar)mst;
                }
                string subyol = "/uploads/" + yol + "/";
                if (test != null)
                {
                    subyol += test.Musteriler.musteriTc + "/aktifdavalar/" + test.DavaAdi;
                }
                else if (testb != null)
                {
                    if (testb.Musteriler != null)
                    {
                        subyol += testb.Musteriler.musteriTc + "/bitendavalar/" + testb.DavaAdi;
                    }
                    else
                    {
                        subyol += testb.BMusteriler.musteriTc + "/bitendavalar/" + testb.DavaAdi;
                    }
                }

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase http = Request.Files[file];

                    //dosya ekleme işlemi
                    string tamyol = subyol + "/" + http.FileName;
                    Request.Files[file].SaveAs(Server.MapPath(tamyol));
                }
                ViewBag.alertler = "success";
            }
            catch (Exception)
            {
                ViewBag.alertler = "error";
            }


            
            return View(dkb);
        }





        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult bireyselmusteriler()
        {
            OurDbContext db = new OurDbContext();
            MusteriListeleViewModel ml = new MusteriListeleViewModel();
            ml.musteriler = db.musterilers.ToList();
            ml.bitenmusteriler = db.bitenmusterilers.ToList();
            TempData["duzenle"] = "";

            return View(ml);
        }

        [CustomAuthorize(Roles = "superadmin,admin")]
        [HttpPost]
        public ActionResult bireyselmusteriler(string name, string sname,string tc, string yas, DateTime dt, string adres, string phone, string evphone, string email, string musteriid)
        {
            MusteriListeleViewModel ml = new MusteriListeleViewModel();
            using(OurDbContext db = new OurDbContext())
            {
                ml.bitenmusteriler = db.bitenmusterilers.ToList();
                ml.musteriler = db.musterilers.ToList();
                int a = int.Parse(musteriid, CultureInfo.CurrentCulture);
                Musteriler must = db.musterilers.Where(x => x.UserID == a).FirstOrDefault();
                foreach(var item in db.musterilers)
                {
                    if (item.UserID != a)
                    {
                        if(item.musteriTc == tc)
                        {
                            TempData["duzenle"] = "tcmatch";
                            return View(ml);
                        }
                    }
                }
                foreach (var item in db.bitenmusterilers)
                {
                    if (item.musteriTc == tc)
                    {
                        TempData["duzenle"] = "tcmatch";
                        return View(ml);
                    }
                }
                must.musteriAd = name;
                must.musteriSoyad = sname;
                must.musteriTc = tc;
                must.musteriDT = dt;
                must.musteriAdres = adres;
                must.PhoneNumber = phone;
                must.EvTelNo = evphone;
                must.email = email;
                db.SaveChanges();
                TempData["duzenle"] = "success";
            }    

            return View(ml);
        }

        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult kurumsalmusteriler()
        {
            OurDbContext db = new OurDbContext();
            MusteriListeleViewModel ml = new MusteriListeleViewModel();
            ml.musteriler = db.musterilers.ToList();
            ml.bitenmusteriler = db.bitenmusterilers.ToList();
            TempData["kontrol"] = "";

            return View(ml);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult kurumsalmusteriler(string name, string tc, string adres, string phone, string evphone, string email, string musteriid)
        {
            MusteriListeleViewModel ml = new MusteriListeleViewModel();
            using (OurDbContext db = new OurDbContext())
            {
                ml.bitenmusteriler = db.bitenmusterilers.ToList();
                ml.musteriler = db.musterilers.ToList();
                int a = int.Parse(musteriid, CultureInfo.CurrentCulture);
                Musteriler must = db.musterilers.Where(x => x.UserID == a).FirstOrDefault();
                foreach (var item in db.musterilers)
                {
                    if (item.UserID != a)
                    {
                        if (item.musteriTc == tc)
                        {
                            TempData["kontrol"] = "tcmatch";
                            return View(ml);
                        }
                    }
                }
                foreach (var item in db.bitenmusterilers)
                {
                    if (item.musteriTc == tc)
                    {
                        TempData["kontrol"] = "tcmatch";
                        return View(ml);
                    }
                }
                must.musteriAd = name;
                must.musteriSoyad = null;
                must.musteriTc = tc;
                must.musteriDT = DateTime.Now;
                must.musteriAdres = adres;
                must.PhoneNumber = phone;
                must.FaxNo = evphone;
                must.email = email;
                db.SaveChanges();
                TempData["kontrol"] = "success";
            }

            return View(ml);
        }






        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult davakayit()
        {
            
            DavakayitViewModel dkn = new DavakayitViewModel();
            dkn.avukats = db.avukats.ToList();
            return View(dkn);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult davakayit( string test, string tip ,string tc,string davaad)
        {
            DavakayitViewModel dkn = new DavakayitViewModel();
            dkn.avukats = db.avukats.ToList();
            string yol=tip; //kurumsal bireysel için dosya yolu
 
            try
            {
                string subyol = "/uploads/" + yol + "/" + tc;
                //müşteri klasör işlemi
                
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase http = Request.Files[file];
                    
                    //dosya ekleme işlemi
                    string tamyol = "/uploads/" + yol + "/" + tc+"/aktifdavalar/"+davaad+"/"+ http.FileName;
                    Request.Files[file].SaveAs(Server.MapPath(tamyol));
                    
                }
                ViewBag.alertler = "success";
            }
            catch (Exception)
            {
                ViewBag.alertler = "error";
                throw;
                
            }
            return View(dkn);

            
        }


        [HttpPost]
        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult davakapat(string kontrolInput)
        {
            if (kontrolInput == "success")
            {
                ViewBag.alert = "success";
            }
            else if (kontrolInput == "error")
            {
                ViewBag.alert = "error";
            }
            else
            {
                ViewBag.alert = "";
            }
            return View();
        }


        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult davakapat()
        {
            return View();
        }





        [CustomAuthorize(Roles = "superadmin,admin")]
        public ActionResult kurumsaldavalar()
        {
            DavakayitViewModel dtb = new DavakayitViewModel();
            dtb.bitenDavalars = db.bitendavalars.ToList();
            dtb.avukats = db.avukats.ToList();
            dtb.davalars = db.davalars.ToList();

            return View(dtb);
        }


        [CustomAuthorize(Roles = "superadmin,admin")]
        [HttpPost]
        public ActionResult kurumsaldavalar(string davaId, string durum)
        {
            try
            {
                OurDbContext db = new OurDbContext();
                DavakayitViewModel dkb = new DavakayitViewModel();
                dkb.bitenDavalars = db.bitendavalars.ToList();
                dkb.davalars = db.davalars.ToList();
                dkb.avukats = db.avukats.ToList();
                Object mst;
                if (durum == "aktif")
                {
                    mst = db.davalars.Where(x => x.DavaId.ToString() == davaId).FirstOrDefault();
                }
                else
                {
                    mst = db.bitendavalars.Where(x => x.DavaId.ToString() == davaId).FirstOrDefault();

                }
                string yol = "kurumsal";

                Davalar test = null;
                BitenDavalar testb = null;
                try
                {
                    test = (Davalar)mst;
                }
                catch (Exception)
                {
                    testb = (BitenDavalar)mst;
                }
                string subyol = "/uploads/" + yol + "/";
                if (test != null)
                {
                    subyol += test.Musteriler.musteriTc + "/aktifdavalar/" + test.DavaAdi;
                }
                else if (testb != null)
                {
                    if (testb.Musteriler != null)
                    {
                        subyol += testb.Musteriler.musteriTc + "/bitendavalar/" + testb.DavaAdi;
                    }
                    else
                    {
                        subyol += testb.BMusteriler.musteriTc + "/bitendavalar/" + testb.DavaAdi;
                    }
                }

                try
                {
                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase http = Request.Files[file];

                        //dosya ekleme işlemi
                        string tamyol = subyol + "/" + http.FileName;
                        Request.Files[file].SaveAs(Server.MapPath(tamyol));
                       
                    }
                   
                }
                catch (Exception)
                {
                    throw;

                }
                return View(dkb);


            }
            catch (Exception)
            {

                throw;
            }
           
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
