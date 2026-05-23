# 📚 THƯ VIỆN CỦA HIẾU
## *"Nơi tri thức ngự trị"*
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
## 6. Kế Hoạch Thực Hiện

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

> 🎨 **Tông màu teal + hồng đào** tạo cảm giác biển — yên bình, học thuật nhưng không khô khan. Đúng tinh thần *"Nơi tri thức ngự trị"*!
