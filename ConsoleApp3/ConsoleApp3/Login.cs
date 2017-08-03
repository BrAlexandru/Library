using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Login
    {

        private List<Account> _accounts = new List<Account>();
        private Librarian _librarian;
        private Library _library;

        public Login(Librarian librarian,Library liabrary)
        {
            _librarian = librarian;
            _library = liabrary;
        }
        public void AddAccount(Account acc)
        {
            _accounts.Add(acc);
        }
        public void AddAccount(string user,string pass)
        {
            _accounts.Add(new Account(user, pass));
        }
        #region AdminInstructions
        public void AddBook()
        {
            Console.Write("Book Name: ");
            string name = Console.ReadLine();
            Console.Write("Book Author: ");
            string author = Console.ReadLine();
            Console.Write("Book Type: ");
            string type = Console.ReadLine();
            Console.Write("Book Code: ");
            int code = int.Parse(Console.ReadLine());

            _library.AddBook(new Book(name, author, type,code));
        }
        public void AddReader()
        {
            Console.Write("Reader Name: ");
            string name = Console.ReadLine();
            Console.Write("Phone Number: ");
            long phone = long.Parse(Console.ReadLine());
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();

            _library.AddReader(new Reader(name, email, address, phone));

            _accounts.Add(new Account(name, name));

        }
        public void RemoveBook()
        {
            Console.Write("Code of the book: ");
            int i = int.Parse(Console.ReadLine());
            _library.RemoveBook(i);
        }
        public void RemoveReader()
        {
            Console.Write("Name of the reader: ");
            string reader = Console.ReadLine();
            _library.RemoveReader(reader);

        }
        public void AdminInstruction()
        {
            var myDictionary = new Dictionary<string, Action>();

            myDictionary.Add("D1", () => AddBook());
            myDictionary.Add("D2", () => AddReader());
            myDictionary.Add("D3", () => RemoveBook());
            myDictionary.Add("D4", () => RemoveReader());
            myDictionary.Add("D5", () => _library.DisplayBooks());
            myDictionary.Add("D6", () => _library.DisplayReaders());

            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("1.Add book to library ");
                Console.WriteLine("2.Add reader to library");
                Console.WriteLine("3.Remove book");
                Console.WriteLine("4.Remove reader");
                Console.WriteLine("5.List all the books");
                Console.WriteLine("6.List all the readers");

                key = Console.ReadKey();
                Console.Write("\n");

                Action value = null;
                if (myDictionary.TryGetValue(key.Key.ToString(), out value))
                {
                    value();
                }
                else Console.WriteLine("Command does not exist");

                Console.WriteLine();
            } while (key.Key != ConsoleKey.Escape);
        }


        #endregion

        #region ReaderInstruction

        public void ChangePass(Account a)
        {
            Console.Write("Current Password: ");
            if (Console.ReadLine() != a.Password)
                Console.WriteLine("Wrong Password");
            else
            {
                Console.Write("New Password: ");
                a.Password = Console.ReadLine();

                
            }

        }
        public void SearchBook()
        {
            Console.Write("Name of the book: ");
            string name=Console.ReadLine();
            Console.Write("Name of the Author: ");
            string author = Console.ReadLine();
            Console.Write("Type of the book: ");
            string type = Console.ReadLine();

            _library.Search(name, author, type);
        }
        public void BorrowBook(Account a)
        {
            Console.Write("Insert code of the book: ");
            int code = int.Parse(Console.ReadLine());
            _library.BorrowBook(a.Username, code);
        }
        public void ReturnBook(Account a)
        {
            Console.Write("Insert code of the book: ");
            int code = int.Parse(Console.ReadLine());
            _library.ReturnBook(a.Username, code);
        }
        public void ReaderInstruction(Account a)
        {
            var myDictionary = new Dictionary<string, Action>();

            myDictionary.Add("D1", () => ChangePass(a));
            myDictionary.Add("D2", () => SearchBook());
            myDictionary.Add("D3", () => BorrowBook(a));
            myDictionary.Add("D4", () => ReturnBook(a));

            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("1. Change Password");
                Console.WriteLine("2. Search Book");
                Console.WriteLine("3. Borrow Book");
                Console.WriteLine("4. Return Book");
                key = Console.ReadKey();
                Console.Write("\n");

                Action value = null;
                if (myDictionary.TryGetValue(key.Key.ToString(), out value))
                {
                    value();
                }
                else Console.WriteLine("Command does not exist");

                Console.WriteLine();
            } while (key.Key != ConsoleKey.Escape);
        }

        #endregion

        public void Start()
        {
            ConsoleKeyInfo g;
            do
            {
                Console.WriteLine("Press Escape to stop");
                g = Console.ReadKey();

                if (g.Key == ConsoleKey.Escape)
                    break;

                Console.Write("Username: ");
                string user = Console.ReadLine();
                Console.Write("Password: ");
                string pass = Console.ReadLine();

                if ((user == _librarian.AdminID) && (pass == _librarian.Password))
                {
                    AdminInstruction();
                }
                else
                {
                    bool ok = false;
                    foreach (var i in _accounts)
                        if ((user == i.Username) && (pass == i.Password))
                        {
                            ReaderInstruction(i);
                            ok = true;
                        }
                    if (ok == false)
                        Console.WriteLine("Wrong Username or Password");
                }

            } while (g.Key != ConsoleKey.Escape);
        }


    }

}

