using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Account : IAccount
    {

        #region Proprietes

        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }


        #endregion

        public Account(string user, string pass)
        {
            Username = user;
            Password = pass;
        }

    }
}
