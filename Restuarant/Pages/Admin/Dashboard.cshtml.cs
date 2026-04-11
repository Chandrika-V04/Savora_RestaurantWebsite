using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;

namespace Restuarant.Pages.Admin{
    public class DashboardModel : PageModel{
        private readonly IConfiguration _configuration;
        public DashboardModel(IConfiguration configuration){
            _configuration = configuration;
        }
        public int TotalReservations { get; set; }
        public int TodayReservations { get; set; }
        public int PendingReservations { get; set; }
        public int ApprovedReservations { get; set; }
        public int CancelledReservations { get; set; }
        public int CompletedReservations { get; set; }
        public void OnGet(){
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString)){
                conn.Open();
                SqlCommand totalCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservations", conn);
                TotalReservations = (int)totalCmd.ExecuteScalar();

                SqlCommand todayCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservations WHERE CAST(Date AS DATE) = CAST(GETDATE() AS DATE)", conn);
                TodayReservations = (int)todayCmd.ExecuteScalar();

                SqlCommand pendingCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservations WHERE Status = 'Pending' OR Status IS NULL", conn);
                PendingReservations = (int)pendingCmd.ExecuteScalar();

                SqlCommand approvedCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservations WHERE Status = 'Approved'", conn);
                ApprovedReservations = (int)approvedCmd.ExecuteScalar();

                SqlCommand cancelledCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservations WHERE Status = 'Cancelled'", conn);
                CancelledReservations = (int)cancelledCmd.ExecuteScalar();

                SqlCommand completedCmd = new SqlCommand(
                   "SELECT COUNT(*) FROM Reservations WHERE Status = 'Completed'", conn);
                CompletedReservations = (int)completedCmd.ExecuteScalar();
            }
        }
    }
}