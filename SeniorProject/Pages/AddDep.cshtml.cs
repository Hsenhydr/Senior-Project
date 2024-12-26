using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages
{
    public class AddDepModel : PageModel
    {
        public void OnPost()
        {
            string title = Request.Form["deptitle"];
            string desc = Request.Form["description"];
            string image = Request.Form["image"];

            new DAL().addDep(title, desc, image);

        }
    }
}
