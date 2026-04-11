using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
namespace Restuarant.Pages.Admin{
    public class AnalyticsModel : PageModel{
        private readonly IConfiguration _configuration;
        public AnalyticsModel(IConfiguration configuration){
            _configuration = configuration;}

        //Basic Stats
        public int TotalReservations { get; set; }
        public int TodayReservations { get; set; }
        public int PendingReservations { get; set; }
        public int ApprovedReservations { get; set; }
        public int CancelledReservations { get; set; }
        public int CompletedReservations { get; set; }
        public int ThisMonthReservations { get; set; }

        // ✅ Months
        public List<string> Months { get; set; } = new()
        {"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"};

        // ✅ Charts Data
        public List<int> WhitefieldData { get; set; } = new();
        public List<int> HennurData { get; set; } = new();
        public List<string> FoodLabels { get; set; } = new();
        public List<int> FoodCounts { get; set; } = new();
        public List<string> SeatingLabels { get; set; } = new();
        public List<int> SeatingCounts { get; set; } = new();
        public List<string> EventLabels { get; set; } = new();
        public List<int> EventCounts { get; set; } = new();
        public void OnGet(){
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString)){
                conn.Open();
                TotalReservations = GetCount(conn, "SELECT COUNT(*) FROM Reservations");
                TodayReservations = GetCount(conn,
                    "SELECT COUNT(*) FROM Reservations WHERE CAST(Date AS DATE) = CAST(GETDATE() AS DATE)");
                PendingReservations = GetCount(conn,
                    "SELECT COUNT(*) FROM Reservations WHERE Status = 'Pending' OR Status IS NULL");
                ApprovedReservations = GetCount(conn,
                    "SELECT COUNT(*) FROM Reservations WHERE Status = 'Approved'");
                CancelledReservations = GetCount(conn,
                    "SELECT COUNT(*) FROM Reservations WHERE Status = 'Cancelled'");
                CompletedReservations = GetCount(conn,
                    "SELECT COUNT(*) FROM Reservations WHERE Status = 'Completed'");
                ThisMonthReservations = GetCount(conn,
                    "SELECT COUNT(*) FROM Reservations WHERE MONTH(Date) = MONTH(GETDATE()) AND YEAR(Date) = YEAR(GETDATE())");
                // 🔥 Load All Analytics
                LoadMonthlyBranchData(conn);
                LoadFoodAnalytics(conn);
                LoadSeatingAnalytics(conn);
                LoadEventAnalytics(conn);
            }
        }

        // ✅ Common Count Method
        private int GetCount(SqlConnection conn, string query) {
            using (SqlCommand cmd = new SqlCommand(query, conn)){
                return (int)cmd.ExecuteScalar();
            }
        }

        // 🔥 Monthly Branch Analytics
        private void LoadMonthlyBranchData(SqlConnection conn){
            WhitefieldData.Clear();
            HennurData.Clear();
            Dictionary<int, int> whitefield = new();
            Dictionary<int, int> hennur = new();
            string query = @"
                SELECT MONTH(Date) AS MonthNumber,Location,COUNT(*) AS Total FROM Reservations GROUP BY MONTH(Date), Location";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader()){
                while (reader.Read()){
                    int month = Convert.ToInt32(reader["MonthNumber"]);
                    string location = reader["Location"]?.ToString()?.Trim() ?? "";
                    int count = Convert.ToInt32(reader["Total"]);
                    if (location.Equals("Whitefield", StringComparison.OrdinalIgnoreCase))
                        whitefield[month] = count;
                    else if (location.Equals("Hennur", StringComparison.OrdinalIgnoreCase))
                        hennur[month] = count;
                }
            }
            for (int i = 1; i <= 12; i++) {
                WhitefieldData.Add(whitefield.ContainsKey(i) ? whitefield[i] : 0);
                HennurData.Add(hennur.ContainsKey(i) ? hennur[i] : 0);
            }
        }

        // 🔥 Food Analytics
        private void LoadFoodAnalytics(SqlConnection conn){
            FoodLabels.Clear();
            FoodCounts.Clear();
            string query = @"
                SELECT FoodPreference, COUNT(*) AS Total FROM Reservations
                WHERE FoodPreference IS NOT NULL AND FoodPreference != '' GROUP BY FoodPreference
                ORDER BY Total DESC";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader()){
                while (reader.Read()){
                    string food = reader["FoodPreference"]?.ToString()?.Trim() ?? "Unknown";
                    int count = Convert.ToInt32(reader["Total"]);
                    FoodLabels.Add(food);
                    FoodCounts.Add(count);
                }
            }
        }

        // 🔥 Seating Analytics
        private void LoadSeatingAnalytics(SqlConnection conn){
            SeatingLabels.Clear();
            SeatingCounts.Clear();
            string query = @"
                SELECT SeatingPreference, COUNT(*) AS Total FROM Reservations
                WHERE SeatingPreference IS NOT NULL AND SeatingPreference != '' GROUP BY SeatingPreference
                ORDER BY Total DESC";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader()){
                while (reader.Read()){
                    string seating = reader["SeatingPreference"]?.ToString()?.Trim() ?? "Unknown";
                    int count = Convert.ToInt32(reader["Total"]);
                    SeatingLabels.Add(seating);
                    SeatingCounts.Add(count);
                }
            }
        }

        // 🔥 Event Analytics
        private void LoadEventAnalytics(SqlConnection conn){
            EventLabels.Clear();
            EventCounts.Clear();
            string query = @"
                SELECT EventType, COUNT(*) AS Total FROM Reservations
                WHERE EventType IS NOT NULL AND EventType != '' GROUP BY EventType ORDER BY Total DESC";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataReader reader = cmd.ExecuteReader()){
                while (reader.Read()){
                    string evt = reader["EventType"]?.ToString()?.Trim() ?? "Unknown";
                    int count = Convert.ToInt32(reader["Total"]);
                    EventLabels.Add(evt);
                    EventCounts.Add(count);
                }
            }
        }
    }
}