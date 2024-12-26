using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class EventsModel : PageModel
    {
        public List<Events> eventlist = new List<Events>();
        public void OnGet()
        {
            eventlist = new DAL().GetEvents();
        }
    }
}
