using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuvienCuaHieu.Data;
using ThuvienCuaHieu.Models;
using ThuvienCuaHieu.ViewModels;

namespace ThuvienCuaHieu.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // 1. Load categories
        var categories = _context.Categories.Include(c => c.Books).ToList();

        // 2. Load newest books (take 4)
        var newestBooks = _context.Books
            .Include(b => b.Category)
            .OrderByDescending(b => b.BookId)
            .Take(4)
            .ToList();

        // 3. Load top borrowed books (take 4 based on view counts or borrow counts)
        var topBorrowedBooks = _context.Books
            .Include(b => b.Category)
            .OrderByDescending(b => b.ViewCount)
            .Take(4)
            .ToList();

        // 4. Set up library statistics
        int totalBooks = _context.Books.Sum(b => b.TotalQty);
        int totalMembers = _context.Users.Count(u => u.Role == "User");
        int totalActiveBorrows = _context.BorrowRecords.Count(br => br.Status == "Approved");

        // 5. Select a random quote
        var quotes = new[]
        {
            ("Không có gì có thể thay thế được văn hóa đọc.", "Albert Einstein"),
            ("Một cuốn sách hay dạy tôi nhiều điều hơn là chỉ đọc nó. Tôi phải nhanh chóng đặt nó xuống và bắt đầu sống theo những gì nó chỉ dẫn.", "Henry David Thoreau"),
            ("Sách là nguồn tri thức vô tận của nhân loại, là chiếc chìa khóa mở ra cánh cửa thành công.", "Socrates"),
            ("Việc đọc tất cả các cuốn sách hay cũng giống như trò chuyện với những bộ óc tuyệt vời nhất của những thế kỷ đã qua.", "René Descartes"),
            ("Đọc sách có hai tác dụng: Một là rèn luyện trí tuệ, hai là mở mang tâm hồn.", "Khuyết Danh")
        };
        var random = new Random();
        var selectedQuote = quotes[random.Next(quotes.Length)];

        var viewModel = new HomeVM
        {
            Categories = categories,
            NewestBooks = newestBooks,
            TopBorrowedBooks = topBorrowedBooks,
            RandomQuote = selectedQuote.Item1,
            RandomAuthor = selectedQuote.Item2,
            TotalBooks = totalBooks > 0 ? totalBooks : 1200, // Seed backup if empty
            TotalMembers = totalMembers > 0 ? totalMembers : 350,
            TotalActiveBorrows = totalActiveBorrows > 0 ? totalActiveBorrows : 89
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

