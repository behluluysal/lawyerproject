using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Avukat_2.Models;

namespace Avukat_2.Security
{
    public class CustomPrincipal : IPrincipal
    {

        private Admin Account;

        public CustomPrincipal(Admin account)
        {
            this.Account = account;
            this.Identity = new GenericIdentity(account.Username);
        }
        public IIdentity Identity { get;set;}

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            return roles.Any(r => this.Account.Roletr.Contains(r));
        }
    }
}