using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication6.Controllers
{
    public class ReaderController : Controller
    {
        public ActionResult ChangePass(int id)
        {
            return View(id);
        }
        [HttpPost]
        public ActionResult ChangePass(int id,string current,string newPass)
        {
            ViewBag.Message = string.Empty;

            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                if(current!=account.AccountPassword)
                {
                    ViewBag.Message = "Wrong password";
                    return View(id);
                }
                if(newPass==current)
                {
                    ViewBag.Message = "New password can't be the old password";
                    return View(id);
                }

                account.AccountPassword = newPass;

                db.SaveChanges();

                return View(id);
            }

        }
        
        public ActionResult SearchBook(int id)
        {
            return View(id);
        }
        [HttpPost]
        public ActionResult SearchBook(int id,string name,string author,string type,string genre)
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                var books = new List<Book>();


                var notBorrowed = db.Books.Where(x => x.IsBorrowed == false).ToList();

                foreach(var book in notBorrowed)
                {
                    if (((book.BookName.ToUpper().Contains(name.ToUpper())) || (name == "")) && ((book.BookAuthor.ToUpper().Contains(author.ToUpper())) || (author == "")) && ((book.BookType.ToUpper().Contains(type.ToUpper())) || (type == "")) && ((book.BookGenre.ToUpper().Contains(genre.ToUpper())) || (genre == "")))
                        books.Add(book);
                }
                if(books.Count==0)
                {
                    ViewBag.Message = "There is no book that match";
                    return View(id);
                }
                else
                {

                    var model = new Models.Readers.SearchedBooks
                    {
                        id = id,
                        SearchedBook = books
                    };

                    return View("SearchBookResult",model);
                }

            }
        }
        public ActionResult DisplayBooks(int id)
        {
            using (var db = new LIBRARYEntities())
            {
                var notBorrowed = new List<Book>();

                foreach (var book in db.Books)
                    if (book.IsBorrowed == false)
                        notBorrowed.Add(book);
                        
                if (notBorrowed.Count == 0)
                    ViewBag.Message = "No books to display";

                var account = db.Accounts.Where(x => x.AccountID ==id).Single();

                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();



                var model = new Models.Readers.SearchedBooks
                {
                    id = id,
                    SearchedBook = notBorrowed,
                    Count = reader.NrOfBooks()

                };

                return View(model);
            }
        }
        [HttpPost]
        public ActionResult DisplayBooks(int id, int code)
        {
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();

                var book = db.Books.Where(x => x.BookID == code).Single();

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

                var notBorrowed = new List<Book>();

                foreach (var item in db.Books)
                    if (item.IsBorrowed == false)
                        notBorrowed.Add(item);

                if (notBorrowed.Count == 0)
                    ViewBag.Message = "No books to display";

                var model = new Models.Readers.SearchedBooks
                {
                    id = id,
                    SearchedBook=notBorrowed,
                    Count=reader.NrOfBooks()

                };
                return View(model);

            }
        }
        public ActionResult BorrowBook(int id)
        {
            return View(id);
        }
        [HttpPost]
        public ActionResult BorrowBook(int id,int code)
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();

                if(reader.NrOfBooks()>=4)
                {
                    ViewBag.Message = "Can't borrow more books";
                    return View(id);
                }

                var books = db.Books.Where(x => x.IsBorrowed == false).ToList();

                if(books.Count==0)
                {
                    ViewBag.Message = "No books to borrow";
                    return View(id);
                }

                if(code<=0)
                {
                    ViewBag.Message = "Invalid input";
                    return View(id);
                }
                try
                {
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

                    return View(id);



                }
                catch(Exception)
                {
                    ViewBag.Message = "No book with the given code";
                    return View(id);
                }



            }
        }
        public ActionResult ReturnBook(int id)
        {
            return View(id);
        }
        [HttpPost]
        public ActionResult ReturnBook(int id,int code)
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();
                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();

                if(reader.NrOfBooks()==0)
                {
                    ViewBag.Message = "No book to return";
                    return View(id);
                }

                if(code<=0)
                {
                    ViewBag.Message = "Invalid input";
                    return View(id);
                }

                try
                {
                    var bbook = db.BorrowedBooks.Where(x => x.ReaderID == reader.ReaderID && x.ReturnDate == null && x.BookID == code).Single();

                    bbook.ReturnDate = DateTime.Now;

                    var book = db.Books.Where(x => x.BookID == code).Single();

                    book.IsBorrowed = false;

                    db.SaveChanges();

                    return View(id);


                }
                catch(Exception)
                {
                    ViewBag.Message = "No book with the given code";
                    return View(id);
                }
                
            }
        }
        public ActionResult BorrowedBooks(int id)
        {
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();

                var books = db.BorrowedBooks.Where(x => x.ReturnDate == null).Where(x => x.ReaderID == reader.ReaderID)
                                            .Join(db.Books,
                                                  bbook => bbook.BookID,
                                                  book => book.BookID,
                                                  (bbook, book) => new { BBook = bbook, Book = book }
                                           ).Select(y => new { y.Book.BookID, y.Book.BookName, y.BBook.ExpectDate })
                                           .ToList().OrderBy(x => x.BookID)
                                           .Select(x=>new Models.Readers.BBook {ID=x.BookID,Name=x.BookName,ExpectDate=x.ExpectDate }).ToList();
                var model = new Models.Readers.BBooks
                {
                    Id = id,
                    books = books
                };

                return View(model);


            }
        }

        [HttpPost]
        public ActionResult BorrowedBooks(int id ,int code)
        {
            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();

                var bbookReturned = db.BorrowedBooks.Where(x => x.ReaderID == reader.ReaderID && x.BookID == code && x.ReturnDate == null).Single();

                bbookReturned.ReturnDate = DateTime.Now;

                var bookReturned = db.Books.Where(x => x.BookID == code).Single();

                bookReturned.IsBorrowed = false;

                db.SaveChanges();

                var books = db.BorrowedBooks.Where(x => x.ReturnDate == null).Where(x => x.ReaderID == reader.ReaderID)
                                            .Join(db.Books,
                                                  bbook => bbook.BookID,
                                                  book => book.BookID,
                                                  (bbook, book) => new { BBook = bbook, Book = book }
                                           ).Select(y => new { y.Book.BookID, y.Book.BookName, y.BBook.ExpectDate })
                                           .ToList().OrderBy(x => x.BookID)
                                           .Select(x => new Models.Readers.BBook { ID = x.BookID, Name = x.BookName, ExpectDate = x.ExpectDate }).ToList();
                var model = new Models.Readers.BBooks
                {
                    Id = id,
                    books = books
                };

                return View(model);
            }
        }
        public ActionResult Top5(int id)
        {
            return View(id);
        }
        [HttpPost]
        public ActionResult Top5(int id ,int year)
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
                                              .Select(x => new Models.Top { BookName = x.BookName, BookAuthor = x.BookAuthor, Count = x.Count }).ToList();
                if(books.Count==0)
                {
                    ViewBag.Message = $"No book borrowed in the year {year}";
                    return View(id);
                }
                var model = new Models.Readers.TopModel
                {
                    Id = id,
                    list = books
                };
                return View("Top5Result", model);
            }
        }
        public ActionResult HistoryOfTheReader(int id)
        {
            ViewBag.Message = string.Empty;

            using (var db = new LIBRARYEntities())
            {
                var account = db.Accounts.Where(x => x.AccountID == id).Single();

                var reader = db.Readers.Where(x => x.ReaderID == account.ReaderID).Single();

                var history = db.BorrowedBooks.Where(x => x.ReaderID == reader.ReaderID)
                                                  .Join(db.Books,
                                                        bbook => bbook.BookID,
                                                        book => book.BookID,
                                                        (bbook, book) => new { BBook = bbook, Book = book })
                                                  .Select(y => new { y.BBook.BorrowedDate, y.BBook.ExpectDate, y.BBook.ReturnDate, y.Book.BookName })
                                                  .OrderBy(x => x.BorrowedDate).ToList()
                                                  .Select(x => new Models.ReaderHistory { ReaderName = reader.ReaderName, BookName = x.BookName, BorrowedDate = x.BorrowedDate, ExpectDate = x.ExpectDate, ReturnDate = x.ReturnDate }).ToList();
                if(history.Count==0)
                {
                    ViewBag.Message = "The reader never borrowed a book";
                    return View(id);
                }

                var model = new Models.Readers.ReaderModel
                {
                    Id = id,
                    list = history

                };
                return View(model);
            }
        }

    }
}