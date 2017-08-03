using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.Tests
{
    [TestClass()]
    public class LibraryTests
    {
        [TestMethod()]
        public void InstanceTest()
        {
            
        }

        [TestMethod()]
        public void AddReaderTest()
        {
            //Arrange
            Librarian librarian = Librarian.Instance("name1", "admin", "pass");
            Library library = Library.Instance("name", librarian);
            Reader a = new Reader("nume", email, address, long phone);
            int expected
        }

        [TestMethod()]
        public void AddBookTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveReaderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveBookTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NotifyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DisplayBooksTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DisplayReadersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchTest()
        {
            Assert.Fail();
        }
    }
}