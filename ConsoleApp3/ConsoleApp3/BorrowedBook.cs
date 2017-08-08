using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class BorrowedBook:IBook
    {

        public string Name
        {
            get;
            set;
        }
        public string BookType
        {
            get;
            set;
        }
        public string Author
        {
            get;
            set;
        }
        public int BookCode
        {
            get;
            set;
        }
        public string Genre
        {
            get;
            set;
        }
        private IReader _reader;

        public BorrowedBook()
        {
            _reader = new Reader();
        }
        public DateTime Date
        {
            get;
            set;
        }
        public BorrowedBook(string name,string author,string type,int code,string genre,IReader reader,DateTime date)
        {
            Name = name;
            Author = author;
            BookType = type;
            BookCode = code;
            Genre = genre;
            _reader = reader;
            Date = date;
        }
        public override string ToString()
        {
            return $"{Name,-10} {Author,-10} {BookType,-10} {Genre,-10} {BookCode,-5} borrowed by {_reader.Name}.Borrowed on {Date.ToString("dd MMM yyyy")} return untill {Date.AddMonths(1).ToString("dd MMM yyyy")}";
        }

    }
}
