using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;
using System.Numerics;

namespace SeniorProject.Pages
{
    public class FormsModel : PageModel
    {
        public List<Courses> courselist = new List<Courses>();
        public List<Events> eventlist = new List<Events>();
      
        public void OnGet()
        {
            courselist = new DAL().GetCourses();
            eventlist = new DAL().GetEvents();
        }

        public void OnPostSeo()
        {
            string email = Request.Form["Email"];
            string firstname = Request.Form["FirstName"];
            string lastname = Request.Form["LastName"];
            string Website = Request.Form["Website"];
            string CurrentSeoStatus = Request.Form["CurrentSeoStatus"];
            int MonthlyBudget = Convert.ToInt32(Request.Form["MonthlyBudget"]);
            DateOnly SeoStartDate =DateOnly.Parse(Request.Form["SeoStartDate"]);
            string notes = Request.Form["notes"];
            int serviceid = Convert.ToInt32(Request.Form["serviceid"]);
            string formType = Request.Form["formtype"];

            new DAL().AddForm( email, firstname, lastname, Website, CurrentSeoStatus,MonthlyBudget, SeoStartDate, notes, serviceid, formType);

            Response.Redirect("/Index");
        }

        public void OnPostDev()
        {
            string email = Request.Form["Email"];
            string firstname = Request.Form["FirstName"];
            string lastname = Request.Form["LastName"];
            string ProjectName = Request.Form["ProjectName"];
            string ProjectType = Request.Form["ProjectType"];
            string responsiveValue = Request.Form["Responsive"];  // Get the value from the form

            // Convert the "Responsive" or "Non-responsive" to a boolean
            bool isResponsive = false;
            if (responsiveValue == "Responsive")
            {
                isResponsive = true;  // If "Responsive" is selected, set true
            }
            else if (responsiveValue == "Non-responsive")
            {
                isResponsive = false; // If "Non-responsive" is selected, set false
            }
            string Platform = Request.Form["Platform"];
            DateOnly TimeLine = DateOnly.Parse(Request.Form["Timeline"]);
            string notes = Request.Form["notes"];
            int Budget =Convert.ToInt32(Request.Form["Budget"]);
            int serviceid = Convert.ToInt32(Request.Form["serviceid"]);
            string formType = Request.Form["formtype"];

            new DAL().AddDevForm( email, firstname, lastname, ProjectName, ProjectType, isResponsive,Platform, TimeLine, notes,Budget, serviceid, formType);

            Response.Redirect("/Index");
        }

        public void OnPostCourse()
        {
            string email = Request.Form["Email"];
            string firstname = Request.Form["FirstName"];
            string lastname = Request.Form["LastName"];
            string CourseName = Request.Form["CourseName"];
            string notes = Request.Form["notes"];
            int serviceid = Convert.ToInt32(Request.Form["serviceid"]);
            string formType = Request.Form["formtype"];

            new DAL().AddCourseForm(email, firstname, lastname, CourseName, notes, serviceid, formType);

            Response.Redirect("/Index");

        }

        public void OnPostEvent()
        {
            string email = Request.Form["Email"];
            string firstname = Request.Form["FirstName"];
            string lastname = Request.Form["LastName"];
            int EventID =Convert.ToInt32(Request.Form["Eventname"]);
            string notes = Request.Form["notes"];
            int serviceid = Convert.ToInt32(Request.Form["serviceid"]);
            string formType = Request.Form["formtype"];
            int price = Convert.ToInt32(Request.Form["Amount"]);

            new DAL().AddEventForm(email, firstname, lastname, EventID, notes, serviceid, formType);
            new DAL().AddFormTransaction(email, serviceid,price,formType );
            Response.Redirect("/Index");    

        }
    }
}
