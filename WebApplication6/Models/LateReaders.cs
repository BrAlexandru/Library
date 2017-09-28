using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class LateReaders
    {
        public string ReaderName { get; set; }
        public string BookName { get; set; }
        public DateTime ExpectDate { get; set; }
    }
}