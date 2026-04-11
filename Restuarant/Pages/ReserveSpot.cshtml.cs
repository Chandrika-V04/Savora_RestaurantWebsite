using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Restuarant.Pages
{
    public class ReserveSpotModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ReserveSpotModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public TimeSpan Time { get; set; }

        [BindProperty]
        public int NoOfPeople { get; set; }

        [BindProperty]
        public string Location { get; set; }

        [BindProperty]
        public string SeatingPreference { get; set; }

        [BindProperty]
        public string EventType { get; set; }

        [BindProperty]
        public string DecorTheme { get; set; }

        [BindProperty]
        public string FoodPreference { get; set; }

        // ✅ Message properties added
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Reservations
                            (Name, PhoneNumber, Email, Date, Time,
                             NoOfPeople, Location, SeatingPreference,
                             EventType, DecorTheme, FoodPreference, CreatedAt)
                             VALUES
                            (@Name, @PhoneNumber, @Email, @Date, @Time,
                             @NoOfPeople, @Location, @SeatingPreference,
                             @EventType, @DecorTheme, @FoodPreference, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        cmd.Parameters.AddWithValue("@Time", Time);
                        cmd.Parameters.AddWithValue("@NoOfPeople", NoOfPeople);
                        cmd.Parameters.AddWithValue("@Location", Location);
                        cmd.Parameters.AddWithValue("@SeatingPreference", SeatingPreference);
                        cmd.Parameters.AddWithValue("@EventType", (object)EventType ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DecorTheme", (object)DecorTheme ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FoodPreference", FoodPreference);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // ✅ Success → redirect
                return RedirectToPage("/reservation-success");
            }
            catch (Exception)
            {
                // ❌ Failure → stay on same page
                Message = "❌ Failed, try again";
                IsSuccess = false;

                return Page(); // IMPORTANT (fixes error in your code)
            }
        }
    }
}