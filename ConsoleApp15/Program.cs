﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp15
{
    class Program
    {
        static void Main(string[] args)
        {
            

            var librarian = Librarian.Instance("Admin", "pass");

            var login = new Login(librarian);

            login.Menu();
            


            

        }
    }
}
