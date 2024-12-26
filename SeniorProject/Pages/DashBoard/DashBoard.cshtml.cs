using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;
using System.Data.SqlClient;

namespace SeniorProject.Pages.DashBoard
{
    public class DashBoardModel : PageModel
    {
     
        public List<Person> Adminlist = new List<Person>();
        public List<Person> userlist = new List<Person>();
        public List<Service> servicelist = new List<Service>();
        public List<Department> list = new List<Department>();
        public List<Events> events = new List<Events>();
        public List<Courses> courses = new List<Courses>();
        public List<Transactions> tran = new List<Transactions>();
        public List<Transactions> ongoing = new List<Transactions>();
        public List<Transactions> completed = new List<Transactions>();

        
        public void OnGet()
        {
            new DAL().whishcheck();

            Adminlist = new DAL().GetAdmins();
            userlist = new DAL().GetUsers();
            servicelist = new DAL().GetServiceAndDep();
            list = new DAL().getDepartment();
            events = new DAL().GetallEvents();
            courses = new DAL().GetCourses();
            tran = new DAL().GetPendingTransactions();
            ongoing = new DAL().GetOngoingTransactions();
            completed = new DAL().GetCompletedTransactions();
    }

       

    }
}
