-- =======================================================
-- KỊCH BẢN KHỞI TẠO CƠ SỞ DỮ LIỆU "THƯ VIỆN CỦA HIẾU"
-- Tương thích: Microsoft SQL Server
-- =======================================================

-- 1. Tạo cơ sở dữ liệu (Nếu chạy độc lập, hãy uncomment 2 dòng dưới)
-- CREATE DATABASE ThuvienCuaHieu;
-- GO
-- USE ThuvienCuaHieu;
-- GO

-- XÓA BẢNG CŨ NẾU ĐÃ TỒN TẠI (Theo thứ tự khóa ngoại để tránh xung đột)
IF OBJECT_ID('Notifications', 'U') IS NOT NULL DROP TABLE Notifications;
IF OBJECT_ID('Wishlists', 'U') IS NOT NULL DROP TABLE Wishlists;
IF OBJECT_ID('Reviews', 'U') IS NOT NULL DROP TABLE Reviews;
IF OBJECT_ID('BorrowRecords', 'U') IS NOT NULL DROP TABLE BorrowRecords;
IF OBJECT_ID('Books', 'U') IS NOT NULL DROP TABLE Books;
IF OBJECT_ID('Categories', 'U') IS NOT NULL DROP TABLE Categories;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
GO

-- 2. TẠO CẤU TRÚC BẢNG (DDL)

-- Bảng Người dùng (Users)
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL, -- SHA-256 Hashed
    Phone NVARCHAR(20) NULL,
    Avatar NVARCHAR(500) NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'User', -- 'Admin' hoặc 'User'
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- Bảng Thể loại (Categories)
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    IconUrl NVARCHAR(255) NULL, -- Lưu class icon FontAwesome (ví dụ: 'fa-brain')
    Description NVARCHAR(500) NULL
);

-- Bảng Sách (Books)
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(250) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    CategoryId INT NOT NULL,
    ISBN NVARCHAR(50) NULL,
    Description NVARCHAR(MAX) NULL,
    CoverImageUrl NVARCHAR(500) NULL,
    Publisher NVARCHAR(150) NULL,
    PublishedYear INT NOT NULL DEFAULT 2000,
    TotalQty INT NOT NULL DEFAULT 1,
    AvailableQty INT NOT NULL DEFAULT 1,
    ViewCount INT NOT NULL DEFAULT 0,
    SourceUrl NVARCHAR(500) NULL, -- Thêm trường SourceUrl lưu link dilib.vn
    CONSTRAINT FK_Books_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE NO ACTION
);

-- Bảng Lịch sử mượn trả (BorrowRecords)
CREATE TABLE BorrowRecords (
    RecordId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    BorrowDate DATETIME NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Approved', 'Returned', 'Rejected'
    FineAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_BorrowRecords_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    CONSTRAINT FK_BorrowRecords_Books FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE
);

-- Bảng Đánh giá sách (Reviews)
CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(1000) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_Books FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE
);

-- Bảng Sách yêu thích (Wishlists)
CREATE TABLE Wishlists (
    WishlistId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    AddedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Wishlists_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    CONSTRAINT FK_Wishlists_Books FOREIGN KEY (BookId) REFERENCES Books(BookId) ON DELETE CASCADE
);

-- Bảng Thông báo (Notifications)
CREATE TABLE Notifications (
    NotifId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Message NVARCHAR(1000) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);
GO

-- TẠO CHỈ MỤC (INDEXES) ĐỂ TĂNG TỐC ĐỘ TÌM KIẾM
CREATE INDEX IX_Books_Title ON Books(Title);
CREATE INDEX IX_Books_Author ON Books(Author);
CREATE INDEX IX_Books_CategoryId ON Books(CategoryId);
CREATE INDEX IX_BorrowRecords_Status ON BorrowRecords(Status);
GO

-- 3. CHÈN DỮ LIỆU THỬ NGHIỆM MẪU (DML - SEED DATA)

