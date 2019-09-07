using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avukat_2.Models
{
    [Table("Admin")]
    public class Admin
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminID { get; set; }


        [Required(ErrorMessage ="Bu alan bos gecilemez")]
        [Display(Name ="Username")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Bu alan bos gecilemez")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Roletr { get; set; }

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string email { get; set; }


    }
}