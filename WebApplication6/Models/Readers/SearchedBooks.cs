using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models.Readers
{
    public class SearchedBooks
    {

        public int id { get; set; }

        public List<Book> SearchedBook;

        public int Count { get; set; }
    }
}