using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class EditEventModel : PageModel
    {
        public Events ev { get; set; }
        public void OnGet()
        {

            int id = Convert.ToInt32(Request.Query["id"]);
            ev = new DAL().GetSpecificEvent(id);
        }

        public void OnPost()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            var title = Request.Form["title"];
            var type = Request.Form["type"];
            DateTime startTime = DateTime.Parse(Request.Form["starttime"]);
            DateTime endTime = Convert.ToDateTime(Request.Form["endtime"]);
            DateTime date = Convert.ToDateTime(Request.Form["date"]);
            int capacity = Convert.ToInt32(Request.Form["capacity"]);
            int price = Convert.ToInt32(Request.Form["price"]);
            var location = Request.Form["location"];
            var instructor = Request.Form["instructor"];
            var description = Request.Form["description"];
            Events ev;
            ev = new Events(id, title, type, startTime, endTime, date, capacity, price, location, instructor, description);
            new DAL().UpdateEvent(ev);
            Response.Redirect("/dashboard/dashboard");

        }
    }
}
