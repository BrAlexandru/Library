﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Login : ILogin
    {

        private List<IAccount> _accounts = new List<IAccount>();
        private ILibrarian _librarian;
        private ILibrary _library;

        public Login(ILibrarian librarian, ILibrary library)
        {
            _librarian = librarian;
            _library = library;
        }
        public void AddAccount(IAccount acc)
        {
            try
            {
                _accounts.Add(acc);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AddAccount(string user, string pass)
        {
            try
            {
                _accounts.Add(new Account(user, pass));
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #region AdminInstructions
        public void AddBook()
        {
            try
            {

                Console.Write("Book Name: ");
                string name = Console.ReadLine();
                if (name == "")
                    throw new FormatException();
                Console.Write("Book Author: ");
                string author = Console.ReadLine();
                if (author == "")
                    throw new FormatException();
                Console.Write("Book Type: ");
                string type = Console.ReadLine();
                if (type == "")
                    throw new FormatException();
                Console.Write("Book Code: ");
                int code = int.Parse(Console.ReadLine());
                Console.Write("Book Genre: ");
                string genre = Console.ReadLine();
                if (genre == "")
                    throw new FormatException();

                _library.AddBook(new Book(name, author, type, code, genre));
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input");
            }


        }
        public void AddReader()
        {
            try
            {


                Console.Write("Reader Name: ");
                string name = Console.ReadLine();
                if (name == "")
                    throw new FormatException();
                Console.Write("Phone Number: ");
                long phone = long.Parse(Console.ReadLine());
                Console.Write("Email: ");
                string email = Console.ReadLine();
                if (email == "")
                    throw new FormatException();
                Console.Write("Address: ");
                string address = Console.ReadLine();
                if (address == "")
                    throw new FormatException();

                _library.AddReader(new Reader(name, email, address, phone));

                _accounts.Add(new Account(name, name));
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input");
            }

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
            myDictionary.Add("D7", () => _library.MaxBooks());

            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("1.Add book to library.");
                Console.WriteLine("2.Add reader to library.");
                Console.WriteLine("3.Remove book.");
                Console.WriteLine("4.Remove reader.");
                Console.WriteLine("5.List all the books.");
                Console.WriteLine("6.List all the readers.");
                Console.WriteLine("7.The reader with the most borrowed books.");


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

        public void ChangePass(IAccount a)
        {
            Console.Write("Current Password: ");
            if (Console.ReadLine() != a.Password)
                Console.WriteLine("Wrong Password");
            else
            {
                Console.Write("New Password: ");
                string pass = Console.ReadLine();
                if (pass == "")
                {
                    Console.WriteLine("Invalid password");
                    return;
                }
                a.Password = pass;



            }

        }
        public void SearchBook()
        {
            Console.Write("Name of the book: ");
            string name = Console.ReadLine();
            Console.Write("Name of the Author: ");
            string author = Console.ReadLine();
            Console.Write("Type of the book: ");
            string type = Console.ReadLine();

            _library.Search(name, author, type);
        }
        public void BorrowBook(IAccount a)
        {
            Console.Write("Insert code of the book: ");
            int code = int.Parse(Console.ReadLine());
            _library.BorrowBook(a.Username, code);
        }
        public void ReturnBook(IAccount a)
        {

            Console.Write("Insert code of the book: ");
            int code = int.Parse(Console.ReadLine());
            _library.ReturnBook(a.Username, code);
        }
        public void ReaderInstruction(IAccount a)
        {
            var myDictionary = new Dictionary<string, Action>();

            myDictionary.Add("D1", () => ChangePass(a));
            myDictionary.Add("D2", () => SearchBook());
            myDictionary.Add("D3", () => BorrowBook(a));
            myDictionary.Add("D4", () => ReturnBook(a));

            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("1. Change Password.");
                Console.WriteLine("2. Search Book.");
                Console.WriteLine("3. Borrow Book.");
                Console.WriteLine("4. Return Book.");
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
                try
                {
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
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (g.Key != ConsoleKey.Escape);
        }


    }

}
