using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Restuarant.Models;
using System;

namespace Restuarant.Pages.Admin{
    public class ViewReservationModel : PageModel{
        private readonly IConfiguration _configuration;
        public ViewReservationModel(IConfiguration configuration){
            _configuration = configuration;
        }
        public Reservation Reservation { get; set; }
        public void OnGet(int id){
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString)){
                string query = "SELECT * FROM Reservations WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader()){
                    if (reader.Read()){
                        Reservation = new Reservation{
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"]?.ToString(),
                            PhoneNumber = reader["PhoneNumber"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = (TimeSpan)reader["Time"],
                            NoOfPeople = Convert.ToInt32(reader["NoOfPeople"]),
                            Location = reader["Location"]?.ToString(),
                            SeatingPreference = reader["SeatingPreference"]?.ToString(),
                            EventType = reader["EventType"]?.ToString(),
                            DecorTheme = reader["DecorTheme"]?.ToString(),
                            FoodPreference = reader["FoodPreference"]?.ToString(),
                            Status = reader["Status"]?.ToString()
                        };
                    }
                }
            }
        }
    }
}