using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Book
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
        #endregion
        public Book()
        {

        }
        public Book(string name,string author,string type, int code)
        {
            Name = name;
            Author = author;
            BookCode = code;
            BookType = type;
        }
        public override string ToString()
        {
            return $"{Name} {Author} {BookType} {BookCode}";
        }
    }
}
