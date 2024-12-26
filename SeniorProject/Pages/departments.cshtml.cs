using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class departmentsModel : PageModel
    {
        public List<Department> list = new List<Department>();
        public void OnGet()
        {
            Department dp = new Department();
            list = new DAL().getDepartment();
        }
    }
}
