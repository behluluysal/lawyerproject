using Avukat_2.Models;
using Avukat_2.Security;
using Avukat_2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System.IO;

namespace Avukat_2.Controllers
{
    
    public class HomeController : Controller
    {
        string result = "";
        char[] letters = {'a','b','c','ç','d','e','f','g','ğ','h','ı','i','j','k','l','m','n','o','ö'
                         ,'p','r','s','ş','t','u','ü','v','y','z'}; //29 karakter var
        string[] letterss = {"a","b","c","ç","d","e","f","g","ğ","h","ı","i","j","k","l","m","n","o"
                          ,"ö","p","r","s","ş","t","u","ü","v","y","z"};

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
        //yukarısı rastgele url için

        static string musteritc;
        static string global = "";







        


        public ActionResult Index()
        {
            OurDbContext db = new OurDbContext();
            Anasayfa ans = db.anasayfa.Where(x => x.anasayfaid == 1).FirstOrDefault();
            if(global == "")
            {
                ViewBag.mesaj = "";
            }
            else
            {
                ViewBag.mesaj = "Girdiğiniz Bilgilere Ait Kullanıcı Bulunamadı!";
            }
            return View(ans);
        }

        [HttpPost]
        public ActionResult Login(string tc, string tcsifre)
        {
            using (OurDbContext db = new OurDbContext())
            {
                global = "";
               
                var usr = db.musterilers.Where(x => x.musteriTc == tc && x.MusteriSifre == tcsifre).FirstOrDefault();
                var biten = db.bitenmusterilers.Where(x => x.musteriTc == tc && x.MusteriSifre == tcsifre).FirstOrDefault();
                if (usr != null)
                {
                    Session["user"] = "aktif";
                    musteritc = usr.musteriTc;
                    return RedirectToAction("musteriPanel");
                }
                else if(biten != null)
                {
                    Session["user"] = "aktif";
                    musteritc = biten.musteriTc;
                    return RedirectToAction("musteriPanel");
                }
                else
                {
                    Session["user"] = "deaktif";
                    global = "error";
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Logout()
        {
            musteritc = "";
            global = "";
            Session["user"] = "deaktif";
            return RedirectToAction("Index");
        }

        public ActionResult musteriPanel()
        {
            if(Session["user"]!= null)
            {
                if (Session["user"].ToString() == "aktif")
                {
                    OurDbContext db = new OurDbContext();
                    musteriPanelViewModel mst = new musteriPanelViewModel();
                    var usr = db.musterilers.Where(x => x.musteriTc == musteritc).FirstOrDefault();
                    var biten = db.bitenmusterilers.Where(x => x.musteriTc == musteritc).FirstOrDefault();
                    if (usr != null)
                    {
                        mst.musteriler = usr;
                        return View(mst);
                    }
                    else
                    {
                        mst.bitenMusteriler = biten;
                        return View(mst);
                    }
                }
            }
            
            return RedirectToAction("Index");
        }
        
        public ActionResult Profilee()
        {
            return View();
        }




        //Tempsilme
        public void tempSil( string path)
        {
            path = path.Substring(0, 13);
            Thread.Sleep(120000);
            System.IO.Directory.Delete(Server.MapPath(path), true);
        }



        public FileContentResult Download(string gonderici, string gondericiiki)
        {
            string[] veri = gonderici.Split('+');
            string akbit = veri[0];
            string birkur = veri[1];
            string musteritc = veri[2];
            string aktbitdava = veri[3];
            string davaad = veri[4];
            string davaid = veri[5];

            if (birkur == "k") { birkur = "bireysel"; } else { birkur = "kurumsal"; }
            if (aktbitdava == "s") { aktbitdava = "aktifdavalar"; } else { aktbitdava = "bitendavalar"; }


            //yol oluşturma işlemi
            string sourceyol = "/uploads/" + birkur + "/" + musteritc + "/" + aktbitdava + "/" + davaad + "/" + gondericiiki;
            string random = randomcharacter2(7);
            string targetyol = "/temp/" + random;
            bool exists = System.IO.Directory.Exists(Server.MapPath(targetyol));
            if (!exists)
                System.IO.Directory.CreateDirectory(Server.MapPath(targetyol));
            targetyol += "/" + gondericiiki;
            System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(targetyol));

            exists = System.IO.File.Exists(Server.MapPath(targetyol));
            if (!exists)
                System.IO.File.Copy(Server.MapPath(sourceyol), Server.MapPath(targetyol));
            
            var bytes = System.IO.File.ReadAllBytes(Server.MapPath(targetyol));
            System.IO.Directory.Delete(Server.MapPath(targetyol.Substring(0,13)), true);

            return File(bytes, file.GetType().ToString(), gondericiiki);
        }

        public ActionResult Chat()
        {
            return View();
        }


    }
}