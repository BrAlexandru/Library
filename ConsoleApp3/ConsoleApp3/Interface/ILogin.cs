using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public interface ILogin
    {


        #region AdminInstructions

        void AddBook();
        void AddReader();
        void RemoveBook();
        void RemoveReader();
        void AdminInstruction();
        void SearchReader();

        #endregion

        #region ReaderInstruction

        void ChangePass(IAccount a);
        void SearchBook();
        void BorrowBook(IAccount a);
        void ReturnBook(IAccount a);
        void ReaderInstruction(IAccount a);


        #endregion

        void Start();
    }
}
