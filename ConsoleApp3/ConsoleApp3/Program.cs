using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            

            
            

            var librarian = Librarian.Instance("Librarian1", "Admin", "pass");

            var library = Library.Instance("Library", librarian);


            


            library.AddBook(new Book("Book1", "Author1", "Type1",1));
            library.AddBook(new Book("Book2", "Author2", "Type1",2));
            library.AddBook(new Book("Book3", "Author1", "Type3",3));
            library.AddBook(new Book("Book4", "Author2",  "Type4",4));

            

            Login login = new Login(librarian, library);

            login.Start();
            

    
            Console.ReadKey();
            
        }
    }
}
