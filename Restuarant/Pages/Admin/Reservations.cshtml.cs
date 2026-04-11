using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Restuarant.Models;

namespace Restuarant.Pages.Admin
{
    public class ReservationsModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ReservationsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Reservation> ListReservations { get; set; } = new List<Reservation>();

        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT Id, Name, PhoneNumber, Email, Date, Time, NoOfPeople,
                                 Location, SeatingPreference, EventType, DecorTheme,
                                 FoodPreference, Status
                                 FROM Reservations ORDER BY Date DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Reservation r = new Reservation
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        PhoneNumber = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Date = reader.GetDateTime(4),
                        Time = reader.GetTimeSpan(5),
                        NoOfPeople = reader.GetInt32(6),
                        Location = reader.IsDBNull(7) ? "" : reader.GetString(7),
                        SeatingPreference = reader.IsDBNull(8) ? "" : reader.GetString(8),
                        EventType = reader.IsDBNull(9) ? "" : reader.GetString(9),
                        DecorTheme = reader.IsDBNull(10) ? "" : reader.GetString(10),
                        FoodPreference = reader.IsDBNull(11) ? "" : reader.GetString(11),
                        Status = reader.IsDBNull(12) ? "Pending" : reader.GetString(12)
                    };

                    ListReservations.Add(r);
                }
            }
        }

        public IActionResult OnPostUpdateStatus(int id, string status)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Reservations SET Status=@status WHERE Id=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@status", status);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToPage();
        }
    }
}