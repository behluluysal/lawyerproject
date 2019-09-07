using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Avukat_2.Models;
using System.Globalization;

namespace Avukat_2.Models
{
    public class OurDbContext : DbContext
    {
        public DbSet<Admin> userAccounts { get; set; }
        public DbSet<Musteriler> musterilers { get; set; }
        public DbSet<BitenMusteriler> bitenmusterilers { get; set; }
        public DbSet<Davalar> davalars { get; set; }
        public DbSet<BitenDavalar> bitendavalars { get; set; }
        public DbSet<Avukat> avukats { get; set; }
        public DbSet<Anasayfa> anasayfa { get; set; }

        public OurDbContext()
        {
            Database.SetInitializer(new VeriTabaniOlusturucu());
        }

        
    }

    public class VeriTabaniOlusturucu : CreateDatabaseIfNotExists<OurDbContext>
    {
        protected override void Seed(OurDbContext context)
        {
            Admin usr = new Admin();
            usr.Username = "su";
            usr.Password = "123";
            usr.Roletr = "superadmin";
            context.userAccounts.Add(usr);
            context.SaveChanges();

            Avukat avkat = new Avukat();
            avkat.AvukatAd = "Yasin";
            avkat.AvukatSoyad = "Dere";
            avkat.telno = "caldirbeni";
            avkat.mail = "maillebeni";
            context.avukats.Add(avkat);
            context.SaveChanges();
            //--------------------------------
            Avukat avkat2 = new Avukat();
            avkat2.AvukatAd = "Behlül";
            avkat2.AvukatSoyad = "Uysal";
            avkat2.telno = "caldirbeni";
            avkat2.mail = "maillebeni";
            context.avukats.Add(avkat2);
            context.SaveChanges();

            DateTime now;
            Musteriler usr2 = new Musteriler();
            usr2.musteriAd = "test";
            usr2.MusteriSifre = "111";
            usr2.musteriSoyad = "soytest";
            usr2.musteriTc = "1001";
            usr2.musteriDT=DateTime.Parse("01/08/1998", CultureInfo.CurrentCulture);
            now = DateTime.Now;
            DateTime dt = new DateTime(1998, 01, 08);
            int age = now.Year - dt.Year;
            if (now < dt.AddYears(age))
                age--;
            usr2.musteriYas = age;
            usr2.kayittarihi= DateTime.Parse("01/08/2017", CultureInfo.CurrentCulture);
            usr2.ucret = 1000;
            usr2.email = "test@gmail.com";
            usr2.musteriCinsiyet = "erkek";
            usr2.musteriAdres = "sakarya";
            usr2.PhoneNumber = "+(90) 999 999 9999";
            usr2.EvTelNo = "+(264) 655 2222";
            context.musterilers.Add(usr2);
            context.SaveChanges();

            //----------------------------------------------------------

            BitenMusteriler usr3 = new BitenMusteriler();
            usr3.musteriAd = "test1";
            usr3.MusteriSifre = "222";
            usr3.musteriSoyad = "soytest1";
            usr3.musteriTc = "1002";
            usr3.musteriDT = DateTime.Parse("01/08/1990", CultureInfo.CurrentCulture);
            now = DateTime.Now;

            usr3.kayittarihi = DateTime.Parse("01/08/2017", CultureInfo.CurrentCulture);
            usr3.bitistarihi = DateTime.Parse("01/08/2019", CultureInfo.CurrentCulture);
            usr3.ucret = 1000;
            usr3.email = "test@gmail.com";
            usr3.musteriCinsiyet = "erkek";
            usr3.musteriAdres = "sakarya";
            usr3.PhoneNumber = "+(90) 999 999 9999";
            usr3.EvTelNo = "+(666) 222 1111";
            context.bitenmusterilers.Add(usr3);
            context.SaveChanges();
            //-------------------------------------------
            usr2.musteriAd = "Kurumsal1";
            usr2.musteriSoyad = null;
            usr2.musteriCinsiyet = null;
            usr2.EvTelNo = null;
            usr2.FaxNo = "+(999) 999 9999";
            usr2.MusteriSifre = "333";
            usr2.musteriTc = "1004";
            usr2.ucret = 10000;
            context.musterilers.Add(usr2);
            context.SaveChanges();
            //------------------------------------------
            usr3.UserID = 1;
            usr3.musteriAd = "Kurumsal2";
            usr3.musteriSoyad = null;
            usr3.musteriCinsiyet = null;
            usr3.EvTelNo = null;
            usr3.FaxNo = "+(999) 999 8888";
            usr3.MusteriSifre = "333";
            usr3.musteriTc = "1005";
            usr3.ucret = 10000;
            context.bitenmusterilers.Add(usr3);
            context.SaveChanges();

            Davalar dava1 = new Davalar
            {
                DavaNo = "70",
                Musteriler = usr2,
                BaslangicTarihi = DateTime.Now,
                DavaAdi = "testdavasi",
                DavaAciklama = "test için eklenmiş dava",
                DosyaAdi = "aa.txt",
                davaturu = Davalar.Davaturu.Belirsiz_Alacak_ve_Tespit_Davaları,
                DavaUcret = 299,
                Avukat = avkat2
            };
            
            context.davalars.Add(dava1);
            context.SaveChanges();

            BitenDavalar dava2 = new BitenDavalar
            {
                DavaNo="80",
                Musteriler = usr2,
                BaslangicTarihi = DateTime.Now,
                DavaAdi = "bitentestdavasi",
                DavaAciklama = "bitentest için eklenmiş dava",
                DosyaAdi = "aaaa.txt",
                DavaUcret = 500,
                davaturu = BitenDavalar.Davaturub.Edim_Davası,
                BitisTarihi = DateTime.Now,
                Avukat = avkat
            };
            context.bitendavalars.Add(dava2);
            context.SaveChanges();

            BitenDavalar dava3 = new BitenDavalar
            {
                DavaNo = "90",
                BMusteriler = usr3,
                BaslangicTarihi = DateTime.Now,
                DavaAdi = "bitentestdavasi",
                DavaAciklama = "bitentest için eklenmiş dava",
                DosyaAdi = "aaaa.txt",
                DavaUcret = 500,
                davaturu = BitenDavalar.Davaturub.Terditli_Davalar,
                BitisTarihi = DateTime.Now,
                Avukat = avkat
            };
            context.bitendavalars.Add(dava3);
            context.SaveChanges();

            Anasayfa ans = new Anasayfa();
            ans.bir = "bir";
            context.anasayfa.Add(ans);
            context.SaveChanges();



            List<Admin> userAccounts = context.userAccounts.ToList();
            List<Musteriler> musterilers = context.musterilers.ToList();
            List<Davalar> davalars = context.davalars.ToList();
            List<BitenMusteriler> bitenmusterilers = context.bitenmusterilers.ToList();
            List<BitenDavalar> bitendavalars = context.bitendavalars.ToList();
        }
    }
}
