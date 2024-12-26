using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class CoursesModel : PageModel
    {
        public List<Courses> courselist = new List<Courses>();
        public void OnGet()
        {
            courselist = new DAL().GetCourses();
        }
    }
}