-- Chèn dữ liệu mẫu bảng Người dùng (Mật khẩu mặc định băm SHA-256)
-- Mật khẩu Admin: Admin@123 -> Hash: 240763f0d2c0b0292723cf2c125606443c220f8c32dcd7505e6bfa2dcd7501a3 (SHA-256)
-- Mật khẩu Độc Giả: User@123 -> Hash: bd731558bf2a0957bcfceacbfa3513220f8c32dcd7505e6bfa2dcd7505e61234a
INSERT INTO Users (FullName, Email, PasswordHash, Phone, Avatar, Role, IsActive, CreatedAt) VALUES
(N'Trần Trung Hiếu (Admin)', 'admin@thuvien.com', 'a3c220f8c32dcd7505e6bfa2dcd7501a3a3c220f8c32dcd7505e6bfa2dcd7501a3', '0987654321', 'https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&q=80&w=200', 'Admin', 1, GETDATE()),
(N'Nguyễn Văn Độc Giả', 'user@thuvien.com', 'bd731558bf2a0957bcfceacbfa3513220f8c32dcd7505e6bfa2dcd7505e61234a', '0123456789', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&q=80&w=200', 'User', 1, GETDATE());

-- Chèn dữ liệu mẫu bảng Thể loại
SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (CategoryId, Name, IconUrl, Description) VALUES
(1, N'Tâm lý - Kỹ năng sống', 'fa-brain', N'Sách rèn luyện kỹ năng và thấu hiểu nội tâm'),
(2, N'Lịch sử - Quân sự', 'fa-landmark', N'Lược sử loài người, lịch sử dân tộc và danh tướng'),
(3, N'Kinh doanh - Quản trị', 'fa-chart-line', N'Bí quyết kinh doanh, làm giàu và quản trị doanh nghiệp'),
(4, N'Văn học - Tiểu thuyết', 'fa-book-open', N'Các tác phẩm văn học, truyện dài và tiểu thuyết nổi tiếng'),
(5, N'Khoa học - Công nghệ', 'fa-flask', N'Khám phá khoa học, công nghệ hiện đại và vũ trụ'),
(6, N'Phát triển bản thân', 'fa-seedling', N'Tự hoàn thiện, thói quen tốt và phát triển tư duy'),
(7, N'Thiếu nhi - Giáo dục', 'fa-child', N'Sách truyện dành cho trẻ em và các phương pháp giáo dục');
SET IDENTITY_INSERT Categories OFF;

-- Chèn dữ liệu mẫu bảng Sách (Books)
SET IDENTITY_INSERT Books ON;
INSERT INTO Books (BookId, Title, Author, CategoryId, ISBN, Description, CoverImageUrl, Publisher, PublishedYear, TotalQty, AvailableQty, ViewCount, SourceUrl) VALUES
-- Tâm lý
(1, N'Đắc Nhân Tâm', 'Dale Carnegie', 1, '9780671723651', N'Cuốn sách nghệ thuật ứng xử, giao tiếp hàng đầu mọi thời đại, giúp kết nối con người và gặt hái thành công vượt bậc.', 'https://covers.openlibrary.org/b/isbn/9780671723651-M.jpg', N'NXB Tổng hợp TP.HCM', 1936, 5, 5, 120, 'https://dilib.vn/dac-nhan-tam-403.html'),
(2, N'Tuổi Trẻ Đáng Giá Bao Nhiêu', N'Rosie Nguyễn', 1, '9786045332657', N'Tác phẩm truyền cảm hứng mạnh mẽ cho giới trẻ Việt Nam về việc định vị bản thân, học hỏi, đi và cống hiến hết mình.', 'https://dilib.vn/img/news/thumb/tuoi-tre-dang-gia-bao-nhieu.jpg', N'NXB Hội Nhà Văn', 2016, 6, 6, 210, 'https://dilib.vn/thu-vien/tam-ly-ky-nang/'),
(3, N'Dám Bị Ghét', 'Ichiro Kishimi', 1, '9786045636069', N'Trình bày về thuyết tâm lý học cá nhân của Adler, giúp độc giả tự cởi trói bản thân khỏi quá khứ và định kiến xã hội để hạnh phúc.', 'https://covers.openlibrary.org/b/id/10527866-M.jpg', N'NXB Lao Động', 2013, 4, 4, 145, 'https://dilib.vn/thu-vien/tam-ly-ky-nang/'),

-- Lịch sử
(4, N'Sapiens - Lược Sử Loài Người', 'Yuval Noah Harari', 2, '9780062316097', N'Hành trình tiến hóa đầy kinh ngạc của loài người từ thời đồ đá cũ cho đến cuộc cách mạng công nghệ sinh học ngày nay.', 'https://covers.openlibrary.org/b/isbn/9780062316097-M.jpg', N'NXB Tri Thức', 2011, 4, 4, 310, 'https://dilib.vn/sapiens-luoc-su-loai-nguoi-1695.html'),
(5, N'Danh Tướng Việt Nam', N'Nguyễn Khắc Thuần', 2, '9786046808794', N'Giới thiệu những vị anh hùng và danh tướng lỗi lạc trong lịch sử kháng chiến giữ nước của dân tộc Việt Nam qua các triều đại.', 'https://dilib.vn/img/news/thumb/danh-tuong-viet-nam-7203.jpg', N'NXB Giáo Dục', 2005, 3, 3, 85, 'https://dilib.vn/danh-tuong-viet-nam-7203.html'),
(6, N'Lão Tử Đạo Đức Kinh', N'Nguyễn Duy Cần', 2, '9786049028711', N'Bản dịch và bình chú sâu sắc của cụ Thu Giang Nguyễn Duy Cần về kiệt tác triết học phương Đông Đạo Đức Kinh của Lão Tử.', 'https://dilib.vn/img/news/thumb/lao-tu-dao-duc-kinh-291.jpg', N'NXB Trẻ', 1960, 3, 3, 105, 'https://dilib.vn/lao-tu-dao-duc-kinh-291.html'),
(7, N'Cổ Học Tinh Hoa', N'Nguyễn Văn Ngọc', 2, '9786049028681', N'Những câu chuyện ngụ ngôn giàu ý nghĩa sâu sắc, giúp tu dưỡng nhân cách đạo đức đúc kết từ tinh hoa cổ học phương Đông.', 'https://dilib.vn/img/news/thumb/co-hoc-tinh-hoa-1056.jpg', N'NXB Văn Học', 1928, 4, 4, 98, 'https://dilib.vn/co-hoc-tinh-hoa-1056.html'),

-- Kinh doanh
(8, N'Tiền Đẻ Ra Tiền', 'Duncan Bannatyne', 3, '9786049028889', N'Hướng dẫn thực tế, dễ hiểu của doanh nhân nổi tiếng Duncan Bannatyne về cách quản lý tài chính và đầu tư khôn ngoan.', 'https://dilib.vn/img/news/thumb/tien-de-ra-tien-7900.jpg', N'NXB Lao Động - Xã Hội', 2008, 5, 5, 160, 'https://dilib.vn/tien-de-ra-tien-7900.html'),
(9, N'10 Bài Học Trên Chiếc Khăn Ăn', 'Don Failla', 3, '9786049028902', N'Kinh điển về tiếp thị liên kết và xây dựng mạng lưới kinh doanh bền vững, giúp bạn thấu hiểu bản chất ngành kinh doanh.', 'https://dilib.vn/img/news/thumb/10-bai-hoc-tren-chiec-khan-an-15765.jpg', N'NXB Trẻ', 1984, 3, 3, 110, 'https://dilib.vn/10-bai-hoc-tren-chiec-khan-an-15765.html'),
(10, N'10 Bí Quyết Diễn Giả Tài Năng', 'Carmine Gallo', 3, '9786049028926', N'Bật mí những bí quyết của các diễn giả hàng đầu thế giới như Steve Jobs để bạn làm chủ sân khấu và thuyết phục người nghe hoàn hảo.', 'https://dilib.vn/img/news/thumb/10-bi-quyet-thanh-cong-dien-gia.jpg', N'NXB Tổng hợp TP.HCM', 2014, 3, 3, 74, 'https://dilib.vn/10-bi-quyet-thanh-cong-cua-nhung-dien-gia-mc-tai-nang-nhat-the-gioi-7924.html'),

-- Văn học
(11, N'Cây Cam Ngọt Của Tôi', 'José Mauro de Vasconcelos', 4, '9786043567892', N'Câu chuyện đẫm nước mắt về cậu bé Zezé thông minh, giàu trí tưởng tượng cùng cuộc đời nghèo khó nhưng lấp lánh sự tử tế và tình thương yêu.', 'https://dilib.vn/img/news/thumb/cay-cam-ngot-cua-toi-7903.jpg', N'NXB Hội Nhà Văn', 1968, 3, 2, 385, 'https://dilib.vn/cay-cam-ngot-cua-toi-7903.html'),
(12, N'Nhà Giả Kim', 'Paulo Coelho', 4, '9780062315007', N'Cuốn sách cổ tích hiện đại đầy triết lý sâu xa về hành trình theo đuổi vận mệnh của cậu bé chăn cừu Santiago.', 'https://covers.openlibrary.org/b/isbn/9780062315007-M.jpg', N'NXB Hội Nhà Văn', 1988, 4, 4, 290, 'https://dilib.vn/thu-vien/van-hoc-nghe-thuat/'),
(13, N'Khu Vườn Bí Mật', 'Frances H. Burnett', 4, '9786049028957', N'Một tác phẩm thiếu nhi kinh điển kỳ diệu về tình bạn và sức sống thiên nhiên chữa lành tâm hồn hai đứa trẻ cô độc.', 'https://dilib.vn/img/news/thumb/khu-vuon-bi-mat-10335.jpg', N'NXB Kim Đồng', 1911, 3, 3, 115, 'https://dilib.vn/khu-vuon-bi-mat-10335.html'),
(14, N'Cuộc Đời Của Pi', 'Yann Martel', 4, '9786049028971', N'Hành trình sinh tồn kỳ diệu ngoài đại dương của cậu bé Pi Patel trên chiếc xuồng cứu sinh cùng một con hổ Bengal dữ tợn.', 'https://dilib.vn/img/news/thumb/cuoc-doi-cua-pi-14045.jpg', N'NXB Hội Nhà Văn', 2001, 4, 4, 135, 'https://dilib.vn/cuoc-doi-cua-pi-14045.html'),
(15, N'1 Cm Ánh Dương', N'Mặc Bảo Phi Bảo', 4, '9786049028995', N'Cuộc tình sâu sắc vượt thời gian đầy gian nan nhưng vô cùng ấm áp giữa chàng phóng viên chiến trường Kỷ Ninh Dương và cô bạn nhỏ.', 'https://dilib.vn/img/news/thumb/1-cm-anh-duong-7909.jpg', N'NXB Văn Học', 2015, 2, 2, 152, 'https://dilib.vn/1-cm-anh-duong-7909.html'),
(16, N'Đông Chu Liệt Quốc', N'Phùng Mộng Long', 4, '9786049029015', N'Bộ tiểu thuyết dã sử đồ sộ tái hiện lại thời kỳ Xuân Thu Chiến Quốc huy hoàng đầy biến động và mưu chước mưu sinh.', 'https://dilib.vn/img/news/thumb/dong-chu-liet-quoc-9511.jpg', N'NXB Văn Học', 1627, 3, 3, 68, 'https://dilib.vn/dong-chu-liet-quoc-9511.html'),

-- Khoa học
(17, N'Mẹ Lưu Manh, Con Thiên Tài', N'Quỷ Miêu Tử', 5, '9786049029053', N'Câu chuyện hài hước, dí dỏm kể về cuộc đấu trí không khoan nhượng giữa người mẹ tinh quái và cậu con trai thiên tài siêu quậy.', 'https://dilib.vn/img/news/thumb/me-luu-manh-con-thien-tai-10767.jpg', N'NXB Phụ Nữ', 2010, 3, 3, 189, 'https://dilib.vn/me-luu-manh-con-thien-tai-10767.html'),
(18, N'Khoa Học Tâm Linh', N'Hoàng Nhật Minh', 5, '9786049029077', N'Khám phá khoa học dưới lăng kính tâm linh, hành trình tự tìm lại chính mình và kết nối sâu sắc với vũ trụ bao la.', 'https://dilib.vn/img/news/thumb/khoa-hoc-tam-linh-hanh-trinh-tim-lai-chinh-minh-5504.jpg', N'NXB Hồng Đức', 2020, 2, 2, 92, 'https://dilib.vn/khoa-hoc-tam-linh-hanh-trinh-tim-lai-chinh-minh-5504.html'),
(19, N'Chúa Tể Những Chiếc Nhẫn', 'J.R.R. Tolkien', 5, '9780007525546', N'Kiệt tác sử thi kỳ ảo vĩ đại nhất mọi thời đại về cuộc chiến tiêu hủy chiếc Nhẫn Chúa cứu rỗi vùng Trung Địa.', 'https://covers.openlibrary.org/b/isbn/9780007525546-M.jpg', N'NXB Kim Đồng', 1954, 3, 3, 220, 'https://dilib.vn/chua-te-nhung-chiec-nhan-doan-ho-nhan-quyen-1-9451.html'),

-- Phát triển bản thân
(20, 'Atomic Habits', 'James Clear', 6, '9780735211292', N'Phương pháp cực kỳ khoa học và thực tế để thay đổi 1% mỗi ngày giúp bạn hình thành thói quen tốt và loại bỏ thói quen xấu vĩnh viễn.', 'https://covers.openlibrary.org/b/isbn/9780735211292-M.jpg', N'NXB Thế Giới', 2018, 4, 4, 240, 'https://dilib.vn/thu-vien/phat-triên-ban-than/');
SET IDENTITY_INSERT Books OFF;

-- Chèn dữ liệu mẫu bảng Lịch sử mượn trả
INSERT INTO BorrowRecords (UserId, BookId, BorrowDate, DueDate, ReturnDate, Status, FineAmount) VALUES
(2, 1, DATEADD(day, -10, GETDATE()), DATEADD(day, 4, GETDATE()), NULL, 'Approved', 0),
(2, 4, DATEADD(day, -20, GETDATE()), DATEADD(day, -6, GETDATE()), DATEADD(day, -5, GETDATE()), 'Returned', 5000),
(2, 11, DATEADD(day, -2, GETDATE()), DATEADD(day, 12, GETDATE()), NULL, 'Pending', 0);

-- Chèn dữ liệu mẫu bảng Đánh giá sách
INSERT INTO Reviews (UserId, BookId, Rating, Comment, CreatedAt) VALUES
(2, 1, 5, N'Sách cực kỳ ý nghĩa, ai cũng nên đọc ít nhất một lần trong đời!', DATEADD(day, -5, GETDATE())),
(2, 4, 4, N'Kiến thức đồ sộ, mở mang tầm mắt, tuy nhiên một số chương hơi dài dòng.', DATEADD(day, -3, GETDATE()));
GO
