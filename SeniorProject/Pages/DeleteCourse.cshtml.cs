using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class DeleteCourseModel : PageModel
    {
        public void OnGet()
        {
            int id = Convert.ToInt32(Request.Query["id"]);
            new DAL().DeleteCourse(id);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
