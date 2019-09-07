using Avukat_2.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Avukat_2.Controllers
{
    public class ScriptsController : Controller
    {
        string result = "";
        char[] letters = {'a','b','c','ç','d','e','f','g','ğ','h','ı','i','j','k','l','m','n','o','ö'
                         ,'p','r','s','ş','t','u','ü','v','y','z'}; //29 karakter var
        string[] letterss = {"a","b","c","ç","d","e","f","g","ğ","h","ı","i","j","k","l","m","n","o"
                          ,"ö","p","r","s","ş","t","u","ü","v","y","z"};

        //---------------------------------------------------------------------------------------------------------------------------
        // GET: Scripts
        public string CheckTC(string input)
        {
            using (OurDbContext db = new OurDbContext())
            {
                Musteriler usr = null;
                usr = db.musterilers.Where(u => u.musteriTc == input).FirstOrDefault();
                if (usr != null)
                    return "Not Avaliable";
                if (usr == null)
                    return "Avaliable";
            }
            return "";
        }
        //---------------------------------------------------------------------------------------------


    
        public JsonResult CheckTC1(string kname, string tip)
        {
            string[] test = new string[12];
            using (OurDbContext db = new OurDbContext())
            {
                Musteriler usr = null;
                usr = db.musterilers.Where(u => u.musteriTc == kname).FirstOrDefault();
                if ((usr != null) && ( (tip =="bireysel" && usr.musteriSoyad != null) || (tip=="kurumsal" && usr.musteriSoyad== null) ))
                {
                    test[0] = "Avaliable";
                    test[1] = usr.musteriAd;
                    test[2] = usr.musteriSoyad;
                    test[3] = usr.musteriTc;
                    test[4] = usr.musteriCinsiyet;
                    test[5] = usr.musteriDT.ToString(CultureInfo.CurrentCulture);
                    test[6] = usr.email;
                    test[7] = usr.PhoneNumber;
                    test[8] = usr.musteriAdres;
                    test[9] = usr.UserID.ToString(CultureInfo.CurrentCulture);
                    if(tip == "bireysel")
                        test[10] = usr.EvTelNo;
                    else
                        test[10] = usr.FaxNo;
                    test[11] = usr.kayittarihi.ToShortDateString();
                    return Json(test, JsonRequestBehavior.AllowGet);
                }
                    
                else if (usr == null)
                {
                    BitenMusteriler usr2 = null;
                    usr2 = db.bitenmusterilers.Where(u => u.musteriTc == kname).FirstOrDefault();
                    if(usr2 !=  null && ((tip == "bireysel" && usr2.musteriSoyad != null) || (tip == "kurumsal" && usr2.musteriSoyad == null)))
                    {
                        test[0] = "Not Avaliable";
                        test[1] = usr2.musteriAd;
                        test[2] = usr2.musteriSoyad;
                        test[3] = usr2.musteriTc;
                        test[4] = usr2.musteriCinsiyet;
                        test[5] = usr2.musteriDT.ToString(CultureInfo.CurrentCulture);
                        test[6] = usr2.email;
                        test[7] = usr2.PhoneNumber;
                        test[8] = usr2.musteriAdres;
                        test[9] = usr2.UserID.ToString(CultureInfo.CurrentCulture);
                        if (tip == "bireysel")
                            test[10] = usr2.EvTelNo;
                        else
                            test[10] = usr2.FaxNo;
                        test[11] = usr2.kayittarihi.ToShortDateString();
                        return Json(test, JsonRequestBehavior.AllowGet);
                    }
                    else if(usr2 == null)
                    {
                        test[0] = "Not Not Avaliable";
                        return Json(test, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                    
            }
            test[0] = "Not Not Avaliable";
            return Json(test, JsonRequestBehavior.AllowGet);
        }



        public string Checkemail(string input)
        {
            using (OurDbContext db = new OurDbContext())
            {
                Musteriler usr = null;
                usr = db.musterilers.Where(u => u.email == input).FirstOrDefault();
                if (usr != null)
                    return "Not Avaliable";
                if (usr == null)
                {
                    if (RegexUtilities.IsValidEmail(input))
                        return "Avaliable";
                    else
                        return "wrong format";
                }

            }
            return "";
        }


        public string kayit(string kname, string klname, string ktc, string kcinsiyet, DateTime kdob, string kemail, string kadres, string kphone)
        {
            using (OurDbContext db = new OurDbContext())
            {
                Musteriler usr = null;
                usr = db.musterilers.Where(u => u.musteriTc == ktc).FirstOrDefault();
                if (usr != null)
                {
                    TempData["registererror"] = "error";
                    return "tcmatch";
                }
                    
                else
                {
                    Musteriler usr2 = new Musteriler();
                    usr2.musteriAd = kname;
                    usr2.musteriSoyad = klname;
                    usr2.musteriTc = ktc;
                    usr2.musteriCinsiyet = kcinsiyet;
                    usr2.musteriDT = kdob;

                    DateTime now = DateTime.Now;
                    int age = now.Year - kdob.Year;
                    if (now < kdob.AddYears(age))
                        age--;
                    usr2.musteriYas = age;
                    usr2.kayittarihi = now;
                    usr2.email = kemail;
                    usr2.musteriAdres = kadres;
                    usr2.PhoneNumber = kphone;
                    db.musterilers.Add(usr2);
                    db.SaveChanges();
                    TempData["registererror"] = "access";
                    return "kayitbasarili";
                }

            }
        }



        public string Davakaydet(string musteridurumu, string musteriad, string musteritc, string musteriemail, string musteritel,
            string musterievfax, string musteriadres, string kayittarihi,/*bireysel*/string musteridt, string musterisoyad,
            /*dava*/string davaadi, string davadosya, string davaaciklama, string davaucret, string davatarihi, string davano, 
            string davatip, string davaavukat, string musteriid, string cinsiyet)
        {
            try
            {
                string sifre="";
            DateTime kayittarihi2 = Convert.ToDateTime(kayittarihi, CultureInfo.CurrentCulture);
            DateTime musteridt2 = Convert.ToDateTime(musteridt, CultureInfo.CurrentCulture);
            DateTime davatarihi2 = Convert.ToDateTime(davatarihi, CultureInfo.CurrentCulture);
            if (davadosya == "Dava dosyasını yüklediğinizde sistem otomatik olarak dolduracaktır.")
            {
                davadosya = "";
            }
            string durum = "";
            if(musteridurumu == "nk" || musteridurumu == "bk" || musteridurumu == "kk")
            {
                durum = "kurumsal";
            }
            else
            {
                durum = "bireysel";
            }
            string subyol = "/uploads/" + durum + "/" + musteritc;
            bool exists = System.IO.Directory.Exists(Server.MapPath(subyol));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subyol));

                //dava klasör işlemi
                exists = System.IO.Directory.Exists(Server.MapPath(subyol + "/aktifdavalar"));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subyol + "/aktifdavalar"));
                exists = System.IO.Directory.Exists(Server.MapPath(subyol + "/bitendavalar"));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subyol + "/bitendavalar"));

                exists = System.IO.Directory.Exists(Server.MapPath(subyol + "/aktifdavalar/" + davaadi));
                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subyol + "/aktifdavalar/" + davaadi));

          
                int musteriidisi;
                using (OurDbContext db = new OurDbContext())
                {
                    bool sifrekontrol = true;
                    while (sifrekontrol)
                    {
                        sifre = randomcharacter2(7);
                        if (db.musterilers.Where(x => x.MusteriSifre == sifre).Any())
                        {
                            sifrekontrol = true;
                        }
                        else if (db.bitenmusterilers.Where(x => x.MusteriSifre == sifre).Any())
                        {
                            sifrekontrol = true;
                        }
                        else
                            sifrekontrol = false;
                    }
                    Musteriler must = new Musteriler();
                    BitenMusteriler bmust = new BitenMusteriler();

                    Davalar dv = new Davalar();
                    

                    Avukat avukat = new Avukat();
                    int avukatid = int.Parse(davaavukat, CultureInfo.CurrentCulture);

                    //dava ekleme işlemi
                    avukat = db.avukats.Where(x => x.AvukatID == avukatid).FirstOrDefault();
                    dv.Avukat = avukat;
                    dv.BaslangicTarihi = davatarihi2;
                    dv.DavaAciklama = davaaciklama;
                    dv.DavaAdi = davaadi;
                    dv.DavaNo = davano;
                    dv.davaturu = (Davalar.Davaturu)int.Parse(davatip, CultureInfo.CurrentCulture);
                    dv.DavaUcret = int.Parse(davaucret, CultureInfo.CurrentCulture);
                    dv.DosyaAdi = davadosya;


                    if (musteridurumu == "k" || musteridurumu == "kk")
                    {
                        musteriidisi = int.Parse(musteriid, CultureInfo.CurrentCulture);
                        must = new Musteriler();
                        must = db.musterilers.Where(x => x.UserID == musteriidisi).FirstOrDefault();
                        dv.Musteriler = must;
                        db.davalars.Add(dv);
                        db.SaveChanges();
                    }
                    else if (musteridurumu == "b" || musteridurumu == "bk")
                    {
                        musteriidisi = int.Parse(musteriid, CultureInfo.CurrentCulture);
                        bmust = db.bitenmusterilers.Where(x => x.UserID == musteriidisi).FirstOrDefault();
                        must = new Musteriler();
                        must.MusteriSifre = bmust.MusteriSifre;
                        must.email = musteriemail;
                        if (musteridurumu == "b")
                            must.EvTelNo = musterievfax;
                        else
                            must.FaxNo = musterievfax;
                        //--------------------------
                        if (musteridurumu == "b")
                            must.musteriSoyad = musterisoyad;
                        else
                            must.musteriSoyad = null;
                        must.kayittarihi = kayittarihi2;
                        must.musteriAd = musteriad;
                        must.musteriAdres = musteriadres;
                        must.musteriCinsiyet = bmust.musteriCinsiyet;
                        must.musteriDT = musteridt2;
                        must.musteriTc = bmust.musteriTc;
                        must.PhoneNumber = musteritel;
                        must.ucret = bmust.ucret;
                        db.musterilers.Add(must);
                        db.SaveChanges();

                        //dava ekleme işlemi

                        dv.Musteriler = must;
                        db.davalars.Add(dv);
                        db.SaveChanges();

                        //dava taşıma işlemi
                        var degisimdavalari = db.bitendavalars.Where(x => x.BMusteriler.UserID == musteriidisi).ToList();
                        degisimdavalari.ForEach(a => a.BMusteriler = null);
                        degisimdavalari.ForEach(b => b.Musteriler = must);

                        //müşteri silme işlemi
                        db.bitenmusterilers.Remove(bmust);
                        db.SaveChanges();

                    }
                    else if (musteridurumu == "nk")
                    {
                        must = new Musteriler();
                        must.musteriSoyad = null;
                        must.MusteriSifre = sifre;
                        must.email = musteriemail;
                        must.EvTelNo = null;
                        must.FaxNo = musterievfax;
                        must.kayittarihi = kayittarihi2;
                        must.musteriAd = musteriad;
                        must.musteriAdres = musteriadres;
                        must.musteriCinsiyet = null;
                        must.musteriDT = DateTime.Now;
                        must.musteriTc = musteritc;
                        must.musteriYas = 0;
                        must.PhoneNumber = musteritel;
                        must.ucret = 0;


                        dv.Musteriler = must;
                        db.davalars.Add(dv);
                        db.SaveChanges();

                        ////mail işlemi
                        //MailMessage msg = new MailMessage();
                        ////Add your email address to the recipients
                        //msg.To.Add(musteriemail);
                        ////Configure the address we are sending the mail from
                        //MailAddress address = new MailAddress("info@hasturkyazilim.com");
                        //msg.From = address;
                        //msg.Subject = "Merhaba " + musteriad + " Müşteri Bilgileriniz Ektedir";
                        //msg.Body = "Sisteme giriş yapıp davalarınızı görüntüleyebilmek için \nKullanıcı Adınız: " + musteritc + " \nŞifreniz: " + sifre + "\nLütfen bilgilerinizi kaybetmeyiniz veya bu maili silmeyiniz.";
                        //msg.IsBodyHtml = false;
                        //SmtpClient client = new SmtpClient();
                        //client.Host = "relay-hosting.secureserver.net";
                        //client.Port = 25;
                        //client.EnableSsl = false;
                        ////Send the msg
                        //client.Send(msg);



                    }
                    else if (musteridurumu == "nm")
                    {
                        must = new Musteriler();
                        must.MusteriSifre = sifre;
                        must.musteriSoyad = musterisoyad;
                        must.email = musteriemail;
                        must.EvTelNo = musterievfax;
                        must.FaxNo = null;
                        must.kayittarihi = kayittarihi2;
                        must.musteriAd = musteriad;
                        must.musteriAdres = musteriadres;
                        must.musteriCinsiyet = null;
                        must.musteriDT = musteridt2;
                        must.musteriTc = musteritc;
                        must.musteriYas = 100;
                        must.PhoneNumber = musteritel;
                        must.ucret = 0;
                        must.musteriCinsiyet = cinsiyet;


                        dv.Musteriler = must;
                        db.davalars.Add(dv);
                        db.SaveChanges();

                        ////mail işlemi
                        //MailMessage msg = new MailMessage();
                        ////Add your email address to the recipients
                        //msg.To.Add(musteriemail);
                        ////Configure the address we are sending the mail from
                        //MailAddress address = new MailAddress("info@hasturkyazilim.com");
                        //msg.From = address;
                        //msg.Subject = "Merhaba " + musteriad + " Müşteri Bilgileriniz Ektedir";
                        //msg.Body = "Sisteme giriş yapıp davalarınızı görüntüleyebilmek için \nKullanıcı Adınız: " + musteritc + " \nŞifreniz: " + sifre + "\nLütfen bilgilerinizi kaybetmeyiniz veya bu maili silmeyiniz.";
                        //msg.IsBodyHtml = false;
                        //SmtpClient client = new SmtpClient();
                        //client.Host = "relay-hosting.secureserver.net";
                        //client.Port = 25;
                        //client.EnableSsl = false;
                        ////Send the msg
                        //client.Send(msg);



                    }

                }
                return "success";
            }
            catch (Exception)
            {
                return "error";
            }
            
        }



        public string ucretGuncelle(string ID, string TC, string yeni, string tercih)
        {
            using (OurDbContext db = new OurDbContext())
            {
                if (tercih == "tc")
                {
                    Davalar guncellenecek = db.davalars.Where(x => ((x.Musteriler.musteriTc == TC) && ((x.DavaAdi + x.DavaAciklama.ToString()).Equals(ID)))).FirstOrDefault();
                    if (guncellenecek != null)
                    {
                        guncellenecek.DavaUcret = Int32.Parse(yeni, CultureInfo.CurrentCulture);
                        db.SaveChanges();
                        return "basarili";
                    }
                    return "basarisiz";
                }
                else
                {
                    Davalar guncellenecek = db.davalars.Where(x => x.DavaNo == TC).FirstOrDefault();
                    if (guncellenecek != null)
                    {
                        guncellenecek.DavaUcret = Int32.Parse(yeni, CultureInfo.CurrentCulture);
                        db.SaveChanges();
                        return "basarili";
                    }
                    return "basarisiz";
                }
            }
        }


        public string davaSil(string ID, string TC, string secimm)
        {
            try
            {
                using (OurDbContext db = new OurDbContext())
                {
                    Davalar silinecek = null;
                    if (secimm == "tc")
                    {
                        silinecek = db.davalars.Where(x => ((x.Musteriler.musteriTc == TC) && ((x.DavaAdi + x.DavaAciklama.ToString()).Equals(ID)))).FirstOrDefault();
                    }
                    else
                    {
                        silinecek = db.davalars.Where(x => x.DavaNo == TC).FirstOrDefault();
                    }
                    Musteriler mst = silinecek.Musteriler;
                    if (silinecek != null)
                    {
                        BitenDavalar yeni = new BitenDavalar();
                        yeni.DavaNo = silinecek.DavaNo;
                        yeni.DavaAdi = silinecek.DavaAdi;
                        yeni.BaslangicTarihi = silinecek.BaslangicTarihi;
                        yeni.BitisTarihi = DateTime.Now;
                        yeni.DavaAciklama = silinecek.DavaAciklama;
                        yeni.davaturu = (BitenDavalar.Davaturub)silinecek.davaturu;
                        yeni.DosyaAdi = silinecek.DosyaAdi;
                        yeni.DavaUcret = silinecek.DavaUcret;
                        yeni.Avukat = silinecek.Avukat;
                        yeni.DosyaAdi = silinecek.DosyaAdi;
                        string silinecekYol = "/uploads/";
                        string targetYol = "/uploads/";
                        if (silinecek.Musteriler.musteriSoyad == null)
                        {
                            silinecekYol += "kurumsal/";
                            targetYol += "kurumsal/";
                        }
                        else
                        {
                            silinecekYol += "bireysel/";
                            targetYol += "bireysel/";
                        }
                        string davaAdi = silinecek.DavaAdi;
                        if (secimm == "tc")
                        {
                            silinecekYol += TC + "/aktifdavalar/" + davaAdi;
                            targetYol += TC + "/bitendavalar/" + davaAdi;
                        }
                        else
                        {
                            silinecekYol += silinecek.Musteriler.musteriTc + "/aktifdavalar/" + davaAdi;
                            targetYol += silinecek.Musteriler.musteriTc + "/bitendavalar/" + davaAdi;
                        }


                        bool exists = System.IO.Directory.Exists(Server.MapPath(targetYol));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(targetYol));

                        string[] dosyalar = silinecek.DosyaAdi.Split('\\');
                        for (int i = 0; i < dosyalar.Length - 1; i++)
                        {
                            string sourceFile = Server.MapPath(Path.Combine(silinecekYol, dosyalar[i]));
                            string destFile = Server.MapPath(Path.Combine(targetYol, dosyalar[i]));
                            Directory.Move(sourceFile, destFile);
                        }
                        Directory.Delete(Server.MapPath(silinecekYol), true);
                        // Müşterinin son Davası ise --->>
                        if (mst.Davalars.Count == 1)
                        {
                            BitenMusteriler btn = new BitenMusteriler();

                            var degisimdavalari = db.bitendavalars.Where(x => x.Musteriler.UserID == mst.UserID).ToList();
                            degisimdavalari.ForEach(a => a.BMusteriler = btn);
                            degisimdavalari.ForEach(b => b.Musteriler = null);

                            btn.bitistarihi = yeni.BitisTarihi;
                            btn.dosyaYolu = mst.dosyaYolu;
                            btn.email = mst.email;
                            btn.kayittarihi = mst.kayittarihi;
                            btn.musteriAd = mst.musteriAd;
                            btn.musteriAdres = mst.musteriAdres;
                            btn.musteriCinsiyet = mst.musteriCinsiyet;
                            btn.musteriDT = mst.musteriDT;
                            btn.musteriSoyad = mst.musteriSoyad;
                            btn.musteriTc = mst.musteriTc;
                            btn.PhoneNumber = mst.PhoneNumber;
                            btn.EvTelNo = mst.EvTelNo;
                            btn.FaxNo = mst.FaxNo;
                            btn.MusteriSifre = mst.MusteriSifre;
                            btn.ucret = mst.ucret + silinecek.DavaUcret;
                            btn.UserID = mst.UserID;
                            yeni.BMusteriler = btn;
                            yeni.Musteriler = null;
                            db.bitenmusterilers.Add(btn);
                            db.musterilers.Remove(mst);
                            db.SaveChanges();
                        }
                        //Son davası değill ise -->>
                        else
                        {
                            mst.ucret += silinecek.DavaUcret;
                            yeni.BMusteriler = null;
                            yeni.Musteriler = silinecek.Musteriler;
                        }
                        db.davalars.Remove(silinecek);
                        db.bitendavalars.Add(yeni);
                        db.SaveChanges();

                        return "basarili";
                    }
                    return "basarisiz";
                }
            }
            catch (Exception)
            {
                return "basarisiz";
                throw;
            }
          
        }



        public JsonResult davaKapaTc(string TC, string trch)
        {
            using (OurDbContext db = new OurDbContext())
            {
                if (trch == "tc")
                {
                    if (db.musterilers.Where(x => x.musteriTc == TC).FirstOrDefault() == null)
                    {
                        return Json("notfound", JsonRequestBehavior.AllowGet);
                    }
                    List<Davalar> davalars = db.davalars.Where(x => x.Musteriler.musteriTc == TC).ToList();
                    int davaSayi = db.davalars.Where(x => x.Musteriler.musteriTc == TC).Count();
                    int adet = (davaSayi * 5) + 1;
                    string[] bulunan = new string[adet];
                    bulunan[0] = adet.ToString(CultureInfo.CurrentCulture);
                    int sayac = 1;
                    for (int i = 0; i < davaSayi; i++)
                    {
                        bulunan[sayac++] = davalars[i].DavaAdi;
                        bulunan[sayac++] = davalars[i].BaslangicTarihi.ToString(CultureInfo.CurrentCulture);
                        bulunan[sayac++] = davalars[i].davaturu.ToString();
                        bulunan[sayac++] = davalars[i].DavaAciklama;
                        bulunan[sayac++] = davalars[i].DavaUcret.ToString(CultureInfo.CurrentCulture);
                    }
                    if (bulunan != null)
                    {
                        return Json(bulunan, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (db.davalars.Where(x => x.DavaNo.ToString() == TC).FirstOrDefault() == null)
                    {
                        return Json("notfound", JsonRequestBehavior.AllowGet);
                    }
                    List<Davalar> davalars = db.davalars.Where(x => x.DavaNo == TC).ToList();
                    int davaSayi = db.davalars.Where(x => x.DavaNo == TC).Count();
                    int adet = (davaSayi * 5) + 1;
                    string[] bulunan = new string[adet];
                    bulunan[0] = adet.ToString(CultureInfo.CurrentCulture);
                    int sayac = 1;
                    for (int i = 0; i < davaSayi; i++)
                    {
                        bulunan[sayac++] = davalars[i].DavaAdi;
                        bulunan[sayac++] = davalars[i].BaslangicTarihi.ToString(CultureInfo.CurrentCulture);
                        bulunan[sayac++] = davalars[i].davaturu.ToString();
                        bulunan[sayac++] = davalars[i].DavaAciklama;
                        bulunan[sayac++] = davalars[i].DavaUcret.ToString(CultureInfo.CurrentCulture);
                    }
                    if (bulunan != null)
                    {
                        return Json(bulunan, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        public void klasorGuncelle(string tippp, string davaid, string yeniii)
        {
            using (OurDbContext db = new OurDbContext())
            {
                
            }
        }



        public JsonResult bireyselDavaGetir(string ID)
        {
            using(OurDbContext db = new OurDbContext())
            {
                string[] veriler = new string[8];
                Davalar dv = new Davalar();
                dv = db.davalars.Where(x => x.DavaId.ToString() == ID).FirstOrDefault();
                if (dv != null)
                {
                    veriler[0] = "geldi";
                    veriler[1] = dv.DavaAdi;
                    veriler[2] = dv.DavaAciklama;
                    veriler[3] = dv.BaslangicTarihi.ToString(CultureInfo.CurrentCulture);
                    veriler[4] = dv.DavaUcret.ToString(CultureInfo.CurrentCulture);
                    veriler[5] = dv.Avukat.AvukatAd +" "+ dv.Avukat.AvukatSoyad;
                    veriler[6] = dv.davaturu.ToString();//sorunlu, enum display name alabiliyor muyuz?
                    veriler[7] = dv.Musteriler.musteriAd + " " + dv.Musteriler.musteriSoyad;
                    return Json(veriler, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    veriler[0] = "gelmedi";
                    return Json(veriler, JsonRequestBehavior.AllowGet);
                }
            }
        }



        public JsonResult kurumsalDavaGetir(string ID)
        {
            using (OurDbContext db = new OurDbContext())
            {
                string[] veriler = new string[8];
                Davalar dv = new Davalar();
                dv = db.davalars.Where(x => x.DavaId.ToString() == ID).FirstOrDefault();
                if (dv != null)
                {
                    veriler[0] = "geldi";
                    veriler[1] = dv.Musteriler.musteriAd;
                    veriler[2] = dv.DavaAdi;
                    veriler[3] = dv.DavaAciklama;
                    veriler[4] = dv.davaturu.ToString();
                    veriler[5] = dv.BaslangicTarihi.ToString(CultureInfo.CurrentCulture);
                    veriler[6] = dv.Avukat.AvukatAd + " " + dv.Avukat.AvukatSoyad;
                    veriler[7] = dv.DavaUcret.ToString(CultureInfo.CurrentCulture);
                    return Json(veriler, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    veriler[0] = "gelmedi";
                    return Json(veriler, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult dosyalariGetir(string davaid,string tip)
        {
            using(OurDbContext db = new OurDbContext())
            {
                string[] dosyalar;
                if (tip == "aktif")
                {
                    Davalar dv = db.davalars.Where(x => x.DavaId.ToString() == davaid).FirstOrDefault();
                    dosyalar = dv.DosyaAdi.Split('\\');
                    return Json(dosyalar, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    BitenDavalar dv = db.bitendavalars.Where(x => x.DavaId.ToString() == davaid).FirstOrDefault();
                    dosyalar = dv.DosyaAdi.Split('\\');
                    return Json(dosyalar, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public string davaEdit(string davaname, string davaaciklama, string davaucret, string kurumadi, DateTime davabt, string avukat, string davaturu, string davaid,string tippp)
        {
            Davalar davaadkontrol;
            BitenDavalar davaadkontrol2;
            using (OurDbContext db = new OurDbContext())
            {
                try
                {
                    int davaidd = int.Parse(davaid, CultureInfo.CurrentCulture);
                    Davalar editdava = db.davalars.Where(x => x.DavaId == davaidd).FirstOrDefault();
                    davaadkontrol = db.davalars.Where(x => x.DavaId == davaidd).FirstOrDefault().Musteriler.Davalars.Where(y=>y.DavaAdi == davaname).FirstOrDefault();
                    davaadkontrol2 = db.davalars.Where(x => x.DavaId == davaidd).FirstOrDefault().Musteriler.BDavalars.Where(y => y.DavaAdi == davaname).FirstOrDefault();
                    if(davaname == editdava.DavaAdi)
                    {
                        davaadkontrol = null;
                    }
                    if (davaadkontrol == null && davaadkontrol2 == null)
                    {
                        if (tippp != null && davaid != null && davaname != null)
                        {
                            Davalar dv = db.davalars.Where(x => x.DavaId.ToString() == davaid).FirstOrDefault();
                            string old = "/uploads/" + tippp + "/" + dv.Musteriler.musteriTc + "/" + "aktifdavalar" + "/" + dv.DavaAdi;
                            string neww = "/uploads/" + tippp + "/" + dv.Musteriler.musteriTc + "/" + "aktifdavalar" + "/" + davaname;
                            if (old != neww)
                            {
                                Directory.Move(Server.MapPath(old), Server.MapPath(neww));
                            }
                        }

                        Avukat newavukat = new Avukat();
                        string[] avukatdizi = avukat.Split(' ');
                        string avukatad = avukatdizi[0];
                        string avukatsoyad = avukatdizi[1];
                        

                        newavukat = db.avukats.Where(x => x.AvukatAd == avukatad && x.AvukatSoyad == avukatsoyad).FirstOrDefault();
                        editdava.DavaAdi = davaname;
                        editdava.DavaAciklama = davaaciklama;
                        editdava.DavaUcret = int.Parse(davaucret, CultureInfo.CurrentCulture);
                        editdava.BaslangicTarihi = davabt;
                        editdava.Avukat = newavukat;
                        editdava.davaturu = (Davalar.Davaturu)int.Parse(davaturu, CultureInfo.CurrentCulture);
                        db.SaveChanges();
                        return "success";
                    }
                    else
                        return "match";
                    
                }
                catch (Exception)
                {
                    return "error";
                    throw;
                }
                

            }
        }


        public string davaeditislemleri(string did, string tip, string dosyalar, string birkur, string davadosyalar)
        {
            string kullanicitc;
            string davaadi;
            string[] dizi = dosyalar.Split('\\');
            string[] dizi2 = davadosyalar.Split('\\');
            using (OurDbContext db = new OurDbContext())
            {
               
                if (tip == "aktif")
                {
                    Davalar dv = db.davalars.Where(x => x.DavaId.ToString() == did).FirstOrDefault();
                    kullanicitc = dv.Musteriler.musteriTc;
                    davaadi = dv.DavaAdi;
                    string temp = dv.DosyaAdi;
                    for(int i = 0; i < dizi.Length-1; i++)
                    {
                        temp = temp.Replace(dizi[i]+"\\","");
                    }
                    for (int i = 0; i < dizi2.Length - 1; i++)
                    {
                        temp = temp.Replace(dizi2[i] + "\\", "");
                    }
                    temp += davadosyalar;
                    dv.DosyaAdi = temp;
                    db.SaveChanges();
                }
                else
                {
                    BitenDavalar dvb = db.bitendavalars.Where(x => x.DavaId.ToString() == did).FirstOrDefault();
                    davaadi = dvb.DavaAdi;
                    if (dvb.Musteriler != null)
                    {
                        kullanicitc = dvb.Musteriler.musteriTc;
                    }
                    else
                    {
                        kullanicitc = dvb.BMusteriler.musteriTc;
                    }
                    string temp2 = dvb.DosyaAdi;
                    for (int i = 0; i < dizi.Length; i++)
                    {
                        temp2.Replace(dizi[i] + "\\", "");
                    }
                    for (int i = 0; i < dizi2.Length - 1; i++)
                    {
                        temp2 = temp2.Replace(dizi2[i] + "\\", "");
                    }
                    temp2 += davadosyalar;
                    dvb.DosyaAdi = temp2;
                    db.SaveChanges();
                }

            }
            if(tip == "aktif")
            {
                tip = "aktifdavalar";
            }
            else
            {
                tip = "bitendavalar";
            }
            string subyol = "/uploads/" + birkur + "/" + kullanicitc + "/" + tip+"/"+ davaadi+"/";
                
            for(int i = 0; i < dizi.Length-1; i++)
            {
                System.IO.File.Delete(Server.MapPath(subyol + dizi[i]));
            }
            return "";
        }


        public JsonResult CheckDavanameno(string davaadi, string musteritanimi,string musterino,string davano)
        {
            string[] donut = new string[2];
            using (OurDbContext db = new OurDbContext())
            {
                Davalar mst;
                BitenDavalar bmst;
                if (davaadi != null && musteritanimi != null && musterino != null)
                    if (musteritanimi == "k" || musteritanimi == "kk")
                    {
                        mst = db.musterilers.Where(x => x.musteriTc == musterino).FirstOrDefault().Davalars.Where(y => y.DavaAdi == davaadi).FirstOrDefault();
                        bmst = db.musterilers.Where(x => x.musteriTc == musterino).FirstOrDefault().BDavalars.Where(y => y.DavaAdi == davaadi).FirstOrDefault();
                        if (mst == null && bmst == null)
                            donut[0] = "Avaliable";
                        else
                            donut[0] = "Not Avaliable";
                    }
                    else if (musteritanimi == "b" || musteritanimi == "bk")
                    {
                        bmst = db.bitenmusterilers.Where(x => x.musteriTc == musterino).FirstOrDefault().BDavalars.Where(y => y.DavaAdi == davaadi).FirstOrDefault();
                        if (bmst == null)
                            donut[0] = "Avaliable";
                        else
                            donut[0] = "Not Avaliable"; ;
                    }
                    else
                    {
                        donut[0] = "externalerror";
                    }
                else
                    donut[0] = "externalerror";

                Davalar dv = new Davalar();
                BitenDavalar dvb = new BitenDavalar();
                    dv = db.davalars.Where(x => x.DavaNo == davano).FirstOrDefault();
                    dvb = db.bitendavalars.Where(x => x.DavaNo == davano).FirstOrDefault();
                    if (dv == null && dvb == null)
                        donut[1] = "Avaliable";
                    else
                        donut[1] = "Not Avaliable";

            }

            return Json(donut, JsonRequestBehavior.AllowGet);
        }


        public string kayiticindavano(string gelen)
        {
            using (OurDbContext db = new OurDbContext())
            {
                Davalar dv = db.davalars.Where(x => x.DavaNo == gelen).FirstOrDefault();
                BitenDavalar dvb =db.bitendavalars.Where(x => x.DavaNo == gelen).FirstOrDefault();
                if (dv == null && dvb == null)
                {
                    return "Avaliable";
                }
                else
                    return "Not Avaliable";
            }
        }


























        public class RegexUtilities
        {
            public static bool IsValidEmail(string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                try
                {
                    // Normalize the domain
                    email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));

                    // Examines the domain part of the email and normalizes it.
                    string DomainMapper(Match match)
                    {
                        // Use IdnMapping class to convert Unicode domain names.
                        var idn = new IdnMapping();

                        // Pull out and process domain name (throws ArgumentException on invalid)
                        var domainName = idn.GetAscii(match.Groups[2].Value);

                        return match.Groups[1].Value + domainName;
                    }
                }
                catch (RegexMatchTimeoutException e)
                {
                    return false;
                }
                catch (ArgumentException e)
                {
                    return false;
                }

                try
                {
                    return Regex.IsMatch(email,
                        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                        RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }
        }


        //Aşağısı Rastgele Şifre İşlemi

        public int aktivasyonkoduolusturucu()
        {
            Random rand = new Random();
            string akt = "";
            for (int i = 0; i < 6; i++) akt += rand.Next(0, 9).ToString(CultureInfo.CurrentCulture);
            return int.Parse(akt, CultureInfo.CurrentCulture);
        }

        public string randomcharacter2(int x)
        {
            int harfsayac = 0;
            int rakamsayac = 0;
            int firstelement = 0;
            bool flag1 = false;
            int test;
            int[] results1 = new int[x];
            firstelement = (int)(System.DateTime.Now.Second % 29);
            results1[0] = firstelement;
            if (firstelement <= 9)
                rakamsayac++;
            else
                harfsayac++;

            for (int i = 1; i < x; i++)
            {
                test = (int)((System.DateTime.Now.Millisecond * i * i * i * (i + 3)) % 29);
                for (int y = 0; y < x; y++)
                {
                    if (test == results1[y])
                        flag1 = true;
                }
                if (flag1 == false)
                {
                    if (rakamsayac < 3 && test <= 9)
                    {
                        results1[i] = test;
                        rakamsayac++;
                    }

                    else if (harfsayac < 4 && test > 9)
                    {
                        results1[i] = test;
                        harfsayac++;
                    }
                    else
                    {
                        i--;
                        flag1 = false;
                    }
                }
                else
                {
                    i--;
                    flag1 = false;
                }

            }
            result = collector(results1, x);
            return result;
        }
        public string collector(int[] exarray, int max)
        {
            result = "";
            for (int i = 0; i < max; i++)
                for (int y = 0; y <= 28; y++)
                {
                    if (exarray[i] == y)
                    {
                        if (y > 9)
                        {
                            result += (letters[y]);
                            break;
                        }

                        else
                        {
                            result += y.ToString(CultureInfo.CurrentCulture);
                            break;
                        }


                        //result += exarray[i];
                    }

                }
            return result;
        }
        //Yukarısı Rastgele Şifre İşlemi
    }
}