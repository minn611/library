using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuvienCuaHieu.Data;
using ThuvienCuaHieu.Models;

namespace ThuvienCuaHieu.Controllers
{
    [Authorize]
    public class BorrowController : Controller
    {
        private readonly AppDbContext _context;

        public BorrowController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /Borrow/RequestBorrow/{bookId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestBorrow(int bookId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Challenge();
            }

            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                return NotFound();
            }

            // Check if book has available copies
            if (book.AvailableQty <= 0)
            {
                TempData["ErrorMessage"] = "Sách hiện tại đã hết bản cứng trong thư viện. Bạn có thể thêm vào yêu thích để theo dõi!";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            // Check if user already has a pending or active borrow record for the SAME book
            var existingRecord = _context.BorrowRecords
                .FirstOrDefault(br => br.UserId == userId && br.BookId == bookId && (br.Status == "Pending" || br.Status == "Approved"));

            if (existingRecord != null)
            {
                if (existingRecord.Status == "Pending")
                {
                    TempData["ErrorMessage"] = "Bạn đã gửi yêu cầu mượn cuốn sách này trước đó. Vui lòng chờ Admin duyệt!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Bạn đang mượn cuốn sách này rồi. Hãy hoàn thành việc đọc và trả sách trước nhé!";
                }
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            // Create BorrowRecord
            var record = new BorrowRecord
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14), // Standard 14 days borrow
                Status = "Pending",
                FineAmount = 0
            };

            _context.BorrowRecords.Add(record);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Yêu cầu mượn sách đã được gửi thành công! Hãy đến quầy thư viện để nhận sách sau khi được phê duyệt.";
            return RedirectToAction("Profile", "Account");
        }

        // POST: /Borrow/ToggleWishlist/{bookId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleWishlist(int bookId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Challenge();
            }

            var wishlistRecord = _context.Wishlists.FirstOrDefault(w => w.UserId == userId && w.BookId == bookId);

            if (wishlistRecord == null)
            {
                // Add to wishlist
                var newWish = new Wishlist
                {
                    UserId = userId,
                    BookId = bookId,
                    AddedAt = DateTime.UtcNow
                };
                _context.Wishlists.Add(newWish);
                TempData["SuccessMessage"] = "Đã thêm cuốn sách này vào danh sách yêu thích ❤️";
            }
            else
            {
                // Remove from wishlist
                _context.Wishlists.Remove(wishlistRecord);
                TempData["SuccessMessage"] = "Đã xóa cuốn sách khỏi danh sách yêu thích.";
            }

            _context.SaveChanges();

            return RedirectToAction("Details", "Books", new { id = bookId });
        }
    }
}
