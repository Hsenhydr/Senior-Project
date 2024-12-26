using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class ServicesModel : PageModel
    {
        public List<Service> servicelist = new List<Service>();
        public string ?DepartmentTitle { get; set; }
      
        public void OnGet()
        {
            int id = Convert.ToInt32(Request.Query["depid"]);
            Service servicemodel = new Service();
            servicelist = new DAL().getService(id);
            DepartmentTitle = servicelist.First().DepartmentTitle;
        }
   

    }
}

