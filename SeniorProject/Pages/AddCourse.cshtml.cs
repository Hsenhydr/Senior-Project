using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class AddCourseModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost()
        {
            string Title = Request.Form["title"];
            string Description = Request.Form["description"];
            int Duration = int.Parse(Request.Form["duration"]);
            string Instructor = Request.Form["instructor"];
            int Price = int.Parse(Request.Form["price"]);
            string Location = Request.Form["location"];
            DateTime StartTime = DateTime.Parse(Request.Form["starttime"]);

            new DAL().AddCourse(Title, Description, Duration, Instructor, Price, Location, StartTime);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
