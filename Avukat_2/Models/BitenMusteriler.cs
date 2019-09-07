using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Avukat_2.Models
{
    [Table("BitenMusteriler")]
    public class BitenMusteriler
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        public string MusteriSifre { get; set; }





        //-------------------------------------------------------------------
        [Required(ErrorMessage = "Bu hane zorunludur.")]
        public string musteriAd { get; set; }



        //-------------------------------------------------------------------

        public string musteriSoyad { get; set; }




        //-------------------------------------------------------------------
        [StringLength(11, ErrorMessage = "TC 11 Haneli olmalıdır.")]
        [Required(ErrorMessage ="Bu hane zorunludur.")]
        public string musteriTc { get; set; }


        //-------------------------------------------------------

        public string musteriCinsiyet { get; set; }






        //-------------------------------------------------------
        [Column(TypeName = "Date")]
        public DateTime musteriDT { get; set; }




        //-------------------------------------------------------
        public string musteriAdres { get; set; }







        //-------------------------------------------------------
        public int ucret { get; set; }


        //-------------------------------------------------------
        public string PhoneNumber { get; set; }

        public string EvTelNo { get; set; }

        public string FaxNo { get; set; }



        //-------------------------------------------------------
        public string email { get; set; }

        public DateTime kayittarihi { get; set; }
        public DateTime bitistarihi { get; set; }

        public string dosyaYolu { get; set; }

        public virtual List<BitenDavalar> BDavalars { get; set; }


    }
}