using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;
using System.Numerics;

namespace SeniorProject.Pages
{
    public class plansModel : PageModel
    {

        public List<Plan> planlist = new List<Plan>();
        public string? ServiceTitle { get; set; }
        public void OnGet()
        {
            int id = Convert.ToInt32(Request.Query["serviceid"]);
            Plan planmodel = new Plan();
            planlist = new DAL().getPlans(id);
            ServiceTitle = planlist.First().ServiceTitle;
        }

        public void OnPost()
        {
            string email = HttpContext.Session.GetString("email");
            int id = Convert.ToInt32(Request.Query["serviceid"]);
            int planid = Convert.ToInt32(Request.Form["planId"]);
            float amount = float.Parse(Request.Form["planPrice"]);

            if (string.IsNullOrEmpty(email))
            {
                Response.Redirect("/"); 

                return;
            }
            new DAL().AddTransaction(email, id, planid, amount);
            new DAL().ToggleSubscriptionStatus(email);
            Response.Redirect("/"); 
        }



    }
}