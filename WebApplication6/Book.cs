//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication6
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.BorrowedBooks = new HashSet<BorrowedBook>();
        }
    
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookType { get; set; }
        public string BookGenre { get; set; }
        public Nullable<bool> IsBorrowed { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
