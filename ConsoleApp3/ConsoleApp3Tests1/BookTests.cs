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
    public class BookTests
    {

        [TestMethod()]
        public void ToStringTest()
        {
            //Arrange
            Book book = new Book("Book", "Author", "Type", 1);
            string expected = "Book Author Type 1";

            //Act
            string actual = book.ToString();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}