using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public sealed class Librarian : ILibrarian
    {
        private static Librarian _instance;
        private static object _padlock = new object();
        #region Proprietes
        public string AdminID
        {
            get;
            set;
        }
        public string Name
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
        private Librarian(string name, string id, string pass)
        {
            Name = name;
            AdminID = id;
            Password = pass;

        }
        public static Librarian Instance(string name, string id, string pass)
        {
            if (_instance == null)
                lock (_padlock)
                {
                    if (_instance == null)
                        _instance = new Librarian(name, id, pass);
                }
            return _instance;
        }


    }
}
