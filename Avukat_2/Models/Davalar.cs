using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Avukat_2.Models
{
    [Table("Davalar")]
    public class Davalar
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DavaId { get; set; }



        //-------------------------------------------------------
        [Required(ErrorMessage = "Bu hane zorunludur.")]
        public string DavaAdi { get; set; }

        public string DavaNo { get; set; }

        //-------------------------------------------------------
        public string DosyaAdi { get; set; }



        //-------------------------------------------------------
        public string DavaAciklama { get; set; }


        //-------------------------------------------------------
        public int DavaUcret { get; set; }




        //-------------------------------------------------------
        [Column(TypeName = "Date")]
        public DateTime BaslangicTarihi { get; set; }


        public enum Davaturu
        {
            [Display(Name = "Edim Davası")]
            Edim_Davası,
            [Display(Name = "Tespit Davası")]
            Tespit_Davası,

            [Display(Name = "Yenilik Doğuran Davalar")]
            Yenilik_Doğuran_Davalar,

            [Display(Name = "Belirsiz Alacak ve Tespit Davaları")]
            Belirsiz_Alacak_ve_Tespit_Davaları,

            [Display(Name = "Objektif Dava Birleşmesi")]
            Objektif_Dava_Birleşmesi,

            [Display(Name = "Terditli Davalar")]
            Terditli_Davalar,

            [Display(Name = "Seçimlik Davalar")]
            Seçimlik_Davalar,

            [Display(Name = "Kısmi Dava")]
            Kısmi_Dava,

            [Display(Name = "Mütelahik Davalar")]
            Mütelahik_Davalar,

            [Display(Name = "Topluluk Davaları")]
            Topluluk_Davaları
        }
        public Davaturu davaturu { get; set; }

        public virtual Musteriler Musteriler { get; set; }
        public virtual Avukat Avukat { get; set; }
    }
}