using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp15
{
    public sealed class Librarian
    {

        private static Librarian _instance;
        private static object _padlock = new object();

        #region Proprietes

        public string AdminID
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

        private Librarian(string id,string pass)
        {
            AdminID = id;
            Password = pass;
        }
        public static Librarian Instance(string id,string pass)
        {
            if(_instance==null)
                lock(_padlock)
                {
                    if (_instance == null)
                        _instance = new Librarian(id, pass);
                }
            return _instance;
        }


    }
}
