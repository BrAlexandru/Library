using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public interface ILibrary
    {

        string Name
        {
            get;
            set;
        }
        ILibrarian Admin
        {
            get;
            set;
        }
        void AddReader(IReader reader);
        void AddBook(IBook book);
        void RemoveReader(string name);
        void RemoveBook(int code);
        void Notify(IBook a);
        void DisplayBooks();
        void DisplayReaders();
        void Search(string name, string author, string type);
        void BorrowBook(string user, int code);
        void ReturnBook(string user, int code);
        void MaxBooks();


    }
}
