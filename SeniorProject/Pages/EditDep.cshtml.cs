using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;
using System.Collections.Generic;

namespace SeniorProject.Pages
{
    public class EditDepModel : PageModel
    {
        public Department Department { get; set; }
        public void OnGet()
        {
            int id =Convert.ToInt32(Request.Query["id"]);
             Department = new DAL().getADepartment(id);
        }

        public void OnPost()
        {
            string desc;
            string image;
            int id;
            string title;
            Department dp;

             id =Convert.ToInt32(Request.Form["id"]);
             title = Request.Form["deptitle"];
            desc= Request.Form["description"];
             image = Request.Form["image"];

            dp = new Department(id, title, desc, image);
            new DAL().updateDep(dp);
            Response.Redirect("/dashboard/dashboard");
        }
    }
}
