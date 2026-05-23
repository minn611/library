# 📚 THƯ VIỆN CỦA HIẾU
## *"Nơi tri thức ngự trị"*

> **Công nghệ:** ASP.NET Core MVC · Entity Framework Core · SQL Server · Bootstrap 5  
> **IDE:** Visual Studio Code · .NET 8

---

## 1. Tầm Nhìn & Bản Sắc Thương Hiệu

| | |
|---|---|
| **Tên trang** | Thư Viện Của Hiếu |
| **Slogan** | *"Nơi tri thức ngự trị"* |
| **Màu chủ đạo** | Tông biển — xanh teal & hồng đào nhẹ |
| **Font chữ** | Georgia / Playfair Display (tiêu đề) + Arial / Inter (nội dung) |
| **Logo ý tưởng** | Quyển sách mở với tia sáng tỏa ra — biểu tượng tri thức lan tỏa |
| **Đối tượng** | Học sinh, sinh viên, người yêu sách mọi lứa tuổi |

**Lấy cảm hứng từ dilib.vn:** trang có cấu trúc rõ ràng theo thể loại, icon card dễ nhận biết, layout tập trung vào nội dung. Thư Viện Của Hiếu học theo cách tổ chức thể loại trực quan đó nhưng thêm tính năng mượn/trả sách vật lý và quản lý người dùng có tài khoản.

---

## 2. 🎨 Bảng Màu Giao Diện

> Tông màu lấy cảm hứng từ mặt biển lúc hoàng hôn — yên tĩnh, nhẹ nhàng, phù hợp không khí đọc sách.

| Tên màu | Mã HEX | Vai trò |
|---|---|---|
| **Xanh bạc hà** | `#D4E5E3` | Nền section, viền card, hover |
| **Xanh xám nhạt** | `#9BBDC8` | Chi tiết phụ, viền phân cách |
| **Xanh teal vừa** | `#4E8FA3` | Badge "còn sách", link, gradient hero |
| **Xanh teal đậm** | `#2D5F70` | Navbar, footer, tiêu đề section |
| **Hồng đào** | `#F0A090` | Nút CTA, điểm nhấn, sao đánh giá |
| **Kem ấm** | `#F5EDD8` | Nền tổng thể, chữ trên nền tối |

```css
/* CSS Variables — dán vào wwwroot/css/site.css */
:root {
  --c-mint:    #D4E5E3;   /* Xanh bạc hà */
  --c-blue-lt: #9BBDC8;   /* Xanh xám nhạt */
  --c-teal:    #4E8FA3;   /* Xanh teal vừa */
  --c-teal-dk: #2D5F70;   /* Xanh teal đậm */
  --c-peach:   #F0A090;   /* Hồng đào */
  --c-cream:   #F5EDD8;   /* Kem ấm */
  --text-dark: #1a3540;
  --text-mid:  #3d6675;
  --text-light:#7aa3b0;
}
```

---

## 3. Các Trang & Chức Năng Chính

### 🏠 Trang Chủ (`/`)
- **Hero Section:** Banner nền `#2D5F70→#4E8FA3`, slogan *"Nơi tri thức ngự trị"*, thanh tìm kiếm nổi bật
- **Danh mục nổi bật:** Grid icon theo thể loại (giống dilib.vn)
- **Sách mới cập nhật:** Slider card ảnh bìa + tên + tác giả
- **Sách được mượn nhiều nhất:** Top 10 dạng bảng xếp hạng
- **Trích dẫn tri thức:** Quote ngẫu nhiên từ tác giả nổi tiếng, nền `#2D5F70`
- **Thống kê:** "1,200 đầu sách · 350 thành viên · 89 cuốn đang được mượn"

### 📖 Trang Thư Viện (`/sach`)
- Bộ lọc bên trái: Thể loại · Tác giả · Năm XB · Trạng thái
- Grid card: ảnh bìa, tên, tác giả, sao đánh giá, badge còn/hết (màu `#4E8FA3` / `#F0A090`)
- Sắp xếp: Mới nhất / Phổ biến / A-Z · Phân trang

