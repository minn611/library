using System.Collections.Generic;
using ThuvienCuaHieu.Models;

namespace ThuvienCuaHieu.ViewModels
{
    public class BookDetailVM
    {
        public Book Book { get; set; } = null!;
        public List<Book> RelatedBooks { get; set; } = new List<Book>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public double AverageRating { get; set; }
        public bool IsInWishlist { get; set; }
        public bool HasBorrowed { get; set; } // To let user review if they have interacted or just general users
        
        // Form properties for new review submission
        public int? RatingInput { get; set; }
        public string? CommentInput { get; set; }
    }
}
