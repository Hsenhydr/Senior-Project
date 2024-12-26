namespace SeniorProject.Models
{
    public class Person
    {
        public string NationalId { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string areacode { get; set; }
        public string phonenumber { get; set; }
        public string account { get; set; }
        public string password { get; set; }

        public Person(string nationalId, string fName, string lName, string areacode, string phonenumber, string account, string password)
        {
            NationalId = nationalId;
            this.fName = fName;
            this.lName = lName;
            this.areacode = areacode;
            this.phonenumber = phonenumber;
            this.account = account;
            this.password = password;
        }

        public Person() { }
    }
}
