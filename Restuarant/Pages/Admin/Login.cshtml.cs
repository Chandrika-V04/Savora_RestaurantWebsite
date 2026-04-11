using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Restaurant.Pages.Admin
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string username { get; set; }

        [BindProperty]
        public string password { get; set; }

        public IActionResult OnPost()
        {
            if (username == "admin" && password == "admin123")
            {
                return RedirectToPage("/Admin/Dashboard");
            }

            return Page();
        }
    }
}