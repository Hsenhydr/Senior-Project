using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class CompletedModel : PageModel
    {
        public void OnGet()
        {
            string email = HttpContext.Session.GetString("email");
            int id = Convert.ToInt32(Request.Query["id"]);
            new DAL().completeTransaction(id);
            new DAL().ToggleSubscriptionStatus(email);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
