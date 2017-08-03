using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Reader
    {
        #region Proprietes
        public string Address
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public long PhoneNumber
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        private List<Book> _books = new List<Book>();
        #endregion
        public Reader()
        {

        }
        public Reader(string name,string email,string address,long phone)
        {

            Name = name;
            Email = email;
            Address = address;
            PhoneNumber = phone;
            
        }
        public void Update(Book a)
        {
            Console.WriteLine($"The reader {Name} was notified about the removal of the book {a.ToString()}");
        }
        public void AddBook(Book a)
        {
            _books.Add(a);
            Console.WriteLine($"{this.Name} borrowed the book {a.ToString()}");
        }
        public void ReturnBook(int code,ref Book a)
        {if (_books.Count > 0)
            {
                bool g = false;
                foreach (var book in _books.ToList())
                    if (book.BookCode == code)
                    {
                        Console.WriteLine($"The reader {Name} returned the book {book.ToString()}");
                        a = book;
                        g = true;
                        _books.Remove(book);
                    }
                if (g == false)
                    Console.WriteLine("There is no book with the given book");
            }
            else
                Console.WriteLine("There are no books to return ");

            
        }
        public override string ToString()
        {
            return $"{Name} {PhoneNumber} {Email} {Address}";
        }

    }
}
