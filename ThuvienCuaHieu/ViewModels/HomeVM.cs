using System.Collections.Generic;
using ThuvienCuaHieu.Models;

namespace ThuvienCuaHieu.ViewModels
{
    public class HomeVM
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Book> NewestBooks { get; set; } = new List<Book>();
        public List<Book> TopBorrowedBooks { get; set; } = new List<Book>();
        public string RandomQuote { get; set; } = string.Empty;
        public string RandomAuthor { get; set; } = string.Empty;
        
        // Statistics
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int TotalActiveBorrows { get; set; }
    }
}
