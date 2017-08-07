using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public interface IBook
    {
        string Name
        {
            get;
            set;
        }
        string BookType
        {
            get;
            set;
        }
        string Author
        {
            get;
            set;
        }
        int BookCode
        {
            get;
            set;
        }
        string Genre
        {
            get;
            set;
        }
    }
}
