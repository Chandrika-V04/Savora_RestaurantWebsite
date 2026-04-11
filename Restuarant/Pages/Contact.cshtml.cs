using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;

namespace Restuarant.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ContactModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Bind form fields directly
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Subject { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Feedback 
                                     (Name, Email, Subject, Message)
                                     VALUES 
                                     (@Name, @Email, @Subject, @Message)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Subject", Subject);
                        cmd.Parameters.AddWithValue("@Message", Message);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["SuccessMessage"] = "Message sent successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }
    }
}