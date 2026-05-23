using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuvienCuaHieu.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(250)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [StringLength(150)]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [StringLength(50)]
        public string? ISBN { get; set; }

        public string? Description { get; set; }

        [StringLength(500)]
        public string? CoverImageUrl { get; set; }

        [StringLength(150)]
        public string? Publisher { get; set; }

        public int PublishedYear { get; set; }

        [Required]
        [Range(0, 1000)]
        public int TotalQty { get; set; }

        [Required]
        [Range(0, 1000)]
        public int AvailableQty { get; set; }

        public int ViewCount { get; set; }

        [StringLength(500)]
        public string? SourceUrl { get; set; }

        // Navigation properties
        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    }
}
