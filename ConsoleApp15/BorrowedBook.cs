//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApp15
{
    using System;
    using System.Collections.Generic;
    
    public partial class BorrowedBook
    {
        public int BorrowedBookID { get; set; }
        public int BookID { get; set; }
        public int ReaderID { get; set; }
        public System.DateTime BorrowedDate { get; set; }
        public System.DateTime ExpectDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }

        

        public virtual Book Book { get; set; }
        public virtual Reader Reader { get; set; }
    }
}
