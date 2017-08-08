using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public sealed class Library : ILibrary
    {
        private static Library _instance;
        private static object _padlock = new object();
        #region Proprietes
        public string Name
        {
            get;
            set;
        }
        public ILibrarian Admin
        {
            get;
            set;
        }
        private List<IReader> _readers = new List<IReader>();
        private List<IBook> _books = new List<IBook>();
        #endregion
        private Library(string name, ILibrarian admin)
        {
            Name = name;
            Admin = admin;
        }

        public static Library Instance(string name, ILibrarian admin)
        {
            if (_instance == null)
                lock (_padlock)
                    if (_instance == null)
                        _instance = new Library(name, admin);
            return _instance;
        }

        public void AddReader(IReader reader)
        {
            _readers.Add(reader);

        }

        public void AddBook(IBook book)
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

        public void Notify(IBook a)
        {
            foreach (var reader in _readers)
                reader.Update(a);
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

        public void DisplayReaders()
        {
            if (_readers.Count > 0)
            {
                _readers = _readers.OrderBy(a => a.Name).ToList();
                foreach (var reader in _readers)
                    Console.WriteLine(reader.ToString());
            }
            else
                Console.WriteLine("No readers to display");
        }

        public void MaxBooks()
        {
            if (_readers.Count > 0)
            {
                int max = _readers.Max(x => x.NrBooks());
                if (max > 0)
                {
                    var readers = _readers.Where(x => x.NrBooks() == max);
                    readers = readers.OrderBy(x => x.Name);
                    foreach (var reader in readers)
                        Console.WriteLine($"The reader {reader.Name} has the most borrowed books.({reader.NrBooks()})");
                }
                else
                    Console.WriteLine("Nobody borrowed any book");

                var debug = _readers.OrderBy(_ => _.NrBooks());

                _readers.Where(_ => _.NrBooks() == _readers.Max(r => r.NrBooks()));
            }
            else
                Console.WriteLine("No readers");
            
        }

        public void Search(string name, string author, string type,string genre)
        {
            var _searchBooks = new List<IBook>();
            foreach (var book in _books)
                if (((book.Name.ToUpper().Contains(name.ToUpper())) || (name == ""))&&((book.Author.ToUpper().Contains(author.ToUpper())) || (author == ""))&&((book.BookType.ToUpper().Contains(type.ToUpper())) || (type == "")) && ((book.Genre.ToUpper().Contains(genre.ToUpper())) || (genre == "")))
                 _searchBooks.Add(book);


            if (_searchBooks.Count > 0)
            {
                _searchBooks = _searchBooks.OrderBy(x => x.Name).ToList();
                foreach (var book in _searchBooks)
                    Console.WriteLine(book.ToString());
            }
            else
                Console.WriteLine("No books that match with the given arguments");
                
        }

        public void BorrowBook(string user, int code)
        {
            if (_books.Count > 0)
            {
                bool g = false;
                IBook book = new Book();
                IReader reader = new Reader();
                foreach (var i in _readers)
                    if (i.Name == user)
                        reader = i;
                foreach (var i in _books)
                    if (i.BookCode == code)
                        if (i.GetType().ToString() == "ConsoleApp3.BorrowedBook")
                        {
                            Console.WriteLine("This book is already borrowed");
                            return;
                        }
                        else
                        {
                            book = i;
                            g = true;
                        }
                    
                if (g == true)
                {
                    reader.AddBook(book);
                    _books.Remove(book);
                    _books.Add(new BorrowedBook(book.Name, book.Author, book.BookType, book.BookCode, book.Genre, reader,DateTime.Now));
                }
                else
                    Console.WriteLine("There is no book with the given code");
            }
            else
                Console.WriteLine("There is no books to borrow");
        }

        public void ReturnBook(string user, int code)
        {
            IBook a = new Book();
            foreach (var reader in _readers.ToList())
                if (reader.Name == user)
                    reader.ReturnBook(code, ref a);
            _books.Add(a);

        }

        public void SearchReader(string reader)
        {
            foreach(var i in _readers)
                if(i.Name.ToUpper().Contains(reader.ToUpper()))
                {
                    Console.WriteLine(i.ToString());
                    return;
                }
        }


    }
}
