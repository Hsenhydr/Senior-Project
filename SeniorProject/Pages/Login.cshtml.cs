using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace SeniorProject.Pages
{
    public class LoginModel : PageModel
    {
        public string error { get; set; }
        public void OnPostSignin()
        {
            bool res;
            string nationalid = Request.Form["nationalid"];
            string areacode = Request.Form["areacode"];
            string firstname = Request.Form["fname"];
            string lastname = Request.Form["lname"];
            string phone = Request.Form["phn"];
            string email = Request.Form["email"];
            string password = Request.Form["pass"];
            string adress = Request.Form["adress"];
            bool Admin;

            res = new DAL().Register(nationalid, firstname, lastname, adress, areacode, phone, email, password);

            if (res)
            {
                Admin = new DAL().IsAdmin(email);
                HttpContext.Session.SetString("admin", Admin.ToString());
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("Fname", firstname); 
                Response.Redirect("/");
            }
            else
            {
               
                error = "National ID, or Email already exists.";
            }
        }



        public void OnPostLog()
        {
            string Email = Request.Form["LEmail"];
            string password = Request.Form["Lpass"];
            string FirstName;
            bool Admin;
            int res = new DAL().auth(Email, password);
            if(res>0)
            {
                error = "Password is not correct";
            }
            else if(res<0)
            {
                error = "Email does not exist";
            }
            else
            {
                FirstName = new DAL().getFirstName(Email);
                Admin = new DAL().IsAdmin(Email);
                HttpContext.Session.SetString("admin", Admin.ToString());
                HttpContext.Session.SetString("Fname", FirstName);
                HttpContext.Session.SetString("email", Email);
                Response.Redirect("/");
            }
        }
    }
}
