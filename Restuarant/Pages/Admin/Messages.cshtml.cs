using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Restuarant.Pages.Admin
{
    public class MessagesModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public MessagesModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<MessageData> MessagesList { get; set; } = new List<MessageData>();

        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Feedback";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MessageData msg = new MessageData();

                    msg.Name = reader["Name"].ToString();
                    msg.Email = reader["Email"].ToString();
                    msg.Subject = reader["Subject"].ToString();
                    msg.Message = reader["Message"].ToString();

                    MessagesList.Add(msg);
                }
            }
        }

        public class MessageData
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }
    }
}