### 🔍 Trang Chi Tiết Sách (`/sach/{id}`)
- Ảnh bìa + thông tin đầy đủ · Nút **"Mượn ngay"** (màu `#F0A090`) · Nút ❤️ yêu thích
- Tab: Mô tả · Đánh giá sao · Sách liên quan

### 👤 Tài Khoản & Admin
- Đăng ký / Đăng nhập / Trang cá nhân / Lịch sử mượn / Wishlist
- Admin: Dashboard biểu đồ · Quản lý sách/người dùng/mượn trả

---

## 4. Thiết Kế Cơ Sở Dữ Liệu

```sql
Users          (UserId, FullName, Email, PasswordHash, Phone, Avatar, Role, IsActive, CreatedAt)
Books          (BookId, Title, Author, CategoryId, ISBN, Description, CoverImageUrl,
                Publisher, PublishedYear, TotalQty, AvailableQty, ViewCount)
Categories     (CategoryId, Name, IconUrl, Description)
BorrowRecords  (RecordId, UserId, BookId, BorrowDate, DueDate, ReturnDate, Status, FineAmount)
Reviews        (ReviewId, UserId, BookId, Rating, Comment, CreatedAt)
Wishlists      (WishlistId, UserId, BookId, AddedAt)
Notifications  (NotifId, UserId, Message, IsRead, CreatedAt)
```

---

## 5. Kiến Trúc Project

```
ThuvienCuaHieu/
├── Controllers/
│   ├── HomeController.cs
│   ├── BooksController.cs
│   ├── BorrowController.cs
│   ├── AccountController.cs
│   └── Admin/  (Dashboard · Books · Users · Borrows)
├── Models/       (Book · Category · User · BorrowRecord · Review)
├── ViewModels/   (HomeVM · BookSearchVM · BookDetailVM · AdminVM)
├── Services/     (BookService · BorrowService · UserService)
├── Data/         (AppDbContext · SeedData)
├── Views/        (Home · Books · Borrow · Account · Admin · Shared)
└── wwwroot/
    ├── css/site.css   ← chứa CSS Variables màu ở trên
    ├── js/
    └── uploads/covers/
```

---

## 6. 📚 Danh Sách Sách Mẫu Cho Thư Viện

> Dữ liệu seed thực tế — copy link ảnh bìa và link tham khảo vào `SeedData.cs`

---

### 🧠 Tâm Lý - Kỹ Năng Sống

