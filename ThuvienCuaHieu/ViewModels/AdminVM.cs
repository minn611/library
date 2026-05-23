using System.Collections.Generic;
using ThuvienCuaHieu.Models;

namespace ThuvienCuaHieu.ViewModels
{
    public class AdminVM
    {
        public int TotalBooks { get; set; }
        public int TotalCategories { get; set; }
        public int TotalMembers { get; set; }
        public int TotalActiveBorrows { get; set; }
        public int PendingApprovalsCount { get; set; }
        public int TotalOverdueRecords { get; set; }

        public List<BorrowRecord> RecentBorrowRequests { get; set; } = new List<BorrowRecord>();
        public List<Book> TopViewedBooks { get; set; } = new List<Book>();

        // Chart Data (Serialized JSON arrays)
        public string ChartMonthsJson { get; set; } = "[]";
        public string ChartBorrowCountsJson { get; set; } = "[]";
    }
}
