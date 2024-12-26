namespace SeniorProject.Models
{
    public class Plan
    {
            public int PlanID { get; set; }
            public string Title { get; set; }
            public double PlanPrice { get; set; }
            public string PlanDescription { get; set; }
            public string VoiceOver { get; set; }
            public string Location { get; set; }
            public string Duration { get; set; }
            public int? Revisions { get; set; }
            public string DesignType { get; set; }
            public int? NumberOfDesigns { get; set; }
            public string PlatformManaged { get; set; }
            public int? PostsPerMonth { get; set; }
            public string ReportingFrequency { get; set; }
            public string MotionGraphicType { get; set; }
            public string VideoLength { get; set; }
            public int? ServiceID { get; set; }
            public string Image { get; set; }
            public string ServiceTitle { get; set; }

            
            public Plan() { }

        public Plan(
            int planID,
            string title,
            double planPrice,
            string planDescription,
            string voiceOver,
            string location,
            string duration,
            int? revisions,
            string designType,
            int? numberOfDesigns,
            string platformManaged,
            int? postsPerMonth,
            string reportingFrequency,
            string motionGraphicType,
            string videoLength,
            int? serviceID,
                string image,
                string servicetitle)
            {
                PlanID = planID;
                Title = title;
                PlanPrice = planPrice;
                PlanDescription = planDescription;
                VoiceOver = voiceOver;
                Location = location;
                Duration = duration;
                Revisions = revisions;
                DesignType = designType;
                NumberOfDesigns = numberOfDesigns;
                PlatformManaged = platformManaged;
                PostsPerMonth = postsPerMonth;
                ReportingFrequency = reportingFrequency;
                MotionGraphicType = motionGraphicType;
                VideoLength = videoLength;
                ServiceID = serviceID;
                Image = image;
            ServiceTitle = servicetitle;

            }
        }

    }