| # | Tên sách | Tác giả | Ảnh bìa | Link tham khảo |
|---|---|---|---|---|
| 1 | Đắc Nhân Tâm | Dale Carnegie | ![Đắc Nhân Tâm](https://dilib.vn/img/news/thumb/dac-nhan-tam-403.jpg) | [🔗 dilib.vn](https://dilib.vn/dac-nhan-tam-403.html) |
| 2 | Tuổi Trẻ Đáng Giá Bao Nhiêu | Rosie Nguyễn | ![Tuổi Trẻ](https://dilib.vn/img/news/thumb/tuoi-tre-dang-gia-bao-nhieu.jpg) | [🔗 dilib.vn](https://dilib.vn/thu-vien/tam-ly-ky-nang/) |
| 3 | Atomic Habits | James Clear | ![Atomic Habits](https://covers.openlibrary.org/b/id/10527843-M.jpg) | [🔗 dilib.vn](https://dilib.vn/thu-vien/tam-ly-ky-nang/) |
| 4 | Dám Bị Ghét | Ichiro Kishimi | ![Dám Bị Ghét](https://covers.openlibrary.org/b/id/10527866-M.jpg) | [🔗 dilib.vn](https://dilib.vn/thu-vien/tam-ly-ky-nang/) |
| 5 | Nhà Giả Kim | Paulo Coelho | ![Nhà Giả Kim](https://covers.openlibrary.org/b/id/8479576-M.jpg) | [🔗 dilib.vn](https://dilib.vn/thu-vien/van-hoc-nghe-thuat/) |

---

### 📜 Lịch Sử - Nhân Vật

| # | Tên sách | Tác giả | Ảnh bìa | Link tham khảo |
|---|---|---|---|---|
| 1 | Sapiens - Lược Sử Loài Người | Yuval Noah Harari | ![Sapiens](https://dilib.vn/img/news/thumb/sapiens-luoc-su-loai-nguoi-1695.jpg) | [🔗 dilib.vn](https://dilib.vn/sapiens-luoc-su-loai-nguoi-1695.html) |
| 2 | Danh Tướng Việt Nam | Nguyễn Khắc Thuần | ![Danh Tướng](https://dilib.vn/img/news/thumb/danh-tuong-viet-nam-7203.jpg) | [🔗 dilib.vn](https://dilib.vn/danh-tuong-viet-nam-7203.html) |
| 3 | Lão Tử Đạo Đức Kinh | Nguyễn Duy Cần | ![Lão Tử](https://dilib.vn/img/news/thumb/lao-tu-dao-duc-kinh-291.jpg) | [🔗 dilib.vn](https://dilib.vn/lao-tu-dao-duc-kinh-291.html) |
| 4 | Cổ Học Tinh Hoa | Ôn Như | ![Cổ Học](https://dilib.vn/img/news/thumb/co-hoc-tinh-hoa-1056.jpg) | [🔗 dilib.vn](https://dilib.vn/co-hoc-tinh-hoa-1056.html) |

---

### 💼 Kinh Doanh - Quản Trị

| # | Tên sách | Tác giả | Ảnh bìa | Link tham khảo |
|---|---|---|---|---|
| 1 | Tiền Đẻ Ra Tiền | Duncan Bannatyne | ![Tiền Đẻ Ra Tiền](https://dilib.vn/img/news/thumb/tien-de-ra-tien-7900.jpg) | [🔗 dilib.vn](https://dilib.vn/tien-de-ra-tien-7900.html) |
| 2 | 10 Bài Học Trên Chiếc Khăn Ăn | Don Failla | ![10 Bài Học](https://dilib.vn/img/news/thumb/10-bai-hoc-tren-chiec-khan-an-15765.jpg) | [🔗 dilib.vn](https://dilib.vn/10-bai-hoc-tren-chiec-khan-an-15765.html) |
| 3 | 10 Bí Quyết Diễn Giả Tài Năng | Carmine Gallo | ![10 Bí Quyết](https://dilib.vn/img/news/thumb/10-bi-quyet-thanh-cong-dien-gia.jpg) | [🔗 dilib.vn](https://dilib.vn/10-bi-quyet-thanh-cong-cua-nhung-dien-gia-mc-tai-nang-nhat-the-gioi-7924.html) |

---

### 📖 Văn Học - Tiểu Thuyết

| # | Tên sách | Tác giả | Ảnh bìa | Link tham khảo |
|---|---|---|---|---|
| 1 | Cây Cam Ngọt Của Tôi | José Mauro | ![Cây Cam](https://dilib.vn/img/news/thumb/cay-cam-ngot-cua-toi-7903.jpg) | [🔗 dilib.vn](https://dilib.vn/cay-cam-ngot-cua-toi-7903.html) |
| 2 | Khu Vườn Bí Mật | Frances H. Burnett | ![Khu Vườn](https://dilib.vn/img/news/thumb/khu-vuon-bi-mat-10335.jpg) | [🔗 dilib.vn](https://dilib.vn/khu-vuon-bi-mat-10335.html) |
| 3 | Cuộc Đời Của Pi | Yann Martel | ![Pi](https://dilib.vn/img/news/thumb/cuoc-doi-cua-pi-14045.jpg) | [🔗 dilib.vn](https://dilib.vn/cuoc-doi-cua-pi-14045.html) |
| 4 | 1 Cm Ánh Dương | Mặc Bảo Phi Bảo | ![1 Cm Ánh Dương](https://dilib.vn/img/news/thumb/1-cm-anh-duong-7909.jpg) | [🔗 dilib.vn](https://dilib.vn/1-cm-anh-duong-7909.html) |
| 5 | Đông Chu Liệt Quốc | Phùng Mộng Long | ![Đông Chu](https://dilib.vn/img/news/thumb/dong-chu-liet-quoc-9511.jpg) | [🔗 dilib.vn](https://dilib.vn/dong-chu-liet-quoc-9511.html) |

---

### 🔬 Khoa Học - Phát Triển Bản Thân

| # | Tên sách | Tác giả | Ảnh bìa | Link tham khảo |
|---|---|---|---|---|
| 1 | Mẹ Lưu Manh, Con Thiên Tài | Quỷ Miêu Tử | ![Mẹ Lưu Manh](https://dilib.vn/img/news/thumb/me-luu-manh-con-thien-tai-10767.jpg) | [🔗 dilib.vn](https://dilib.vn/me-luu-manh-con-thien-tai-10767.html) |
| 2 | Khoa Học Tâm Linh | Hoàng Nhật Minh | ![Khoa Học TL](https://dilib.vn/img/news/thumb/khoa-hoc-tam-linh-hanh-trinh-tim-lai-chinh-minh-5504.jpg) | [🔗 dilib.vn](https://dilib.vn/khoa-hoc-tam-linh-hanh-trinh-tim-lai-chinh-minh-5504.html) |
| 3 | Chúa Tể Những Chiếc Nhẫn | J.R.R. Tolkien | ![CTNCT](https://covers.openlibrary.org/b/id/8406786-M.jpg) | [🔗 dilib.vn](https://dilib.vn/chua-te-nhung-chiec-nhan-doan-ho-nhan-quyen-1-9451.html) |

---

## 7. Ảnh Bìa Sách — Nguồn Lấy Ảnh

> Dùng cho trường `CoverImageUrl` trong database hoặc seed data.

| Nguồn | URL mẫu | Ghi chú |
|---|---|---|
| **Open Library** | `https://covers.openlibrary.org/b/isbn/{ISBN}-M.jpg` | Miễn phí, đa dạng |
| **Open Library (by ID)** | `https://covers.openlibrary.org/b/id/{ID}-M.jpg` | ID lấy từ OpenLibrary |
| **dilib.vn** | Từ các link bên trên | Tham khảo, không lấy trực tiếp |
| **Google Books API** | `https://www.googleapis.com/books/v1/volumes?q={title}` | API miễn phí, trả về ảnh bìa |
| **Tự upload** | `/uploads/covers/{ten-sach}.jpg` | Lưu vào `wwwroot/uploads/covers/` |

```csharp
// Ví dụ lấy ảnh bìa từ Open Library theo ISBN
// ISBN 9780062316097 = Atomic Habits
string coverUrl = $"https://covers.openlibrary.org/b/isbn/{book.ISBN}-M.jpg";
```

---

## 8. Seed Data Mẫu (SeedData.cs)

```csharp
public static void Initialize(AppDbContext context)
{
    if (context.Books.Any()) return;

    var categories = new Category[]
    {
        new Category { Name = "Tâm lý - Kỹ năng sống", IconUrl = "/img/icons/tam-ly.png" },
        new Category { Name = "Lịch sử - Quân sự",     IconUrl = "/img/icons/lich-su.png" },
        new Category { Name = "Kinh doanh - Quản trị", IconUrl = "/img/icons/kinh-doanh.png" },
        new Category { Name = "Văn học - Tiểu thuyết", IconUrl = "/img/icons/van-hoc.png" },
        new Category { Name = "Khoa học - Công nghệ",  IconUrl = "/img/icons/khoa-hoc.png" },
        new Category { Name = "Phát triển bản thân",   IconUrl = "/img/icons/phat-trien.png" },
        new Category { Name = "Thiếu nhi - Giáo dục",  IconUrl = "/img/icons/thieu-nhi.png" },
    };
    context.Categories.AddRange(categories);

    var books = new Book[]
    {
        new Book {
            Title = "Đắc Nhân Tâm", Author = "Dale Carnegie",
            ISBN = "9780671723651",
            CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780671723651-M.jpg",
            SourceUrl = "https://dilib.vn/dac-nhan-tam-403.html",
            TotalQty = 5, AvailableQty = 5, PublishedYear = 1936,
            CategoryId = 1
        },
        new Book {
            Title = "Sapiens - Lược Sử Loài Người", Author = "Yuval Noah Harari",
            ISBN = "9780062316097",
            CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780062316097-M.jpg",
            SourceUrl = "https://dilib.vn/sapiens-luoc-su-loai-nguoi-1695.html",
            TotalQty = 3, AvailableQty = 3, PublishedYear = 2011,
            CategoryId = 2
        },
        new Book {
            Title = "Nhà Giả Kim", Author = "Paulo Coelho",
            ISBN = "9780062315007",
            CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780062315007-M.jpg",
            SourceUrl = "https://dilib.vn/thu-vien/van-hoc-nghe-thuat/",
            TotalQty = 4, AvailableQty = 4, PublishedYear = 1988,
            CategoryId = 4
        },
        new Book {
            Title = "Atomic Habits", Author = "James Clear",
            ISBN = "9780735211292",
            CoverImageUrl = "https://covers.openlibrary.org/b/isbn/9780735211292-M.jpg",
            SourceUrl = "https://dilib.vn/thu-vien/phat-trien-ban-than/",
            TotalQty = 4, AvailableQty = 4, PublishedYear = 2018,
            CategoryId = 6
        },
        new Book {
            Title = "Cây Cam Ngọt Của Tôi", Author = "José Mauro de Vasconcelos",
            ISBN = "9786043567892",
            CoverImageUrl = "https://dilib.vn/img/news/thumb/cay-cam-ngot-cua-toi-7903.jpg",
            SourceUrl = "https://dilib.vn/cay-cam-ngot-cua-toi-7903.html",
            TotalQty = 3, AvailableQty = 2, PublishedYear = 1968,
            CategoryId = 4
        },
    };
    context.Books.AddRange(books);
    context.SaveChanges();
}
```

> 💡 Thêm trường `SourceUrl` vào Model `Book` để lưu link tham khảo từ dilib.vn

---

## 9. Kế Hoạch Thực Hiện

| Giai đoạn | Nội dung | Tuần |
|---|---|---|
| **Khởi tạo** | Tạo project, cài EF Core, thiết kế DB, migration | 1 |
| **Nền tảng** | Models, DbContext, SeedData + 20 cuốn sách mẫu | 2 |
| **Sách** | CRUD sách (admin), hiển thị danh sách, chi tiết | 3 |
| **Xác thực** | Đăng ký, đăng nhập, phân quyền Admin/User | 4 |
| **Tìm kiếm** | Thanh tìm kiếm, bộ lọc thể loại, phân trang | 5 |
| **Mượn/Trả** | Luồng mượn, duyệt, trả, tính phạt trễ hạn | 6 |
| **Cá nhân hóa** | Wishlist, lịch sử, đánh giá, thông báo | 7 |
| **Admin** | Dashboard, biểu đồ Chart.js, báo cáo | 8 |
| **Giao diện** | Áp dụng bảng màu teal, responsive Bootstrap 5 | 9 |
| **Tổng kết** | Kiểm thử, fix bug, viết báo cáo | 10 |

---

## 10. Khởi Tạo Project

```bash
dotnet new mvc -n ThuvienCuaHieu
cd ThuvienCuaHieu

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

---

> 💡 **Mẹo giao diện:** Dán CSS Variables vào `wwwroot/css/site.css` ngay từ đầu — tất cả màu sắc sẽ nhất quán từ trang chủ đến admin panel chỉ bằng cách thay đổi một chỗ.
>
> 🎨 **Tông màu teal + hồng đào** tạo cảm giác biển — yên bình, học thuật nhưng không khô khan. Đúng tinh thần *"Nơi tri thức ngự trị"*!
