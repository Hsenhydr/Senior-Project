namespace SeniorProject.Models
{
    public class Courses
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor {  get; set; }
        public int Price { get; set; }
        public int Duration {  get; set; }
        public string Location {  get; set; }
        public DateOnly StartDate { get; set; }

        public Courses(int id, string title, string description, string instructor, int price, int duration, string location, DateOnly startDate)
        {
            Id = id;
            Title = title;
            Description = description;
            Instructor = instructor;
            Price = price;
            Duration = duration;
            Location = location;
            StartDate = startDate;
        }
    }
}
