using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Library
    {
        private static Library _instance;
        private static object _padlock = new object();
        #region Proprietes
        public string Name
        {
            get;
            private set;
        }
        public Librarian Admin
        {
            get;
            private set;
        }
        private List<Reader> _readers = new List<Reader>();
        private List<Book> _books = new List<Book>();
        #endregion
        private Library(string name,Librarian admin)
        {
            Name = name;
            Admin = admin;
        }
        public static Library Instance(string name, Librarian admin)
        {
            if (_instance == null)
                lock (_padlock)
                    if (_instance == null)
                        _instance = new Library(name, admin);
            return _instance;
        }
        public void AddReader(Reader reader)
        {
            _readers.Add(reader);
        }
        public void AddBook(Book book)
        {
            _books.Add(book);
        }
        public void RemoveReader(string name)
        {
            if (_readers.Count > 0)
            {
                bool g = false;
                foreach (var i in _readers.ToList())
                    if (i.Name == name)
                    {
                        _readers.Remove(i);
                        g = true;
                    }
                if (g == false)
                    Console.WriteLine("There is no reader with the given name");

            }
            else
                Console.WriteLine("There is no readers to remove");
        }
        public void RemoveBook(int code)
        {
            if (_books.Count > 0)
            {
                bool g = false;
                foreach (var book in _books.ToList())
                    if (book.BookCode == code)
                    {
                        Notify(book);
                        _books.Remove(book);
                        g = true;
                        
                    }
                if (g == false)
                    Console.WriteLine("There is no book with the give code");
            }
            else
                Console.WriteLine("There is no books to remove");

        }
        public void Notify(Book a)
        {
            foreach (var reader in _readers)
                reader.Update(a);
        }
        public void DisplayBooks()
        {
            foreach (var book in _books)
                Console.WriteLine(book.ToString());
        }
        public void DisplayReaders()
        {
            foreach (var reader in _readers)
                Console.WriteLine(reader.ToString());

        }
        public void Search(string name,string author,string type)
        {
            bool g = false;
            foreach (var book in _books)
                if (((book.Name == name) || (name == "")) && ((book.Author == author) || (author == "")) && ((book.BookType == type) || (type == "")))
                {
                    Console.WriteLine(book.ToString());
                    g = true;
                }
            if (g == false)
                Console.WriteLine("There is no book");
        }
        public void BorrowBook(string user,int code)
        {
            Book book=new Book();
            Reader reader = new Reader(); 
            foreach (var i in _readers)
                if (i.Name == user)
                    reader = i;
            foreach (var i in _books)
                if (i.BookCode == code)
                    book = i;
            reader.AddBook(book);
            _books.Remove(book);
        }
        public void ReturnBook(string user,int code)
        {
            Book a = new Book();
            foreach (var reader in _readers.ToList())
                if (reader.Name == user)
                    reader.ReturnBook(code,ref a);
            _books.Add(a);
        }
        
                    
    }
}
