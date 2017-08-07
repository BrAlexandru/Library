using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Reader : IReader
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
        private List<IBook> _books = new List<IBook>();
        #endregion
        public Reader()
        {

        }
        public Reader(string name, string email, string address, long phone)
        {

            Name = name;
            Email = email;
            Address = address;
            PhoneNumber = phone;

        }
        public void Update(IBook a)
        {
            Console.WriteLine($"The reader {Name} was notified about the removal of the book {a.Name}");
        }
        public void AddBook(IBook a)
        {
            _books.Add(a);
            Console.WriteLine($"{this.Name} borrowed the book {a.Name}");

        }
        public void ReturnBook(int code, ref IBook a)
        {
            if (_books.Count > 0)
            {
                bool g = false;
                foreach (var book in _books.ToList())
                    if (book.BookCode == code)
                    {
                        Console.WriteLine($"The reader {Name} returned the book {book.Name}");
                        a = book;
                        g = true;
                        _books.Remove(book);
                    }
                if (g == false)
                    Console.WriteLine($"The reader {Name}  does not have the book with the given code");
            }
            else
                Console.WriteLine("There are no books to return ");


        }
        public void DisplayBooks()
        {
            if (_books.Count > 0)
            {
                _books = _books.OrderBy(a => a.Name).ToList();
                foreach (var book in _books)
                    Console.WriteLine(book.ToString());

            }
            else
                Console.WriteLine("No books to display");
        }
        public int NrBooks()
        {
            return _books.Count;
        }
        public override string ToString()
        {
            return $"{Name,-10} {PhoneNumber,-10} {Email,-10} {Address,-10}";
        }

    }
}
