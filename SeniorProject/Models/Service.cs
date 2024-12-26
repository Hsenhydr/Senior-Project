namespace SeniorProject.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public string DepartmentTitle { get; set; }
        public string ServiceType { get; set; }

    public Service() { }

        public Service(int id, string title, string description, string image, string departmentTitle, string servicetype)
        {
            Id = id;
            Title = title;
            Description = description;
            Image = image;
            DepartmentTitle = departmentTitle;
            ServiceType = servicetype;
        }
        public Service(int id, string title, string description, string image, string servicetype)
        {
            Id = id;
            Title = title;
            Description = description;
            Image = image;
            ServiceType = servicetype;
        }

        public Service(int id, string title, string description, string departmentTitle)
        {
            Id = id;
            Title = title;
            Description = description;
            DepartmentTitle = departmentTitle;
        }
    }
}
