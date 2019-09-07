using Avukat_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avukat_2.ViewModels
{
    public class MusteriListeleViewModel
    {
        public List<Musteriler> musteriler { get; set; }
        public List<BitenMusteriler> bitenmusteriler { get; set; }
    }
}