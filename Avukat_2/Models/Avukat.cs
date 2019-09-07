using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avukat_2.Models
{
    public class Avukat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AvukatID { get; set; }
        public string AvukatAd { get; set; }
        public string AvukatSoyad { get; set; }
        public string telno { get; set; }
        public string mail { get; set; }




        public virtual List<Davalar> Davalars { get; set; }
        public virtual List<BitenDavalar> bDavalars { get; set; }
    }
}