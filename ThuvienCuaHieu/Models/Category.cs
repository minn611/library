using System.ComponentModel.DataAnnotations;

namespace ThuvienCuaHieu.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên thể loại không được để trống")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? IconUrl { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
