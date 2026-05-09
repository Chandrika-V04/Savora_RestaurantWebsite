using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Restuarant.Models;
using System;
namespace Restuarant.Pages.Admin{
    public class EditReservationModel : PageModel{
        private readonly IConfiguration _configuration;
        public EditReservationModel(IConfiguration configuration){
            _configuration = configuration;}
        [BindProperty]
        public Reservation Reservation { get; set; }
        public IActionResult OnGet(int id){
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString)){
                string query = @"SELECT Id, Name, PhoneNumber, Email, Date, Time, NoOfPeople,
                                 Location, SeatingPreference, EventType, DecorTheme, FoodPreference, Status
                                 FROM Reservations WHERE Id=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader()){
                    if (reader.Read()){
                        Reservation = new Reservation{
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
                            Status = reader.IsDBNull(12) ? "Pending" : reader.GetString(12)};
                    }
                }
            }return Page();
        }
        public IActionResult OnPost(int id){
            Reservation.Id = id;
            if (!ModelState.IsValid){
                return Page();}
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString)){
                string query = @"UPDATE Reservations SET Name=@name,
                                     PhoneNumber=@phone,Email=@email,Date=@date,Time=@time,
                                     NoOfPeople=@people,Location=@location,SeatingPreference=@seating,
                                     EventType=@event,DecorTheme=@decor,FoodPreference=@food,
                                     Status=@status WHERE Id=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Reservation.Id);
                cmd.Parameters.AddWithValue("@name", Reservation.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@phone", Reservation.PhoneNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@email", Reservation.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@date", Reservation.Date);
                cmd.Parameters.AddWithValue("@time", Reservation.Time);
                cmd.Parameters.AddWithValue("@people", Reservation.NoOfPeople);
                cmd.Parameters.AddWithValue("@location", Reservation.Location ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@seating", Reservation.SeatingPreference ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@event", Reservation.EventType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@decor", Reservation.DecorTheme ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@food", Reservation.FoodPreference ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", Reservation.Status ?? "Pending");
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0){
                    ModelState.AddModelError("", "Update failed. Reservation not found.");
                    return Page();}
            }
            TempData["SuccessMessage"] = "Changes updated successfully!";
            return RedirectToPage("Reservations");}
    }
}