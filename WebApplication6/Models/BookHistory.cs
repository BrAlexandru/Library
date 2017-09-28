using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class BookHistory
    {

        public string BookName  { get; set; }

        public string ReaderName { get; set; }

        public DateTime BorrowedDate { get; set; }

        public DateTime ExpectDate { get; set; }
        public Nullable<DateTime> ReturnDate { get; set; }



    }
}