using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class ServiceListModel : PageModel
    {
        public List<Service> servicelist = new List<Service>();

        public void OnGet()
        {
            servicelist = new DAL().GetServiceAll();
        }

    }
}
