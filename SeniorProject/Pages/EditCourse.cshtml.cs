using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class EditCourseModel : PageModel
    {
        public Courses cr {  get; set; }
        public void OnGet()
        {
            int id = Convert.ToInt32(Request.Query["id"]);
            cr = new DAL().GetSpecificCourse(id);
        }

        public void OnPost()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            string Title = Request.Form["title"];
            string Description = Request.Form["description"];
            int Duration = int.Parse(Request.Form["duration"]);
            string Instructor = Request.Form["instructor"];
            int Price = int.Parse(Request.Form["price"]);
            string Location = Request.Form["location"];
            DateOnly StartTime = DateOnly.Parse(Request.Form["starttime"]);
            Courses cr;
            cr = new Courses(id, Title, Description,Instructor, Duration, Price, Location, StartTime);
            new DAL().UpdateCourse(cr);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
