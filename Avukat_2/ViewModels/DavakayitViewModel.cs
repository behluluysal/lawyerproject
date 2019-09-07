using Avukat_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avukat_2.ViewModels
{
    public class DavakayitViewModel
    {
        public List<Avukat> avukats { get; set; }
        public Davalar dava { get; set; }
        public List<Davalar> davalars { get; set; }
        public List<BitenDavalar> bitenDavalars { get; set; }

    }

}