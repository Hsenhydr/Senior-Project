using System.Diagnostics;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;

namespace SeniorProject.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public String Image { get; set; }



        public Department() { }

        public Department(int DepartmentId, string title, string description, string image)
        {
            this.DepartmentId = DepartmentId;
            this.Title = title;
            this.Description = description;
            this.Image = image;
        }


    }
}
