using System.Collections.Generic;
using ThuvienCuaHieu.Models;

namespace ThuvienCuaHieu.ViewModels
{
    public class BookSearchVM
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<string> Authors { get; set; } = new List<string>();
        public List<int> PublishYears { get; set; } = new List<int>();

        // Search & Filter Parameters
        public string? SearchQuery { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string? SelectedAuthor { get; set; }
        public int? SelectedPublishYear { get; set; }
        public string? SelectedStatus { get; set; } // "All", "Available", "OutOfStock"
        public string SortOrder { get; set; } = "newest"; // "newest", "popular", "az", "za"

        // Pagination parameters
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int TotalItems { get; set; }
        public int TotalPages => (int)System.Math.Ceiling((double)TotalItems / PageSize);
    }
}
