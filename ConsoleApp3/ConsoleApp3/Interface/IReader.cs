using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public interface IReader
    {

        string Address
        {
            get;
            set;
        }
        string Email
        {
            get;
            set;
        }
        long PhoneNumber
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        
        void Update(IBook a);
        void AddBook(IBook a);
        void ReturnBook(int code, ref IBook a);
        void DisplayBooks();
        int NrBooks();


    }
}
