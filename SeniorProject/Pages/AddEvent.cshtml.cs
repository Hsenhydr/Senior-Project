using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class AddEventModel : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost()
        {
            var title = Request.Form["title"];
            var type = Request.Form["type"];
            DateTime startTime =Convert.ToDateTime(Request.Form["starttime"]);
            DateTime endTime = Convert.ToDateTime(Request.Form["endtime"]);
            DateTime date = Convert.ToDateTime(Request.Form["date"]);
            int capacity =Convert.ToInt32(Request.Form["capacity"]);
            int price = Convert.ToInt32( Request.Form["price"]);
            var location = Request.Form["location"];
            var instructor = Request.Form["instructor"];
            var description = Request.Form["description"];

            new DAL().AddNewEvent(title, type, startTime, endTime, date, capacity, price, location, instructor, description);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
