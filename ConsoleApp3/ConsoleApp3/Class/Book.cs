using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Book : IBook
    {
        #region Proprietes
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
        #endregion
        public Book()
        {

        }
        public Book(string name, string author, string type, int code, string genre)
        {
            Name = name;
            Author = author;
            BookCode = code;
            BookType = type;
            Genre = genre;
        }
        public override string ToString()
        {
            return $"{Name,-10} {Author,-10} {BookType,-10} {Genre,-10} {BookCode}";
        }

    }
}
