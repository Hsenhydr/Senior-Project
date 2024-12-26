using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class AddServiceModel : PageModel
    {
        public List<Department> list = new List<Department>();
        public void OnGet()
        {
            Department dp = new Department();
            list = new DAL().getDepartment();
        }

        public void OnPost()
        {
            string title = Request.Form["servicetitle"];
            string desc = Request.Form["description"];
            int depid = Convert.ToInt32(Request.Form["depid"]);
            string image = Request.Form["image"];
            string type = Request.Form["type"];

            new DAL().addService(title, desc, depid, image,type);
        }
    }
}
