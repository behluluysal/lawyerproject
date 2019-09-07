using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Avukat_2.Models;

namespace Avukat_2.ViewModels
{
    public class panelViewModel
    {
        public List<Musteriler> musterilers { get; set; }
        public List<Davalar> davalars { get; set; }
        public List<BitenMusteriler> bmusteriler { get; set; }
        public List<BitenDavalar> bdavalars { get; set; }
        public List<Avukat> avukats { get; set; }
    }
}