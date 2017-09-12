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

        public void Menu()
        {
            var options = new Dictionary<string, Action>()
            {
                {"D1",()=>Start() },
                {"D2",()=>ForgotPass() },
                {"D3",()=>AddReader() }

            };
            ConsoleKeyInfo key;
            do
            {
                
                Console.WriteLine("1.Login");
                Console.WriteLine("2.Forgot my password");
                Console.WriteLine("3.Create an account");
               
                key = Console.ReadKey();
                Console.WriteLine();

                Action value = null;
                if (options.TryGetValue(key.Key.ToString(), out value))
                {
                    value();
                }
                
                else Console.WriteLine("Command does not exist");

                Console.WriteLine();
            } while (key.Key != ConsoleKey.Escape);


        }
        


        private void Start()
        {
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
                                ReaderInstruction(account.ReaderID);
                                ok = true;
                            }
                    if (ok == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong username and password");
                        Console.ResetColor();
                    }
                    }
                }
              
            
        }

        #region Admin


        private void AdminInstruction()
        {

            var adminInstruction = new Dictionary<string, Action>()
            {
                {"A",()=>AddBook() },
                {"B",()=>RemoveBook() },
                {"C",()=>RemoveReader() },
                {"D",()=>DisplayBooks() },
                {"E",()=>DisplayReaders() },
                {"F",()=>SearchBook() },
                {"G",()=>SearchReader() },
                {"H",()=>LateReaders() },
                {"I",()=>DisplayBorrowed() },
                {"J",()=>MaxBooks() },
                {"K",()=>Top() },
                {"L",()=>HistoryOfTheBook() },
                {"M",()=>HistroyOfTheReader() },
                {"N",()=>ReaderNrBooks() }
            };
            
            

            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine();
                Console.WriteLine("A.Add book");
                Console.WriteLine("B.Remove book");
                Console.WriteLine("C.Remove reader");
                Console.WriteLine("D.Display all books");
                Console.WriteLine("E.Display all readers");
                Console.WriteLine("F.Search book");
                Console.WriteLine("G.Search reader");
                Console.WriteLine("H.Display the readers who are late");
                Console.WriteLine("I.Display all the borrowed books");
                Console.WriteLine("J.The readers with the most borrowed books");
                Console.WriteLine("K.Top 5 borrowed books");
                Console.WriteLine("L.Display history of book");
                Console.WriteLine("M.Display history of reader");
                Console.WriteLine("N.Display the readers and the number of books they have");

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
                    BookGenre=genre,
                    IsBorrowed=false
                };
                db.Books.Add(book);

                db.SaveChanges();
            }

        }

      

        private void RemoveBook()
        {
            try
            {
                Console.Write("Code of the book:");
                int code = int.Parse(Console.ReadLine());
                if(code<=0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                using (var db = new LIBRARYEntities())
                { 
                    var book = db.Books.Where(x => x.BookID == code).Single();
                    
                    if(book.IsBorrowed==true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Can't remove a borrowed book");
                        Console.ResetColor();
                        return;
                    }
                    foreach (var item in db.BorrowedBooks.ToList())
                        if (item.BookID == book.BookID)
                            db.BorrowedBooks.Remove(item);

                    
                    
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
                if(code<=0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                using (var db = new LIBRARYEntities())
                {
                    var reader = db.Readers.Where(x => x.ReaderID == code).Single();
                    db.Readers.Remove(reader);

                    var account = db.Accounts.Where(x => x.ReaderID == code).Single();
                    db.Accounts.Remove(account);

                    foreach(var book in db.BorrowedBooks.ToList())
                        if(book.ReaderID==code)
                        {
                            if(book.ReturnDate==null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Can't remove a reader who still has borrowed books");
                                Console.ResetColor();
                                return;
                            }
                            db.BorrowedBooks.Remove(book);
                        }

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
                db.Readers.ToList().ForEach(reader => Console.WriteLine($"{reader.ReaderID,-3} {reader.ReaderName,-10} {reader.ReaderPhone,-10} {reader.ReaderEmail,-20} {reader.ReaderAddress}"));
                

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
                try
                {
                    var reader = db.Readers.Where(x => x.ReaderName == name).Single();

                    Console.WriteLine($"{reader.ReaderID,-3} {reader.ReaderName,-10} {reader.ReaderPhone,-10} {reader.ReaderEmail,-20} {reader.ReaderAddress}");
                }
                catch(Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There is no reader with the given name");
                    Console.ResetColor();
                    return;
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
                                               (x, book) => new { X=x, Book = book }
                                        ).Select(y => new { y.X.Reader.ReaderName, y.Book.BookName, y.X.BBook.ExpectDate,y.X.BBook.ReturnDate }                                        
                                        ).ToList()
                                        .Where(x => x.ExpectDate.AddHours(17) < DateTime.Now && x.ReturnDate==null
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
                    Console.WriteLine($"{item.ReaderName,-10} {item.BookName,-10} {item.ExpectDate.ToString("dd MMM yyyy")}");
                });
                
                
            }
        }

        private void DisplayBorrowed()
        {
            using (var db = new LIBRARYEntities())
            {
                var borrowed = db.BorrowedBooks.Where(x=>x.ReturnDate==null)
                                               .Join(db.Books,
                                                   bbook => bbook.BookID,
                                                   book => book.BookID,
                                                   (bbook, book) => new { BBook = bbook, Book = book }
                                              ).Join(db.Readers,
                                                     x => x.BBook.ReaderID,
                                                     reader => reader.ReaderID,
                                                     (x, reader) => new { X = x, Reader = reader }
                                              ).Select(y => new {  y.X.Book.BookName, y.X.BBook.BorrowedDate, y.X.BBook.ExpectDate,  y.Reader.ReaderName }
                                              ).ToList().OrderBy(x => x.ReaderName).ThenBy(x => x.BookName);
                if(borrowed.Count()==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There are no borrowed books");
                    Console.ResetColor();
                    return;
                }

                borrowed.ToList().ForEach(book =>
                {
                    Console.WriteLine($"{book.ReaderName,-10 } borrowed {book.BookName,-10} from {book.BorrowedDate.ToString("dd MMM yyyy"),-11} untill {book.ExpectDate.ToString("dd MMM yyyy")}");
                });
                
                    
                    
            }
        }

        private void HistoryOfTheBook()
        {
            using (var db = new LIBRARYEntities())
            {
                try
                {
                    Console.Write("Code of the book:");
                    int code = int.Parse(Console.ReadLine());
                    if(code<=0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        Console.ResetColor();
                        return;
                    }
                    var book = db.Books.Where(x => x.BookID == code).Single();

                    var books = db.BorrowedBooks.Where(x => x.BookID == code)
                                              .Join(db.Readers,
                                                    bbook => bbook.ReaderID,
                                                    reader => reader.ReaderID,
                                                    (bbook, reader) => new { BBook = bbook, Reader = reader })
                                              .Select(y => new { y.Reader.ReaderName, y.BBook.BorrowedDate, y.BBook.ReturnDate, y.BBook.ExpectDate })
                                              .OrderBy(x => x.BorrowedDate).ToList();
                    if(books.Count==0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{book.BookName} was never borrowed");
                        Console.ResetColor();
                        return;
                    }

                    foreach (var item in books)
                    {
                        if (item.ReturnDate != null)
                            Console.WriteLine($"{book.BookName} was borrowed by {item.ReaderName} in {item.BorrowedDate.ToString("dd MMM yyyy")} and was returned in {item.ReturnDate.Value.ToString("dd MMM yyyy")} ");
                        else
                            Console.WriteLine($"{book.BookName} was borrowed by {item.ReaderName} in {item.BorrowedDate.ToString("dd MMM yyyy")} and need to be returned untill {item.ExpectDate.ToString("dd MMM yyyy")} ");
                    }
                    

                }
                catch(FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }
                catch(Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No book with the given code");
                    Console.ResetColor();
                    return;
                }
            }
        }

        private void HistroyOfTheReader()
        {
            using (var db = new LIBRARYEntities())
            {
                if (db.Readers.Count() == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No readers");
                    Console.ResetColor();
                    return;
                }
                try
                {

                    Console.Write("Code of the reader:");
                    int code = int.Parse(Console.ReadLine());
                    if (code <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        Console.ResetColor();
                        return;
                    }

                    var reader = db.Readers.Where(x => x.ReaderID == code).Single();

                    var history = db.BorrowedBooks.Where(x => x.ReaderID == code)
                                                  .Join(db.Books,
                                                        bbook => bbook.BookID,
                                                        book => book.BookID,
                                                        (bbook, book) => new { BBook = bbook, Book = book })
                                                  .Select(y => new { y.BBook.BorrowedDate, y.BBook.ExpectDate, y.BBook.ReturnDate, y.Book.BookName })
                                                  .OrderBy(x => x.BorrowedDate).ToList();
                    if (history.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{reader.ReaderName} never borrowed a book");
                        Console.ResetColor();
                        return;
                    }
                    foreach (var item in history)
                    {
                        if (item.ReturnDate != null)
                            Console.WriteLine($"{reader.ReaderName} borrowed {item.BookName} in {item.BorrowedDate.ToString("dd MMM yyyy")} and returned it in {item.ReturnDate.Value.ToString("dd MMM yyyy")}");
                        else
                            Console.WriteLine($"{reader.ReaderName} borrowed {item.BookName} in {item.BorrowedDate.ToString("dd MMM yyyy")} and have to return it untill {item.ExpectDate.ToString("dd MMM yyyy")}");
                    }

                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No reader with the given code");
                    Console.ResetColor();
                    return;
                }

            }
        }

        private void ReaderNrBooks()
        {
            using (var db = new LIBRARYEntities())
            {

                var readers = db.Readers.ToList()
                                      .Where(x => x.NrOfBooks() > 0)
                                      .Select(x => new { x.ReaderName, Nr = x.NrOfBooks() })
                                      .OrderByDescending(x => x.Nr).ThenBy(x=>x.ReaderName);
                
                foreach (var reader in readers)
                    Console.WriteLine($"{reader.ReaderName} {reader.Nr}");

            }
        }

        #endregion


        private  void Top()
        {
            try
            { 
            Console.Write("Year:");
            int year = int.Parse(Console.ReadLine());

                using (var db = new LIBRARYEntities())
                {


                    var books = db.BorrowedBooks.Where(x => x.BorrowedDate.Year == year)
                                              .GroupBy(x => x.BookID)
                                              .Select(y => new { y.Key, Count = y.Count() })
                                              .Join(db.Books,
                                                    x => x.Key,
                                                    book => book.BookID,
                                                    (x, book) => new { X = x, Book = book })
                                              .Select(y => new { y.Book.BookName, y.Book.BookAuthor, y.X.Count })
                                              .OrderByDescending(x => x.Count)
                                              .Take(5)
                                              .ToList();
                    if(books.Count==0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"No books borrowed in the year {year}");
                        Console.ResetColor();
                        return;
                    }

                    foreach (var book in books)
                        Console.WriteLine($"{book.BookName,-10} by {book.BookAuthor,-10} was borrowed {book.Count,-3} times");
                }
                
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }
        }

        private void ForgotPass()
        {
            using (var db = new LIBRARYEntities())
            {
                Console.Write("Username:");
                string user = Console.ReadLine();

                try
                {
                    var account = db.Accounts.Where(x => x.AccountUsername == user).Single();

                    var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();

                    Console.Write("Phone Number:");
                    if(long.Parse(Console.ReadLine())!=reader.ReaderPhone)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong");
                        Console.ResetColor();
                        return;
                    }

                    Console.Write("Email:");
                    if(Console.ReadLine()!=reader.ReaderEmail)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong");
                        Console.ResetColor();
                        return;
                    }

                    Console.Write("Address:");
                    if(Console.ReadLine()!=reader.ReaderAddress)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong");
                        Console.ResetColor();
                        return;
                    }

                    Console.Write("New password:");
                    string pass = Console.ReadLine();

                    if(pass==string.Empty)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        Console.ResetColor();
                        return;
                    }
                    

                    account.AccountPassword = pass;

                    db.SaveChanges();

                    


                    
                }
                catch(FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong");
                    Console.ResetColor();
                }
                catch(Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No account with the given username");
                    Console.ResetColor();
                }
                
                

                
                
            }
        }

        private void AddReader()
        {
            try
            {
                Console.Write("Name of the reader:");
                string name = Console.ReadLine();
                if (name == string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                Console.Write("Phone of the reader:");
                long phone = long.Parse(Console.ReadLine());
                if (phone < 100000000 || phone > 9999999999)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                Console.Write("Email of the reader:");
                string email = Console.ReadLine();
                if (email == string.Empty)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }

                Console.Write("Address of the reader:");
                string address = Console.ReadLine();
                if (address == string.Empty)
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
                    foreach (var item in db.Readers)
                        if (reader.ReaderName == item.ReaderName)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Username already used");
                            Console.ResetColor();
                            return;
                        }
                    db.Readers.Add(reader);

                    var account = new Account
                    {
                        AccountUsername = name,
                        AccountPassword = name,
                        ReaderID = reader.ReaderID
                    };

                    db.Accounts.Add(account);

                    db.SaveChanges();

                }

            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input");
                Console.ResetColor();
                return;
            }

        }



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
                {"F",()=>DisplayReaderBorrowed(id) },
                {"G",()=>Top() },
                {"H",()=>HistroyOfTheReader(id) }
                

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
                Console.WriteLine("G.Top 5 most borrowed books");
                Console.WriteLine("H.Display history of reader");


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
                

                var account = db.Accounts.Where(x => x.ReaderID == id).Single();

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
                    if(pass==account.AccountPassword)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("New password can't be the same password");
                        Console.ResetColor();
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
                var notBorrowed = db.Books.Where(x => x.IsBorrowed==false).ToList();
               

                if(notBorrowed.Count==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("All books are borrowed");
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
                var notBorrowed = db.Books.Where(x => x.IsBorrowed == false).ToList();

                if(notBorrowed.Count==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No books to display");
                    Console.ResetColor();
                    return;
                }

                foreach (var book in notBorrowed)
                    Console.WriteLine($"{book.BookID,-3} {book.BookName,-10} {book.BookAuthor,-10} {book.BookType,-10} {book.BookGenre}");
            }
        }

        private void BorrowBook(int id)
        {
            using (var db = new LIBRARYEntities())
            {

                

                var reader = db.Readers.Where(x => x.ReaderID == id).Single();

                if(reader.NrOfBooks()>=4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Can't borrow more books");
                    Console.ResetColor();
                    return;
                }

                var books = db.Books.Where(x => x.IsBorrowed == false).ToList();
                
                if(books.Count==0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No books to borrow");
                    Console.ResetColor();
                    return;
                }
                try
                {
                    Console.Write("Code of the book:");
                    int code = int.Parse(Console.ReadLine());
                    if(code<=0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        Console.ResetColor();
                        return;
                    }

                    var book = books.Where(x => x.BookID == code).Single();

                    var bbook = new BorrowedBook
                    {
                        BookID = book.BookID,
                        ReaderID = reader.ReaderID,
                        BorrowedDate = DateTime.Now,
                        ExpectDate = DateTime.Now.AddMonths(1)
                    };

                    book.IsBorrowed = true;
                    db.BorrowedBooks.Add(bbook);

                    db.SaveChanges();

                }
                catch(FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input");
                    Console.ResetColor();
                    return;
                }
                catch(Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No book with the given code");
                    Console.ResetColor();
                    return;
                }
                   
            }
        }

        private void ReturnBook(int id)
        {
            using (var db = new LIBRARYEntities())
            {

               

                var reader = db.Readers.Where(x => x.ReaderID == id).Single();

                try
                {
                    if(reader.NrOfBooks()==0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No book to return");
                        Console.ResetColor();
                        return;
                    }

                    Console.Write("Code of the book:");
                    int code = int.Parse(Console.ReadLine());
                    if(code<=0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        Console.ResetColor();
                        return;

                    }

                    var bbook = db.BorrowedBooks.Where(x => x.ReaderID == reader.ReaderID && x.ReturnDate == null && x.BookID == code).Single();

                    bbook.ReturnDate = DateTime.Now;

                    var book = db.Books.Where(x => x.BookID == code).Single();

                    book.IsBorrowed = false;

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
                var reader = db.Readers.Where(x => x.ReaderID == id).Single();

                var books = db.BorrowedBooks.Where(x=>x.ReturnDate==null).Where(x => x.ReaderID == reader.ReaderID)
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

        private void HistroyOfTheReader(int code)
        {
            using (var db = new LIBRARYEntities())
            {
                if (db.Readers.Count() == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No readers");
                    Console.ResetColor();
                    return;
                }
                try
                {

                    var reader = db.Readers.Where(x => x.ReaderID == code).Single();

                    var history = db.BorrowedBooks.Where(x => x.ReaderID == code)
                                                  .Join(db.Books,
                                                        bbook => bbook.BookID,
                                                        book => book.BookID,
                                                        (bbook, book) => new { BBook = bbook, Book = book })
                                                  .Select(y => new { y.BBook.BorrowedDate, y.BBook.ExpectDate, y.BBook.ReturnDate, y.Book.BookName })
                                                  .OrderBy(x => x.BorrowedDate).ToList();
                    if (history.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{reader.ReaderName} never borrowed a book");
                        Console.ResetColor();
                        return;
                    }
                    foreach (var item in history)
                    {
                        if (item.ReturnDate != null)
                            Console.WriteLine($"{reader.ReaderName} borrowed {item.BookName} in {item.BorrowedDate.ToString("dd MMM yyyy")} and returned it in {item.ReturnDate.Value.ToString("dd MMM yyyy")}");
                        else
                            Console.WriteLine($"{reader.ReaderName} borrowed {item.BookName} in {item.BorrowedDate.ToString("dd MMM yyyy")} and have to return it untill {item.ExpectDate.ToString("dd MMM yyyy")}");
                    }

                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No reader with the given code");
                    Console.ResetColor();
                    return;
                }

            }
        }

        #endregion

    }
}
