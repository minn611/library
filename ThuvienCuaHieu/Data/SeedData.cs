using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ThuvienCuaHieu.Models;

namespace ThuvienCuaHieu.Data
{
    public static class SeedData
    {
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashedBytes).ToLower();
            }
        }

        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // 1. Seed Users if none exist
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        FullName = "Trần Trung Hiếu (Admin)",
                        Email = "admin@thuvien.com",
                        PasswordHash = HashPassword("Admin@123"),
                        Role = "Admin",
                        Phone = "0987654321",
                        Avatar = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&q=80&w=200",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        FullName = "Nguyễn Văn Độc Giả",
                        Email = "user@thuvien.com",
                        PasswordHash = HashPassword("User@123"),
                        Role = "User",
                        Phone = "0123456789",
                        Avatar = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&q=80&w=200",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
            }

            // 2. Seed Categories if none exist
            if (!context.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category { Name = "Tâm lý - Kỹ năng sống", IconUrl = "fa-brain", Description = "Sách rèn luyện kỹ năng và thấu hiểu nội tâm" },
                    new Category { Name = "Lịch sử - Quân sự",     IconUrl = "fa-landmark", Description = "Lược sử loài người, lịch sử dân tộc và danh tướng" },
                    new Category { Name = "Kinh doanh - Quản trị", IconUrl = "fa-chart-line", Description = "Bí quyết kinh doanh, làm giàu và quản trị doanh nghiệp" },
                    new Category { Name = "Văn học - Tiểu thuyết", IconUrl = "fa-book-open", Description = "Các tác phẩm văn học, truyện dài và tiểu thuyết nổi tiếng" },
                    new Category { Name = "Khoa học - Công nghệ",  IconUrl = "fa-flask", Description = "Khám phá khoa học, công nghệ hiện đại và vũ trụ" },
                    new Category { Name = "Phát triển bản thân",   IconUrl = "fa-seedling", Description = "Tự hoàn thiện, thói quen tốt và phát triển tư duy" },
                    new Category { Name = "Thiếu nhi - Giáo dục",  IconUrl = "fa-child", Description = "Sách truyện dành cho trẻ em và các phương pháp giáo dục" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // 3. Seed Books if none exist
            if (!context.Books.Any())
            {
                // Retrieve seeded categories to link correct CategoryId
                var catMental = context.Categories.First(c => c.Name == "Tâm lý - Kỹ năng sống");
                var catHistory = context.Categories.First(c => c.Name == "Lịch sử - Quân sự");
                var catBusiness = context.Categories.First(c => c.Name == "Kinh doanh - Quản trị");
                var catLiterature = context.Categories.First(c => c.Name == "Văn học - Tiểu thuyết");
                var catScience = context.Categories.First(c => c.Name == "Khoa học - Công nghệ");
                var catDev = context.Categories.First(c => c.Name == "Phát triển bản thân");
                var catKids = context.Categories.First(c => c.Name == "Thiếu nhi - Giáo dục");

                var books = new Book[]
                {
                    // === Tâm lý - Kỹ năng sống ===
                    new Book {
                        Title = "Đắc Nhân Tâm", Author = "Dale Carnegie", ISBN = "9780671723651",
                        Description = "Cuốn sách nghệ thuật ứng xử, giao tiếp hàng đầu mọi thời đại, giúp kết nối con người và gặt hái thành công vượt bậc.",
                        CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780671723651-M.jpg",
                        SourceUrl = "https://dilib.vn/dac-nhan-tam-403.html",
                        Publisher = "NXB Tổng hợp TP.HCM", PublishedYear = 1936,
                        TotalQty = 5, AvailableQty = 5, ViewCount = 120, CategoryId = catMental.CategoryId
                    },
                    new Book {
                        Title = "Tuổi Trẻ Đáng Giá Bao Nhiêu", Author = "Rosie Nguyễn", ISBN = "9786045332657",
                        Description = "Tác phẩm truyền cảm hứng mạnh mẽ cho giới trẻ Việt Nam về việc định vị bản thân, học hỏi, đi và cống hiến hết mình.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/tuoi-tre-dang-gia-bao-nhieu.jpg",
                        SourceUrl = "https://dilib.vn/thu-vien/tam-ly-ky-nang/",
                        Publisher = "NXB Hội Nhà Văn", PublishedYear = 2016,
                        TotalQty = 6, AvailableQty = 6, ViewCount = 210, CategoryId = catMental.CategoryId
                    },
                    new Book {
                        Title = "Dám Bị Ghét", Author = "Ichiro Kishimi", ISBN = "9786045636069",
                        Description = "Trình bày về thuyết tâm lý học cá nhân của Adler, giúp độc giả tự cởi trói bản thân khỏi quá khứ và định kiến xã hội để hạnh phúc.",
                        CoverImageUrl = "https://covers.openlibrary.org/b/id/10527866-M.jpg",
                        SourceUrl = "https://dilib.vn/thu-vien/tam-ly-ky-nang/",
                        Publisher = "NXB Lao Động", PublishedYear = 2013,
                        TotalQty = 4, AvailableQty = 4, ViewCount = 145, CategoryId = catMental.CategoryId
                    },

                    // === Lịch sử - Quân sự ===
                    new Book {
                        Title = "Sapiens - Lược Sử Loài Người", Author = "Yuval Noah Harari", ISBN = "9780062316097",
                        Description = "Hành trình tiến hóa đầy kinh ngạc của loài người từ thời đồ đá cũ cho đến cuộc cách mạng công nghệ sinh học ngày nay.",
                        CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780062316097-M.jpg",
                        SourceUrl = "https://dilib.vn/sapiens-luoc-su-loai-nguoi-1695.html",
                        Publisher = "NXB Tri Thức", PublishedYear = 2011,
                        TotalQty = 4, AvailableQty = 4, ViewCount = 310, CategoryId = catHistory.CategoryId
                    },
                    new Book {
                        Title = "Danh Tướng Việt Nam", Author = "Nguyễn Khắc Thuần", ISBN = "9786046808794",
                        Description = "Giới thiệu những vị anh hùng và danh tướng lỗi lạc trong lịch sử kháng chiến giữ nước của dân tộc Việt Nam qua các triều đại.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/danh-tuong-viet-nam-7203.jpg",
                        SourceUrl = "https://dilib.vn/danh-tuong-viet-nam-7203.html",
                        Publisher = "NXB Giáo Dục", PublishedYear = 2005,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 85, CategoryId = catHistory.CategoryId
                    },
                    new Book {
                        Title = "Lão Tử Đạo Đức Kinh", Author = "Nguyễn Duy Cần", ISBN = "9786049028711",
                        Description = "Bản dịch và bình chú sâu sắc của cụ Thu Giang Nguyễn Duy Cần về kiệt tác triết học phương Đông Đạo Đức Kinh của Lão Tử.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/lao-tu-dao-duc-kinh-291.jpg",
                        SourceUrl = "https://dilib.vn/lao-tu-dao-duc-kinh-291.html",
                        Publisher = "NXB Trẻ", PublishedYear = 1960,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 105, CategoryId = catHistory.CategoryId
                    },
                    new Book {
                        Title = "Cổ Học Tinh Hoa", Author = "Nguyễn Văn Ngọc", ISBN = "9786049028681",
                        Description = "Những câu chuyện ngụ ngôn giàu ý nghĩa sâu sắc, giúp tu dưỡng nhân cách đạo đức đúc kết từ tinh hoa cổ học phương Đông.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/co-hoc-tinh-hoa-1056.jpg",
                        SourceUrl = "https://dilib.vn/co-hoc-tinh-hoa-1056.html",
                        Publisher = "NXB Văn Học", PublishedYear = 1928,
                        TotalQty = 4, AvailableQty = 4, ViewCount = 98, CategoryId = catHistory.CategoryId
                    },

                    // === Kinh doanh - Quản trị ===
                    new Book {
                        Title = "Tiền Đẻ Ra Tiền", Author = "Duncan Bannatyne", ISBN = "9786049028889",
                        Description = "Hướng dẫn thực tế, dễ hiểu của doanh nhân nổi tiếng Duncan Bannatyne về cách quản lý tài chính và đầu tư khôn ngoan.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/tien-de-ra-tien-7900.jpg",
                        SourceUrl = "https://dilib.vn/tien-de-ra-tien-7900.html",
                        Publisher = "NXB Lao Động - Xã Hội", PublishedYear = 2008,
                        TotalQty = 5, AvailableQty = 5, ViewCount = 160, CategoryId = catBusiness.CategoryId
                    },
                    new Book {
                        Title = "10 Bài Học Trên Chiếc Khăn Ăn", Author = "Don Failla", ISBN = "9786049028902",
                        Description = "Kinh điển về tiếp thị liên kết và xây dựng mạng lưới kinh doanh bền vững, giúp bạn thấu hiểu bản chất ngành kinh doanh.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/10-bai-hoc-tren-chiec-khan-an-15765.jpg",
                        SourceUrl = "https://dilib.vn/10-bai-hoc-tren-chiec-khan-an-15765.html",
                        Publisher = "NXB Trẻ", PublishedYear = 1984,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 110, CategoryId = catBusiness.CategoryId
                    },
                    new Book {
                        Title = "10 Bí Quyết Diễn Giả Tài Năng", Author = "Carmine Gallo", ISBN = "9786049028926",
                        Description = "Bật mí những bí quyết của các diễn giả hàng đầu thế giới như Steve Jobs để bạn làm chủ sân khấu và thuyết phục người nghe hoàn hảo.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/10-bi-quyet-thanh-cong-dien-gia.jpg",
                        SourceUrl = "https://dilib.vn/10-bi-quyet-thanh-cong-cua-nhung-dien-gia-mc-tai-nang-nhat-the-gioi-7924.html",
                        Publisher = "NXB Tổng hợp TP.HCM", PublishedYear = 2014,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 74, CategoryId = catBusiness.CategoryId
                    },

                    // === Văn học - Tiểu thuyết ===
                    new Book {
                        Title = "Cây Cam Ngọt Của Tôi", Author = "José Mauro de Vasconcelos", ISBN = "9786043567892",
                        Description = "Câu chuyện đẫm nước mắt về cậu bé Zezé thông minh, giàu trí tưởng tượng cùng cuộc đời nghèo khó nhưng lấp lánh sự tử tế và tình thương yêu.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/cay-cam-ngot-cua-toi-7903.jpg",
                        SourceUrl = "https://dilib.vn/cay-cam-ngot-cua-toi-7903.html",
                        Publisher = "NXB Hội Nhà Văn", PublishedYear = 1968,
                        TotalQty = 3, AvailableQty = 2, ViewCount = 385, CategoryId = catLiterature.CategoryId
                    },
                    new Book {
                        Title = "Nhà Giả Kim", Author = "Paulo Coelho", ISBN = "9780062315007",
                        Description = "Cuốn sách cổ tích hiện đại đầy triết lý sâu xa về hành trình theo đuổi vận mệnh của cậu bé chăn cừu Santiago.",
                        CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780062315007-M.jpg",
                        SourceUrl = "https://dilib.vn/thu-vien/van-hoc-nghe-thuat/",
                        Publisher = "NXB Hội Nhà Văn", PublishedYear = 1988,
                        TotalQty = 4, AvailableQty = 4, ViewCount = 290, CategoryId = catLiterature.CategoryId
                    },
                    new Book {
                        Title = "Khu Vườn Bí Mật", Author = "Frances H. Burnett", ISBN = "9786049028957",
                        Description = "Một tác phẩm thiếu nhi kinh điển kỳ diệu về tình bạn và sức sống thiên nhiên chữa lành tâm hồn hai đứa trẻ cô độc.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/khu-vuon-bi-mat-10335.jpg",
                        SourceUrl = "https://dilib.vn/khu-vuon-bi-mat-10335.html",
                        Publisher = "NXB Kim Đồng", PublishedYear = 1911,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 115, CategoryId = catLiterature.CategoryId
                    },
                    new Book {
                        Title = "Cuộc Đời Của Pi", Author = "Yann Martel", ISBN = "9786049028971",
                        Description = "Hành trình sinh tồn kỳ diệu ngoài đại dương của cậu bé Pi Patel trên chiếc xuồng cứu sinh cùng một con hổ Bengal dữ tợn.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/cuoc-doi-cua-pi-14045.jpg",
                        SourceUrl = "https://dilib.vn/cuoc-doi-cua-pi-14045.html",
                        Publisher = "NXB Hội Nhà Văn", PublishedYear = 2001,
                        TotalQty = 4, AvailableQty = 4, ViewCount = 135, CategoryId = catLiterature.CategoryId
                    },
                    new Book {
                        Title = "1 Cm Ánh Dương", Author = "Mặc Bảo Phi Bảo", ISBN = "9786049028995",
                        Description = "Cuộc tình sâu sắc vượt thời gian đầy gian nan nhưng vô cùng ấm áp giữa chàng phóng viên chiến trường Kỷ Ninh Dương và cô bạn nhỏ.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/1-cm-anh-duong-7909.jpg",
                        SourceUrl = "https://dilib.vn/1-cm-anh-duong-7909.html",
                        Publisher = "NXB Văn Học", PublishedYear = 2015,
                        TotalQty = 2, AvailableQty = 2, ViewCount = 152, CategoryId = catLiterature.CategoryId
                    },
                    new Book {
                        Title = "Đông Chu Liệt Quốc", Author = "Phùng Mộng Long", ISBN = "9786049029015",
                        Description = "Bộ tiểu thuyết dã sử đồ sộ tái hiện lại thời kỳ Xuân Thu Chiến Quốc huy hoàng đầy biến động và mưu chước mưu sinh.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/dong-chu-liet-quoc-9511.jpg",
                        SourceUrl = "https://dilib.vn/dong-chu-liet-quoc-9511.html",
                        Publisher = "NXB Văn Học", PublishedYear = 1627,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 68, CategoryId = catLiterature.CategoryId
                    },

                    // === Khoa học - Phát triển bản thân ===
                    new Book {
                        Title = "Mẹ Lưu Manh, Con Thiên Tài", Author = "Quỷ Miêu Tử", ISBN = "9786049029053",
                        Description = "Câu chuyện hài hước, dí dỏm kể về cuộc đấu trí không khoan nhượng giữa người mẹ tinh quái và cậu con trai thiên tài siêu quậy.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/me-luu-manh-con-thien-tai-10767.jpg",
                        SourceUrl = "https://dilib.vn/me-luu-manh-con-thien-tai-10767.html",
                        Publisher = "NXB Phụ Nữ", PublishedYear = 2010,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 189, CategoryId = catScience.CategoryId
                    },
                    new Book {
                        Title = "Khoa Học Tâm Linh", Author = "Hoàng Nhật Minh", ISBN = "9786049029077",
                        Description = "Khám phá khoa học dưới lăng kính tâm linh, hành trình tự tìm lại chính mình và kết nối sâu sắc với vũ trụ bao la.",
                        CoverImageUrl = "https://dilib.vn/img/news/thumb/khoa-hoc-tam-linh-hanh-trinh-tim-lai-chinh-minh-5504.jpg",
                        SourceUrl = "https://dilib.vn/khoa-hoc-tam-linh-hanh-trinh-tim-lai-chinh-minh-5504.html",
                        Publisher = "NXB Hồng Đức", PublishedYear = 2020,
                        TotalQty = 2, AvailableQty = 2, ViewCount = 92, CategoryId = catScience.CategoryId
                    },
                    new Book {
                        Title = "Chúa Tể Những Chiếc Nhẫn", Author = "J.R.R. Tolkien", ISBN = "9780007525546",
                        Description = "Kiệt tác sử thi kỳ ảo vĩ đại nhất mọi thời đại về cuộc chiến tiêu hủy chiếc Nhẫn Chúa cứu rỗi vùng Trung Địa.",
                        CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780007525546-M.jpg",
                        SourceUrl = "https://dilib.vn/chua-te-nhung-chiec-nhan-doan-ho-nhan-quyen-1-9451.html",
                        Publisher = "NXB Kim Đồng", PublishedYear = 1954,
                        TotalQty = 3, AvailableQty = 3, ViewCount = 220, CategoryId = catScience.CategoryId
                    },
                    new Book {
                        Title = "Atomic Habits", Author = "James Clear", ISBN = "9780735211292",
                        Description = "Phương pháp cực kỳ khoa học và thực tế để thay đổi 1% mỗi ngày giúp bạn hình thành thói quen tốt và loại bỏ thói quen xấu vĩnh viễn.",
                        CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780735211292-M.jpg",
                        SourceUrl = "https://dilib.vn/thu-vien/phat-trien-ban-than/",
                        Publisher = "NXB Thế Giới", PublishedYear = 2018,
                        TotalQty = 4, AvailableQty = 4, ViewCount = 240, CategoryId = catDev.CategoryId
                    }
                };

                context.Books.AddRange(books);
                context.SaveChanges();
            }

            // 4. Seed Reviews & BorrowRecords if none exist to make the admin dashboard highly active immediately
            if (!context.BorrowRecords.Any() && context.Users.Any() && context.Books.Any())
            {
                var reader = context.Users.First(u => u.Role == "User");
                var decNhanTam = context.Books.First(b => b.Title == "Đắc Nhân Tâm");
                var camNgot = context.Books.First(b => b.Title == "Cây Cam Ngọt Của Tôi");
                var sapiens = context.Books.First(b => b.Title == "Sapiens - Lược Sử Loài Người");

                context.BorrowRecords.AddRange(
                    new BorrowRecord
                    {
                        UserId = reader.UserId,
                        BookId = decNhanTam.BookId,
                        BorrowDate = DateTime.Now.AddDays(-10),
                        DueDate = DateTime.Now.AddDays(4),
                        Status = "Approved",
                        FineAmount = 0
                    },
                    new BorrowRecord
                    {
                        UserId = reader.UserId,
                        BookId = sapiens.BookId,
                        BorrowDate = DateTime.Now.AddDays(-20),
                        DueDate = DateTime.Now.AddDays(-6),
                        ReturnDate = DateTime.Now.AddDays(-5),
                        Status = "Returned",
                        FineAmount = 5000 // Overdue by 1 day
                    },
                    new BorrowRecord
                    {
                        UserId = reader.UserId,
                        BookId = camNgot.BookId,
                        BorrowDate = DateTime.Now.AddDays(-2),
                        DueDate = DateTime.Now.AddDays(12),
                        Status = "Pending",
                        FineAmount = 0
                    }
                );

                context.Reviews.AddRange(
                    new Review
                    {
                        UserId = reader.UserId,
                        BookId = decNhanTam.BookId,
                        Rating = 5,
                        Comment = "Sách cực kỳ ý nghĩa, ai cũng nên đọc ít nhất một lần trong đời!",
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new Review
                    {
                        UserId = reader.UserId,
                        BookId = sapiens.BookId,
                        Rating = 4,
                        Comment = "Kiến thức đồ sộ, mở mang tầm mắt, tuy nhiên một số chương hơi dài dòng.",
                        CreatedAt = DateTime.Now.AddDays(-3)
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
