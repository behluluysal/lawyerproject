using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Avukat_2.Models
{
    public class Anasayfa
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int anasayfaid { get; set; }
        public string bir { get; set; }
        public string iki { get; set; }
        public string uc { get; set; }
        public string dort { get; set; }
        public string bes { get; set; }
        public string alti { get; set; }
        public string yedi { get; set; }
        public string sekiz { get; set; }
        public string dokuz { get; set; }
        public string on { get; set; }
        public string onbir { get; set; }
        public string oniki { get; set; }
        public string onuc { get; set; }
        public string ondort { get; set; }
        public string onbes { get; set; }
        public string onalti { get; set; }
        public string onyedi { get; set; }
        public string onsekiz { get; set; }
        public string ondokuz { get; set; }
        public string yirmi { get; set; }
        public string yirmibir { get; set; }
        public string yirmiiki { get; set; }
        public string yirmiuc { get; set; }
        public string yirmidort { get; set; }
        public string yirmibes { get; set; }
        public string yirmialti { get; set; }
        public string yirmiyedi { get; set; }
        public string yirmisekiz { get; set; }
        public string yirmidokuz { get; set; }
        public string otuz { get; set; }
        public string otuzbir { get; set; }
        public string otuziki { get; set; }
        public string otuzuc { get; set; }
        public string otuzdort { get; set; }
        public string otuzbes { get; set; }
        public string otuzalti { get; set; }
        public string otuzyedi { get; set; }
        public string otuzsekiz { get; set; }
        public string otuzdokuz { get; set; }
        public string kirk { get; set; }
        public string kirkbir { get; set; }
        public string kirkiki { get; set; }
        public string kirkuc { get; set; }
        public string kirkdort { get; set; }
        public string kirkbes { get; set; }
        public string kirkalti { get; set; }
        public string kirkyedi { get; set; }
        public string kirksekiz { get; set; }
        public string kirkdokuz { get; set; }
        public string elli { get; set; }
        public string ellibir { get; set; }
        public string elliiki { get; set; }
        public string elliuc { get; set; }
        public string ellidort { get; set; }
        public string ellibes { get; set; }
        public string ellialti { get; set; }
        public string elliyedi { get; set; }
        public string ellisekiz { get; set; }
        public string ellidokuz { get; set; }
        public string altmis { get; set; }
        public string altmisbir { get; set; }
        public string altmisiki { get; set; }
        public string altmisuc { get; set; }
        public string altmisdort { get; set; }
    }
}