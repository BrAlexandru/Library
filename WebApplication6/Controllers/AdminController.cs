using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebApplication6.Controllers
{
    public class AdminController : Controller
    {
        
        public ActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddBook(string name,string author,string type,string genre)
        {
            ViewBag.Message = string.Empty;
            


            using (var db = new LIBRARYEntities())
            {
                var book = new Book
                {
                    BookName = name,
                    BookAuthor = author,
                    BookType = type,
                    BookGenre = genre,
                    IsBorrowed = false
                };
                db.Books.Add(book);

                db.SaveChanges();
            }
            return View();
        }
        public ActionResult RemoveBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RemoveBook(int code)
        {
            ViewBag.Message = string.Empty;
            
            using (var db = new LIBRARYEntities())
            {
                try
                {
                    var book = db.Books.Where(x => x.BookID == code).Single();

                    if(book.IsBorrowed==true)
                    {
                        ViewBag.Message = "Can't remove a borrowed book";
                        return View();
                    }

                    db.Books.Remove(book);
                    db.SaveChanges();
                }
                catch(Exception)
                {
                    ViewBag.Message = "Invalid input";
                    
                }
            }
            return View();
        }
        public ActionResult RemoveReader()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RemoveReader(int code)
        {
            ViewBag.Message = string.Empty;

            

            using (var db = new LIBRARYEntities())
            {
                try
                {
                    var reader = db.Readers.Where(x => x.ReaderID == code).Single();
                    db.Readers.Remove(reader);

                    var account = db.Accounts.Where(x => x.ReaderID == code).Single();
                    db.Accounts.Remove(account);

                    foreach (var book in db.BorrowedBooks.ToList())
                        if (book.ReaderID == code)
                        {
                            if (book.ReturnDate == null)
                            {
                                ViewBag.Message = "Can't remove a reader who still has borrowed books";
                                return View();
                            }
                            db.BorrowedBooks.Remove(book);
                        }

                    db.SaveChanges();


                    return View();
                }
                catch(Exception)
                {
                    ViewBag.Message = "There is no reader with the given code";
                    return View();
                }
            }
        }
        public ActionResult DisplayBooks()
        {
            using (var db = new LIBRARYEntities())
            {
                return View(db.Books.ToList());

            }
        }

        public ActionResult DisplayReaders()
        {
            using (var db = new LIBRARYEntities())
            {
                return View(db.Readers.ToList());
            }
        }
        public ActionResult SearchBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchBook(string name,string author,string type,string genre)
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                var books = new List<Book>();
                foreach(var book in db.Books)
                {
                    if (((book.BookName.ToUpper().Contains(name.ToUpper())) || (name == "")) && ((book.BookAuthor.ToUpper().Contains(author.ToUpper())) || (author == "")) && ((book.BookType.ToUpper().Contains(type.ToUpper())) || (type == "")) && ((book.BookGenre.ToUpper().Contains(genre.ToUpper())) || (genre == "")))
                        books.Add(book);
                }
                if(books.Count==0)
                {
                    ViewBag.Message = "There is no book that match";
                    return View();
                }
                else
                {
                    return View("SearchBookResult",books);
                }
            }
        }
        public ActionResult SearchReader()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchReader(string name)
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                try
                {
                    var reader = db.Readers.Where(x => x.ReaderName == name).Single();
                    return View("SearchReaderResult", reader);

                }
                catch(Exception)
                {
                    ViewBag.Message = "There is no reader with the given name";
                    return View();
                }
            }
        }
        public ActionResult LateReaders()
        {

            ViewBag.Message = string.Empty;
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
                                        ).Select(y => new { y.X.Reader.ReaderName, y.Book.BookName, y.X.BBook.ExpectDate, y.X.BBook.ReturnDate }
                                        ).ToList()
                                        .Where(x => x.ExpectDate.AddHours(17) < DateTime.Now && x.ReturnDate == null
                                        ).OrderBy(x => x.ExpectDate).ThenBy(x => x.ReaderName).ThenBy(x => x.BookName)
                                        .Select(x => new Models.LateReaders { ReaderName = x.ReaderName, BookName = x.BookName, ExpectDate = x.ExpectDate });
                                  

                if (late.Count()==0)
                {
                    ViewBag.Message = "Nobody is late";
                    return View();
                }
                return View("LateReadersResult", late);
            }

        }
        public ActionResult BorrowedBooks()
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                var borrowed = db.BorrowedBooks.Where(x => x.ReturnDate == null)
                                               .Join(db.Books,
                                                   bbook => bbook.BookID,
                                                   book => book.BookID,
                                                   (bbook, book) => new { BBook = bbook, Book = book }
                                              ).Join(db.Readers,
                                                     x => x.BBook.ReaderID,
                                                     reader => reader.ReaderID,
                                                     (x, reader) => new { X = x, Reader = reader }
                                              ).Select(y => new { y.X.Book.BookName, y.X.BBook.BorrowedDate, y.X.BBook.ExpectDate, y.Reader.ReaderName }
                                              ).ToList().OrderBy(x => x.ReaderName).ThenBy(x => x.BookName)
                                              .Select(x => new Models.Borrowed { ReaderName = x.ReaderName, BookName = x.BookName, BorrowedDate = x.BorrowedDate, ExpectDate = x.ExpectDate });
                if(borrowed.Count()==0)
                {
                    ViewBag.Message = "There is no borrowed books";
                    return View();
                }
                return View("BorrowedBooksResult", borrowed);

            }
        }
        public ActionResult MostBorrowedBooks()
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                if(db.BorrowedBooks.Count()==0)
                {
                    ViewBag.Message = "Nobody borrowed any books";
                    return View();
                }
                var reader = db.Readers.ToList().OrderByDescending(x => x.NrOfBooks()).First();

                return View("MostBorrowedBooksResult", reader);
            }
        }
        public ActionResult Top5()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Top5(int year)
        {
            ViewBag.Message = string.Empty;
            
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
                                              .ToList()
                                              .Select(x => new Models.Top { BookName = x.BookName, BookAuthor = x.BookAuthor, Count = x.Count });
                if(books.Count()==0)
                {
                    ViewBag.Message = $"No book borrowed in the year {year}";
                    return View();
                }
                return View("Top5Result", books);
            }
        }
        public ActionResult HistoryOfTheBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HistoryOfTheBook(int code)
        {
            ViewBag.Message = string.Empty;
            
            using (var db = new LIBRARYEntities())
            {
                try
                {

                    var book = db.Books.Where(x => x.BookID == code).Single();

                    var books = db.BorrowedBooks.Where(x => x.BookID == code)
                                              .Join(db.Readers,
                                                    bbook => bbook.ReaderID,
                                                    reader => reader.ReaderID,
                                                    (bbook, reader) => new { BBook = bbook, Reader = reader })
                                              .Select(y => new { y.Reader.ReaderName, y.BBook.BorrowedDate, y.BBook.ReturnDate, y.BBook.ExpectDate })
                                              .OrderBy(x => x.BorrowedDate).ToList()
                                              .Select(x => new Models.BookHistory { BookName = book.BookName, ReaderName = x.ReaderName, BorrowedDate = x.BorrowedDate, ExpectDate = x.ExpectDate, ReturnDate = x.ReturnDate });
                    if(books.Count()==0)
                    {
                        ViewBag.Message = $"{book.BookName} was never borrowed";
                        return View();
                    }
                    return View("HistoryOfTheBookResult", books);
                }
                catch(Exception)
                {
                    ViewBag.Message = "No book with the given code";
                    return View();
                }
            }
        }
        public ActionResult HistoryOfTheReader()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HistoryOfTheReader(int code)
        {
            ViewBag.Message = string.Empty;
            
            using (var db = new LIBRARYEntities())
            {
                try
                {
                    var reader = db.Readers.Where(x => x.ReaderID == code).Single();

                    var history = db.BorrowedBooks.Where(x => x.ReaderID == code)
                                                  .Join(db.Books,
                                                        bbook => bbook.BookID,
                                                        book => book.BookID,
                                                        (bbook, book) => new { BBook = bbook, Book = book })
                                                  .Select(y => new { y.BBook.BorrowedDate, y.BBook.ExpectDate, y.BBook.ReturnDate, y.Book.BookName })
                                                  .OrderBy(x => x.BorrowedDate).ToList()
                                                  .Select(x=>new Models.ReaderHistory { ReaderName=reader.ReaderName,BookName=x.BookName,BorrowedDate=x.BorrowedDate,ExpectDate=x.ExpectDate,ReturnDate=x.ReturnDate});
                    if(history.Count()==0)
                    {
                        ViewBag.Message = "The reader never borrowed a book";
                        return View();
                    }
                    return View("HistoryOfTheReaderResult", history);
                }
                catch(Exception)
                {
                    ViewBag.Message = "No reader with the given code";
                    return View();
                }
            }

        }
        public ActionResult ReadersAndBooks()
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                var readers = db.Readers.ToList()
                                      .Where(x => x.NrOfBooks() > 0)
                                      .Select(x => new { x.ReaderName, Nr = x.NrOfBooks() })
                                      .OrderByDescending(x => x.Nr).ThenBy(x => x.ReaderName)
                                      .Select(x => new Models.ReaderAndNr { ReaderName = x.ReaderName, Count = x.Nr });
                if(readers.Count()==0)
                {
                    ViewBag.Message = "Nobody borrowed any book";
                    return View();
                }
                return View("ReadersAndBooksResult", readers);

            }
        }

    }
}