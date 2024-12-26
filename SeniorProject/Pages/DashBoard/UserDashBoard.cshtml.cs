using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;

namespace SeniorProject.Pages.DashBoard
{
    public class UserDashBoardModel : PageModel
    {
        public List<Transactions> transactionlist = new List<Transactions>();
        public List<Transactions> formlist = new List<Transactions>();
        public void OnGet()
        {
            string email = HttpContext.Session.GetString("email");
            transactionlist = new DAL().GetUserDashboardplan(email);
            formlist = new DAL().GetUserDashboardform(email);
        }
    }
}
