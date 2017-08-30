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
    
    public partial class Reader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reader()
        {
            this.Accounts = new HashSet<Account>();
            this.BorrowedBooks = new HashSet<BorrowedBook>();
        }
    
        public int ReaderID { get; set; }
        public string ReaderName { get; set; }
        public Nullable<long> ReaderPhone { get; set; }
        public string ReaderEmail { get; set; }
        public string ReaderAddress { get; set; }

        public int NrOfBooks()
        {
            using (var db = new LIBRARYEntities())
            {
                int i = 0;
                foreach (var book in db.BorrowedBooks)
                    if (book.ReaderID == this.ReaderID && book.ReturnDate == null)
                        i++;
                return i;
            }
        }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
