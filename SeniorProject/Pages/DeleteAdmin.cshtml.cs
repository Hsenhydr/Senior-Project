using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class DeleteAdminModel : PageModel
    {
        public void OnGet()
        {
            string id = Request.Query["id"];
            new DAL().DeleteUser(id);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
