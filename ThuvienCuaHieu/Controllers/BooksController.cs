using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuvienCuaHieu.Data;
using ThuvienCuaHieu.Models;
using ThuvienCuaHieu.ViewModels;

namespace ThuvienCuaHieu.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Books
        public IActionResult Index(string? searchQuery, int? categoryId, string? author, int? publishYear, string? status, string sortOrder = "newest", int pageIndex = 1)
        {
            var query = _context.Books.Include(b => b.Category).AsQueryable();

            // 1. Search Query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(b => b.Title.ToLower().Contains(searchQuery.ToLower()) || 
                                     b.Author.ToLower().Contains(searchQuery.ToLower()) || 
                                     b.ISBN.Contains(searchQuery));
            }

            // 2. Category Filter
            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            // 3. Author Filter
            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(b => b.Author == author);
            }

            // 4. Publish Year Filter
            if (publishYear.HasValue)
            {
                query = query.Where(b => b.PublishedYear == publishYear.Value);
            }

            // 5. Status Filter
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Equals("Available", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(b => b.AvailableQty > 0);
                }
                else if (status.Equals("OutOfStock", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(b => b.AvailableQty == 0);
                }
            }

            // 6. Sorting
            query = sortOrder.ToLower() switch
            {
                "popular" => query.OrderByDescending(b => b.ViewCount),
                "az" => query.OrderBy(b => b.Title),
                "za" => query.OrderByDescending(b => b.Title),
                _ => query.OrderByDescending(b => b.BookId) // Default: newest
            };

            // Retrieve filter options
            var categories = _context.Categories.Include(c => c.Books).ToList();
            var distinctAuthors = _context.Books.Select(b => b.Author).Distinct().OrderBy(a => a).ToList();
            var distinctYears = _context.Books.Select(b => b.PublishedYear).Distinct().OrderByDescending(y => y).ToList();

            // Pagination
            int pageSize = 8;
            int totalItems = query.Count();
            var books = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new BookSearchVM
            {
                Books = books,
                Categories = categories,
                Authors = distinctAuthors,
                PublishYears = distinctYears,
                SearchQuery = searchQuery,
                SelectedCategoryId = categoryId,
                SelectedAuthor = author,
                SelectedPublishYear = publishYear,
                SelectedStatus = status,
                SortOrder = sortOrder,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        // GET: /Books/Details/{id}
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Category)
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefault(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            // Increment ViewCount
            book.ViewCount++;
            _context.SaveChanges();

            // Load related books in same category (excluding current book)
            var relatedBooks = _context.Books
                .Where(b => b.CategoryId == book.CategoryId && b.BookId != book.BookId)
                .Take(4)
                .ToList();

            // Calculate average rating
            double averageRating = 0;
            if (book.Reviews.Any())
            {
                averageRating = book.Reviews.Average(r => r.Rating);
            }

            // Check if current user has it in wishlist
            bool isInWishlist = false;
            bool hasBorrowed = false;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    isInWishlist = _context.Wishlists.Any(w => w.UserId == userId && w.BookId == book.BookId);
                    hasBorrowed = _context.BorrowRecords.Any(br => br.UserId == userId && br.BookId == book.BookId && br.Status == "Returned");
                }
            }

            var viewModel = new BookDetailVM
            {
                Book = book,
                RelatedBooks = relatedBooks,
                Reviews = book.Reviews.OrderByDescending(r => r.CreatedAt).ToList(),
                AverageRating = Math.Round(averageRating, 1),
                IsInWishlist = isInWishlist,
                HasBorrowed = hasBorrowed
            };

            return View(viewModel);
        }

        // POST: /Books/AddReview/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(int id, int rating, string comment)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Challenge();
            }

            var bookExists = _context.Books.Any(b => b.BookId == id);
            if (!bookExists)
            {
                return NotFound();
            }

            // Simple validation
            if (rating < 1 || rating > 5)
            {
                TempData["ErrorMessage"] = "Đánh giá sao phải từ 1 đến 5.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            var review = new Review
            {
                BookId = id,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cảm ơn bạn đã gửi đánh giá sách!";
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
