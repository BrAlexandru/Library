﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel;
namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();

            container.Register(Component.For<ILogin>().ImplementedBy<Login>());
            container.Register(Component.For<IBook>().ImplementedBy<Book>().LifestyleTransient());
            

            var librarian = Librarian.Instance("Librarian", "Admin", "pass");

            var library = Library.Instance("Library", librarian);

            library.AddBook(container.Resolve<IBook>(new { name = "Book2", author = "Author2", type = "Type2", code = 2, genre = "Genre2" }));
            library.AddBook(container.Resolve<IBook>(new { name = "Book3", author = "Author3", type = "Type3", code = 3, genre = "Genre3" }));
            library.AddBook(container.Resolve<IBook>(new { name = "Book1", author = "Author1", type = "Type1", code = 1, genre = "Genre1" }));




            var login = container.Resolve<ILogin>(new { librarian = librarian, library = library });

            

            

            login.Start();


            Console.ReadKey();

        }
    }
}
