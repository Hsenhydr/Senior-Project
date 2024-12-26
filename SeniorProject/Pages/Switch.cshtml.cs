using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class SwitchModel : PageModel
    {
        public void OnGet()
        {
            string email = Request.Query["email"];
            new DAL().ToggleIsAdmin(email);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
