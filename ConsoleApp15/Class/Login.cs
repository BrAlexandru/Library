using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp15
{
    class Login
    {

        private Librarian _librarian;

        public Login(Librarian librarian)
        {
            _librarian = librarian;
        }


        public void Start()
        {
            ConsoleKeyInfo g;
            do
            {

                Console.WriteLine("Press Escape to stop");
                g = Console.ReadKey();

                if (g.Key == ConsoleKey.Escape)
                    break;
                Console.Write("Username:");
                string user = Console.ReadLine();
                Console.Write("Password:");
                string pass = Console.ReadLine();

                if((user==_librarian.AdminID) && (pass==_librarian.Password))
                {
                    AdminInstruction();
                }
                else
                {
                    using (var db = new LIBRARYEntities())
                    {
                        bool ok = false;
                        foreach(var account in db.Accounts)
                            if((user==account.AccountUsername) && (pass==account.AccountPassword))
                            {
                                ReaderInstruction(account.AccountID);
                                ok = true;
                            }
                        if (ok == false)
                            Console.WriteLine("Wrong username and password");
                    }
                }

            } while (g.Key != ConsoleKey.Escape);
        }

        #region Admin


        private void AdminInstruction()
        {

            var adminInstruction = new Dictionary<string, Action>()
            {
                {"A",()=>AddBook() },
                {"B",()=>AddReader() },
                {"C",()=>RemoveBook() },
                {"D",()=>RemoveReader() },
                {"E",()=>DisplayBooks() },
                {"F",()=>DisplayReaders() },
                {"G",()=>SearchBook() },
                {"H",()=>SearchReader() },
                {"I",()=>LateReaders() },
                {"J",()=>DisplayBorrowed() },
                {"K",()=>MaxBooks() }
            };
            
            

            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine();
                Console.WriteLine("A.Add book");
                Console.WriteLine("B.Add reader");
                Console.WriteLine("C.Remove book");
                Console.WriteLine("D.Remove reader");
                Console.WriteLine("E.Display all books");
                Console.WriteLine("F.Display all readers");
                Console.WriteLine("G.Search book");
                Console.WriteLine("H.Search reader");
                Console.WriteLine("I.Display the readers who are late");
                Console.WriteLine("J.Display all the borrowed books");
                Console.WriteLine("K.The readers with the most borrowed books");

                key = Console.ReadKey();
                Console.WriteLine();

                Action value = null;
                if (adminInstruction.TryGetValue(key.Key.ToString(), out value))
                {
                    value();
                }
                else Console.WriteLine("Command does not exist");

                Console.WriteLine();  
            } while (key.Key != ConsoleKey.Escape);


        }

        private void AddBook()
        {
            Console.Write("Name of the book:");
            string name = Console.ReadLine();
            if(name==string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }

            Console.Write("Name of the author:");
            string author = Console.ReadLine();
            if(author==string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }

            Console.Write("Type of the book");
            string type = Console.ReadLine();
            if(type==string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }

            Console.Write("Genre of the book:");
            string genre = Console.ReadLine();
            if(genre==string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }

            using (var db = new LIBRARYEntities())
            {
                var book = new Book
                {
                    BookName = name,
                    BookAuthor=author,
                    BookType=type,
                    BookGenre=genre
                };
                db.Books.Add(book);

                db.SaveChanges();
            }

        }

        private void AddReader()
        {
            try
            {
                Console.Write("Name of the reader:");
                string name = Console.ReadLine();
                if(name==string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                Console.Write("Phone of the reader:");
                long phone = long.Parse(Console.ReadLine());

                Console.Write("Email of the reader:");
                string email = Console.ReadLine();
                if(email==string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                Console.Write("Address of the reader:");
                string address = Console.ReadLine();
                if(address==string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                using (var db = new LIBRARYEntities())
                {

                    var reader = new Reader
                    {
                        ReaderName = name,
                        ReaderPhone = phone,
                        ReaderEmail = email,
                        ReaderAddress = address
                    };

                    db.Readers.Add(reader);

                    var account = new Account
                    {
                        AccountUsername = name,
                        AccountPassword = name,
                        ReaderID=reader.ReaderID
                    };

                    db.Accounts.Add(account);

                    db.SaveChanges();

                }

            }
            catch(FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }
        }

        private void RemoveBook()
        {
            try
            {
                Console.Write("Code of the book:");
                int code = int.Parse(Console.ReadLine());

                using (var db = new LIBRARYEntities())
                { 
                    var book = db.Books.Where(x => x.BookID == code).Single();

                    

                    foreach(var item in db.BorrowedBooks)
                        if(item.BookID==book.BookID)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Can't remove a borrowed book");
                            Console.ResetColor();
                            return;
                        }
                    
                    
                    db.Books.Remove(book);

                    db.SaveChanges();
                }

            }
            catch(FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();

            }
            catch(Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;                
                Console.WriteLine("There is no book with the given code");
                Console.ResetColor();
            }
        }

        private void RemoveReader()
        {
            try
            {
                Console.Write("Id of the reader:");
                int code = int.Parse(Console.ReadLine());

                using (var db = new LIBRARYEntities())
                {
                    var reader = db.Readers.Where(x => x.ReaderID == code).Single();
                    db.Readers.Remove(reader);

                    var account = db.Accounts.Where(x => x.ReaderID==code).Single();
                    db.Accounts.Remove(account);

                    foreach (var book in db.BorrowedBooks.ToList())
                        if (book.ReaderID == reader.ReaderID)
                            db.BorrowedBooks.Remove(book);

                    db.SaveChanges();
                }

            }
            catch(FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
            }
            catch(Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no reader with the given code");
                Console.ResetColor();
            }
        }

        private void DisplayBooks()
        {
            using (var db = new LIBRARYEntities())
            {
                if(db.Books.Count()==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no books to display");
                    Console.ResetColor();
                    return;
                }

                db.Books.ToList().ForEach(book => Console.WriteLine($"{book.BookID,-3} {book.BookName,-10} {book.BookAuthor,-10} {book.BookType,-10} {book.BookGenre}"));

            }
        }

        private void DisplayReaders()
        {
            using (var db = new LIBRARYEntities())
            {
                
                if(db.Readers.Count()==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no readers to display");
                    Console.ResetColor();
                    return;
                }
                db.Readers.ToList().ForEach(reader => Console.WriteLine($"{reader.ReaderID,-3} {reader.ReaderName,-10} {reader.ReaderPhone,-10} {reader.ReaderEmail,-10} {reader.ReaderAddress}"));
                

            }
        }

        private void MaxBooks()
        {
            using (var db = new LIBRARYEntities())
            {
                if(db.BorrowedBooks.Count()==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Nobody borrowed any book");
                    Console.ResetColor();
                    return;
                }

                var reader = db.Readers.ToList().OrderByDescending(x => x.NrOfBooks()).First();

                Console.WriteLine($"{reader.ReaderID,-3} {reader.ReaderName,-10} {reader.NrOfBooks()}");
                
            }
        }

        private void SearchBook()
        {
            Console.Write("Name of the book:");
            string name = Console.ReadLine();

            Console.Write("Name of the author:");
            string author = Console.ReadLine();

            Console.Write("Type of the book:");
            string type = Console.ReadLine();

            Console.Write("Genre of the book:");
            string genre = Console.ReadLine();

            using (var db = new LIBRARYEntities())
            {
                bool g = false;
                db.Books.ToList().ForEach(book =>
                {
                    if (((book.BookName.ToUpper().Contains(name.ToUpper())) || (name == "")) && ((book.BookAuthor.ToUpper().Contains(author.ToUpper())) || (author == "")) && ((book.BookType.ToUpper().Contains(type.ToUpper())) || (type == "")) && ((book.BookGenre.ToUpper().Contains(genre.ToUpper())) || (genre == "")))
                    {
                        Console.WriteLine($"{book.BookID,-3} {book.BookName,-10} {book.BookAuthor,-10} {book.BookType,-10} {book.BookGenre}");
                        g = true;
                    }
                });
                if (g == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no book that match");
                    Console.ResetColor();
                }
            }

        }

        private void SearchReader()
        {
            Console.Write("Name of the reader:");
            string name = Console.ReadLine();
            if(name==string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }

            using (var db = new LIBRARYEntities())
            {
                bool g = false;
                db.Readers.ToList().ForEach(reader =>
                {
                    if (reader.ReaderName == name)
                    {
                        Console.WriteLine($"{reader.ReaderID,-3} {reader.ReaderName,-10} {reader.ReaderPhone,-10} {reader.ReaderEmail,-10} {reader.ReaderAddress}");
                        g = true;
                    }
                });
                if (g == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no reader with the given name");
                    Console.ResetColor();
                }
                       
            }
        }

        private void LateReaders()
        {
            using (var db = new LIBRARYEntities())
            {
                var late = db.BorrowedBooks.Join(db.Readers,
                                               bbook => bbook.ReaderID,
                                               reader => reader.ReaderID,
                                               (bbook, reader) => new { BBook = bbook, Reader = reader }
                                        ).Join(db.Books,
                                               x => x.BBook.BookID,
                                               book => book.BookID,
                                               (x, book) => new { X = x, Book = book }
                                        ).Select(y => new { y.X.Reader.ReaderName,y.X.Reader.ReaderID, y.Book.BookName,y.Book.BookID, y.X.BBook.ExpectDate }                                        
                                        ).ToList()
                                        .Where(x => x.ExpectDate.AddHours(17) < DateTime.Now
                                        ).OrderBy(x => x.ExpectDate).ThenBy(x => x.ReaderName).ThenBy(x => x.BookName);
                if (late.Count() == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Nobody is late");
                    Console.ResetColor();
                    return;
                }
                late.ToList().ForEach(item =>
                {
                    Console.WriteLine($"({item.ReaderID}){item.ReaderName,-10} ({item.BookID}){item.BookName,-10} {item.ExpectDate.ToString("dd MMM yyyy")}");
                });
                
                
            }
        }

        private void DisplayBorrowed()
        {
            using (var db = new LIBRARYEntities())
            {
                var borrowed = db.BorrowedBooks.Join(db.Books,
                                                   bbook => bbook.BookID,
                                                   book => book.BookID,
                                                   (bbook, book) => new { BBook = bbook, Book = book }
                                              ).Join(db.Readers,
                                                     x => x.BBook.ReaderID,
                                                     reader => reader.ReaderID,
                                                     (x, reader) => new { X = x, Reader = reader }
                                              ).Select(y => new { y.X.Book.BookID, y.X.Book.BookName, y.X.BBook.BorrowedDate, y.X.BBook.ExpectDate, y.Reader.ReaderID, y.Reader.ReaderName }
                                              ).ToList().OrderBy(x => x.BookID).ThenBy(x => x.ReaderID);
                if(borrowed.Count()==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There are no borrowed books");
                    Console.ResetColor();
                    return;
                }

                borrowed.ToList().ForEach(book =>
                {
                    Console.WriteLine($"({book.ReaderID}){book.ReaderName,-10 } borrowed ({book.BookID}){book.BookName,-10} from {book.BorrowedDate.ToString("dd MMM yyyy"),-11} untill {book.ExpectDate.ToString("dd MMM yyyy")}");
                });
                
                    
                    
            }
        }


        #endregion

        #region Reader

        private void ReaderInstruction(int id)
        {
            var readerInstruction = new Dictionary<string, Action>()
            {
                {"A",()=>ChangePass(id) },
                {"B",()=>ReaderSearchBook() },
                {"C",()=>ReaderBookDisplay() },
                {"D",()=>BorrowBook(id) },
                {"E",()=>ReturnBook(id) },
                {"F",()=>DisplayReaderBorrowed(id) }
                

            };

            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine();
                Console.WriteLine("A.Change password");
                Console.WriteLine("B.Search book");
                Console.WriteLine("C.Display books");
                Console.WriteLine("D.Borrow book");
                Console.WriteLine("E.Return book");
                Console.WriteLine("F.Display the borrowed books");
                


                key = Console.ReadKey();
                Console.WriteLine();

                Action value = null;
                if (readerInstruction.TryGetValue(key.Key.ToString(), out value))
                {
                    value();
                }
                else Console.WriteLine("Command does not exist");

                Console.WriteLine();
            } while (key.Key != ConsoleKey.Escape);
        }

        private void ChangePass(int id)
        {
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                Console.Write("Current password:");
                if (Console.ReadLine() != account.AccountPassword)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong password");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("New password:");
                    string pass = Console.ReadLine();
                    if (pass == String.Empty)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong input");
                        Console.ResetColor();
                        return;
                    }
                    account.AccountPassword = pass;

                    db.SaveChanges();

                }
            }

        }

        public static void ReaderSearchBook()
        {
            using (var db = new LIBRARYEntities())
            {
                var borrowed = db.BorrowedBooks.Select(x => x.BookID).ToList();

                var notBorrowed = db.Books.Where(x => !borrowed.Contains(x.BookID)).ToList();

                if(notBorrowed.Count==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("All the books are borrowed");
                    Console.ResetColor();
                    return;
                }

                Console.Write("Name of the book:");
                string name = Console.ReadLine();

                Console.Write("Name of the author:");
                string author = Console.ReadLine();

                Console.Write("Type of the book:");
                string type = Console.ReadLine();

                Console.Write("Genre of the book");
                string genre = Console.ReadLine();

                bool g = false;
                foreach (var book in notBorrowed)
                    if (((book.BookName.ToUpper().Contains(name.ToUpper())) || (name == "")) && ((book.BookAuthor.ToUpper().Contains(author.ToUpper())) || (author == "")) && ((book.BookType.ToUpper().Contains(type.ToUpper())) || (type == "")) && ((book.BookGenre.ToUpper().Contains(genre.ToUpper())) || (genre == "")))
                    {
                        Console.WriteLine($"{book.BookID,-3} {book.BookName,-10} {book.BookAuthor,-10} {book.BookType,-10} {book.BookGenre}");
                        g = true;
                    }
                if (g == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no book that match");
                    Console.ResetColor();
                }



            }
        }

        private void ReaderBookDisplay()
        {
            using (var db = new LIBRARYEntities())
            {
                var borrowed = db.BorrowedBooks.Select(x => x.BookID).ToList();

                var books = db.Books.Where(x => !borrowed.Contains(x.BookID)).ToList();

                if(books.Count==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No books to display");
                    Console.ResetColor();
                    return;
                }

                foreach (var book in books)
                    Console.WriteLine($"{book.BookID,-3} {book.BookName,-10} {book.BookAuthor,-10} {book.BookType,-10} {book.BookGenre}");
            }
        }

        private void BorrowBook(int id)
        {
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();
                if(reader.NrOfBooks()>=4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Can't borrow more then 4 books");
                    Console.ResetColor();
                    return;
                }

                try
                {
                    var borrowed = db.BorrowedBooks.Select(x => x.BookID).ToList();

                    var books = db.Books.Where(x => !borrowed.Contains(x.BookID)).ToList();

                    if (books.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("There are no books to borrow");
                        Console.ResetColor();
                        return;
                    }

                    Console.Write("Insert code of the book:");
                    int code = int.Parse(Console.ReadLine());

                    var book = books.Where(x => x.BookID == code).Single();

                    var bbook = new BorrowedBook
                    {
                        BookID = book.BookID,
                        ReaderID = account.ReaderID,
                        BorrowedDate = DateTime.Now,
                        ExpectDate = DateTime.Now.AddMonths(1)
                    };

                    db.BorrowedBooks.Add(bbook);

                    db.SaveChanges();


                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                }
                catch(Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no book with the give code that ca be borrowed");
                    Console.ResetColor();
                }
                

            }
        }

        private void ReturnBook(int id)
        {
            using (var db = new LIBRARYEntities())
            {

                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                

                try
                {
                    Console.Write("Code of the book:");
                    int code = int.Parse(Console.ReadLine());

                    var book = db.BorrowedBooks.Where(x => x.BookID == code && x.ReaderID == account.ReaderID).Single();

                    db.BorrowedBooks.Remove(book);

                    db.SaveChanges();


                }
                catch(FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                }
                catch(Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no book with the given code that can be returned");
                    Console.ResetColor();
                }

            }
        }

        private void DisplayReaderBorrowed(int id)
        {
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.ReaderID == id).Single();

                var books = db.BorrowedBooks.Where(x => x.ReaderID == account.ReaderID)
                                            .Join(db.Books,
                                                  bbook => bbook.BookID,
                                                  book => book.BookID,
                                                  (bbook, book) => new { BBook = bbook, Book = book }
                                           ).Select(y => new {y.Book.BookID ,y.Book.BookName,y.BBook.ExpectDate })
                                           .ToList().OrderBy(x=>x.BookID);
                if(books.Count()==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No book borrowed");
                    Console.ResetColor();
                    return;
                }
                foreach (var book in books)
                    Console.WriteLine($"{book.BookID,-3} {book.BookName,-10} untill {book.ExpectDate.ToString("dd MMM yyyy")}");

            }
        }

        #endregion

    }
}
