namespace SeniorProject.Models
{
    public class Events
    {
            public int Id { get; set; } // Primary Key
            public string Title { get; set; } // Event Title
            public string Type { get; set; } // Event Type (e.g., Conference, Workshop)
            public DateTime StartTime { get; set; } // Event Start Time
            public DateTime EndTime { get; set; } // Event End Time
            public DateTime Date { get; set; } // Event Date
            public int Capacity { get; set; } // Max number of attendees
            public int Price { get; set; } // Event Price
            public string Location { get; set; } // Event Location
            public string Instructor { get; set; } // Event Instructor
            public string Description { get; set; } // Event Description

        public Events(int id, string title, string type, DateTime startTime, DateTime endTime, DateTime date, int capacity, int price, string location, string instructor, string description)
        {
            Id = id;
            Title = title;
            Type = type;
            StartTime = startTime;
            EndTime = endTime;
            Date = date;
            Capacity = capacity;
            Price = price;
            Location = location;
            Instructor = instructor;
            Description = description;
        }

       
    }
}
