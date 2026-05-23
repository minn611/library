using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuvienCuaHieu.Models
{
    public class BorrowRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);

        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Returned", "Rejected"

        public decimal FineAmount { get; set; } = 0;
    }
}
