using Avukat_2.Models;
using Avukat_2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Avukat_2.Security;

namespace Avukat_2.Controllers
{
    public class DeleteController : Controller
    {
        OurDbContext db = new OurDbContext();

        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Davasil()
        {
            OurDbContext db = new OurDbContext();
            DavakayitViewModel dkb = new DavakayitViewModel();
            dkb.bitenDavalars = db.bitendavalars.ToList();
            dkb.davalars = db.davalars.ToList();
            dkb.avukats = db.avukats.ToList();

            return View(dkb);
        }

        [CustomAuthorize(Roles = "superadmin")]
        public ActionResult Musterisil()
        {
            OurDbContext db = new OurDbContext();
            MusteriListeleViewModel dkb = new MusteriListeleViewModel();
            dkb.bitenmusteriler = db.bitenmusterilers.ToList();
            dkb.musteriler = db.musterilers.ToList();
            return View(dkb);
        }

        public string davvasil(string ID)
        {
            string test = "/uploads/";
            try
            {
                int silinecekid = int.Parse(ID.Substring(2), CultureInfo.CurrentCulture);
                if (ID[0] == 'a')
                {
                    Davalar dv = db.davalars.Where(x => x.DavaId == silinecekid).FirstOrDefault();
                    if(dv.Musteriler.musteriSoyad != null)
                    {
                        test += "bireysel/";
                    }
                    else
                    {
                        test += "kurumsal/";
                    }
                    test += dv.Musteriler.musteriTc;
                    test += "/aktifdavalar/";
                    test += dv.DavaAdi;
                    System.IO.Directory.Delete(Server.MapPath(test), true);

                    db.davalars.Remove(dv);
                    db.SaveChanges();
                    return "success";
                }
                else if (ID[0] == 'b')
                {
                    BitenDavalar dvb = db.bitendavalars.Where(x => x.DavaId == silinecekid).FirstOrDefault();
                    if (dvb.Musteriler.musteriSoyad != null)
                    {
                        test += "bireysel/";
                    }
                    else
                    {
                        test += "kurumsal/";
                    }
                    if (dvb.Musteriler != null)
                    {
                        test += dvb.Musteriler.musteriTc;
                    }
                    else { test += dvb.BMusteriler.musteriTc; }
                    test += "/bitendavalar/";
                    test += dvb.DavaAdi;

                    Directory.Delete(Server.MapPath(test), true);
                    db.bitendavalars.Remove(dvb);
                    db.SaveChanges();
                    return "success";
                }
                else
                {
                    return "externalerror";
                }
            }
            catch (Exception)
            {
                return "error";
                throw;
            }
            
        }

        public string musteriyiSil(string ID)
        {
            string test = "/uploads/";
            try
            {
                int silinecekid = int.Parse(ID.Substring(2), CultureInfo.CurrentCulture);
                if (ID[0] == 'a')
                {
                    Musteriler mst = db.musterilers.Where(x => x.UserID == silinecekid).FirstOrDefault();
                   
                    if(mst.musteriSoyad != null)
                    {
                        test += "bireysel/";
                    }
                    else { test += "kurumsal/"; }
                    test += mst.musteriTc+"/";
                    Directory.Delete(Server.MapPath(test), true);

                    db.davalars.RemoveRange(mst.Davalars);
                    db.SaveChanges();

                    db.bitendavalars.RemoveRange(mst.BDavalars);
                    db.SaveChanges();

                    db.musterilers.Remove(mst);
                    db.SaveChanges();

                    return "success";
                }
                else if (ID[0] == 'b')
                {
                    BitenMusteriler bmst = db.bitenmusterilers.Where(x => x.UserID == silinecekid).FirstOrDefault();

                    if (bmst.musteriSoyad != null)
                    {
                        test += "bireysel/";
                    }
                    else { test += "kurumsal/"; }
                    test += bmst.musteriTc;
                    Directory.Delete(Server.MapPath(test), true);

                    db.bitendavalars.RemoveRange(bmst.BDavalars);
                    db.SaveChanges();

                    db.bitenmusterilers.Remove(bmst);
                    db.SaveChanges() ;
                    return "success";
                }
                else
                {
                    return "externalerror";
                }
            }
            catch (Exception e)
            {
                return "error";
                throw;
            }

        }

    }
}