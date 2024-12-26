using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Numerics;
using System.Data;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Specialized;

namespace SeniorProject.Models
{
    public class DAL
    {
        public string strcon = "Data Source=.\\sqlexpress;Initial Catalog=\"Final Senior Project\";Integrated Security=True;";
        SqlConnection con;
        SqlCommand cmd;

            public List<Department> getDepartment()
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand com = new SqlCommand("select * from departments", con);
                List<Department> ls = new List<Department>();
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string Title = reader.GetString(1);
                    string Description = reader.GetString(2);
                    string Image = reader.GetString(3);

                    Department s = new Department(id, Title, Description, Image);
                    ls.Add(s);
                }
                con.Close();
                return ls;
            }

        public bool exists(string nationalid, string email)
        {
            int res = 0, em = 0;
            con = new SqlConnection(strcon);
            cmd = new SqlCommand("select count(*) from Account where AccountEmail = @d", con);
            cmd.Parameters.AddWithValue("@d", email);
            con.Open();
            em = (int)cmd.ExecuteScalar();
            con.Close();

            con = new SqlConnection(strcon);
            cmd = new SqlCommand("select count(*) from Person where NationalID =@b ", con);
            cmd.Parameters.AddWithValue("@b", nationalid);
            con.Open();
            res = (int)cmd.ExecuteScalar();
            con.Close();


            if (res == 0 && em == 0) { return false; }
            else { return true; }


        }

        public bool Register(string nationalid, string firstname, string lastname, string address, string areacode, string phonenumber, string email, string password)
        {
            bool existsAlready = exists(nationalid, email); // Check if NationalID or email already exists
            if (existsAlready)
            {
                return false;  // National ID or email already exists, return false
            }

            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlTransaction transaction = null;
                try
                {
                    con.Open();

                    // Start a transaction
                    transaction = con.BeginTransaction();

                    // Insert into Person table
                    string insertPersonQuery = @"
                INSERT INTO Person (NationalID, FirstName, LastName, Address)
                VALUES (@NationalID, @FirstName, @LastName, @Address);";

                    using (SqlCommand cmdPerson = new SqlCommand(insertPersonQuery, con, transaction))
                    {
                        cmdPerson.Parameters.AddWithValue("@NationalID", nationalid);
                        cmdPerson.Parameters.AddWithValue("@FirstName", firstname);
                        cmdPerson.Parameters.AddWithValue("@LastName", lastname);
                        cmdPerson.Parameters.AddWithValue("@Address", address);
                        cmdPerson.ExecuteNonQuery();
                    }

                    // Insert into PhoneNumber table
                    string insertPhoneNumberQuery = @"
                INSERT INTO PhoneNumber (AreaCode, PhoneNumber, NationalID)
                VALUES (@AreaCode, @PhoneNumber, @NationalID);";

                    using (SqlCommand cmdPhoneNumber = new SqlCommand(insertPhoneNumberQuery, con, transaction))
                    {
                        cmdPhoneNumber.Parameters.AddWithValue("@AreaCode", areacode);
                        cmdPhoneNumber.Parameters.AddWithValue("@PhoneNumber", phonenumber);
                        cmdPhoneNumber.Parameters.AddWithValue("@NationalID", nationalid);
                        cmdPhoneNumber.ExecuteNonQuery();
                    }

                    // Insert into Account table
                    string insertAccountQuery = @"
                INSERT INTO Account (AccountEmail, Password, NationalID)
                VALUES (@AccountEmail, @Password, @NationalID);";

                    using (SqlCommand cmdAccount = new SqlCommand(insertAccountQuery, con, transaction))
                    {
                        cmdAccount.Parameters.AddWithValue("@AccountEmail", email);
                        cmdAccount.Parameters.AddWithValue("@Password", password);
                        cmdAccount.Parameters.AddWithValue("@NationalID", nationalid);
                        cmdAccount.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();
                    return true; // Successful registration
                }
                catch (Exception ex)
                {
                    // Roll back transaction on error
                    transaction?.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                    return false; // Registration failed
                }
            }
        }

        public int auth(string email, string password)
        {
            int userExists = 0;
            int passwordMatch = 0;

            using (SqlConnection con = new SqlConnection(strcon))
            {
                // Check if the email exists
                string emailQuery = "SELECT COUNT(*) FROM Account WHERE AccountEmail = @Email";
                using (SqlCommand cmd = new SqlCommand(emailQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    userExists = (int)cmd.ExecuteScalar();
                    con.Close();
                }

                if (userExists > 0)
                {
                    // Check if the password matches for the given email
                    string passwordQuery = "SELECT COUNT(*) FROM Account WHERE AccountEmail = @Email AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(passwordQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);
                        con.Open();
                        passwordMatch = (int)cmd.ExecuteScalar();
                        con.Close();
                    }

                    if (passwordMatch > 0)
                    {
                        return 0; // Email and password match
                    }
                    else
                    {
                        return 1; // Email exists but password is incorrect
                    }
                }
            }

            return -1; // Email does not exist
        }

        public string getFirstName(string email)
        {
            string firstname = "";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT p.FirstName FROM Person p INNER JOIN Account a ON p.NationalID = a.NationalID WHERE a.AccountEmail = @a", con);
            cmd.Parameters.AddWithValue("@a", email);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                firstname = reader["FirstName"].ToString();
            }
            con.Close();

            return firstname;
        }

        public List<Service> getService(int id)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT s.ServiceID, s.ServiceTitle, s.ServiceDescription, s.ServiceImage, d.DepartmentTitle, s.ServiceType FROM Service s INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID WHERE s.DepartmentID = @a;", con);
            com.Parameters.AddWithValue("@a", id);
            List<Service> ls = new List<Service>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string Title = reader.GetString(1);
                string Description = reader.GetString(2);
                string Image = reader.GetString(3);
                string DepartmentTitle = reader.GetString(4);
                string servicetype = reader.GetString(5);


                Service s = new Service(Id, Title, Description, Image, DepartmentTitle, servicetype);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public List<Plan> getPlans(int id)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand(" SELECT p.*, s.ServiceTitle FROM [Plan] p INNER JOIN [Service] s ON p.ServiceID = s.ServiceID INNER JOIN [Departments] d ON s.DepartmentID = d.DepartmentID WHERE p.ServiceID = @a;", con);
            com.Parameters.AddWithValue("@a", id);
            List<Plan> ls = new List<Plan>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {

                int planid = reader.GetInt32(reader.GetOrdinal("PlanID"));
                string title = reader.GetString(reader.GetOrdinal("Title"));
                double planPrice = reader.GetDouble(reader.GetOrdinal("PlanPrice"));
                string planDescription = reader.GetString(reader.GetOrdinal("PlanDescription"));
                string voiceOver = reader.IsDBNull(reader.GetOrdinal("voice_over")) ? null : reader.GetString(reader.GetOrdinal("voice_over"));
                string location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location"));
                string duration = reader.IsDBNull(reader.GetOrdinal("Duration")) ? null : reader.GetString(reader.GetOrdinal("Duration"));
                int? revisions = reader.IsDBNull(reader.GetOrdinal("Revisions")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Revisions"));
                string designType = reader.IsDBNull(reader.GetOrdinal("design_type")) ? null : reader.GetString(reader.GetOrdinal("design_type"));
                int? numberOfDesigns = reader.IsDBNull(reader.GetOrdinal("number_of_designs")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("number_of_designs"));
                string platformManaged = reader.IsDBNull(reader.GetOrdinal("platform_managed")) ? null : reader.GetString(reader.GetOrdinal("platform_managed"));
                int? postsPerMonth = reader.IsDBNull(reader.GetOrdinal("posts_per_month")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("posts_per_month"));
                string reportingFrequency = reader.IsDBNull(reader.GetOrdinal("reporting_frequency")) ? null : reader.GetString(reader.GetOrdinal("reporting_frequency"));
                string motionGraphicType = reader.IsDBNull(reader.GetOrdinal("motion_graphic_type")) ? null : reader.GetString(reader.GetOrdinal("motion_graphic_type"));
                string videoLength = reader.IsDBNull(reader.GetOrdinal("video_length")) ? null : reader.GetString(reader.GetOrdinal("video_length"));
                int? serviceID = reader.IsDBNull(reader.GetOrdinal("ServiceID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ServiceID"));
                string Image = reader.IsDBNull(reader.GetOrdinal("PlanImage")) ? null : reader.GetString(reader.GetOrdinal("PlanImage"));
                string serviceTitle = reader.IsDBNull(reader.GetOrdinal("ServiceTitle")) ? null : reader.GetString(reader.GetOrdinal("ServiceTitle"));

                Plan s = new Plan(
                    planid,
                    title,
                    planPrice,
                    planDescription,
                    voiceOver,
                    location,
                    duration,
                    revisions,
                    designType,
                    numberOfDesigns,
                    platformManaged,
                    postsPerMonth,
                    reportingFrequency,
                    motionGraphicType,
                    videoLength,
                    serviceID,
                    Image,
                    serviceTitle
                );



                ls.Add(s);
            }
            con.Close();
            return ls;
        }


        public bool IsAdmin(string email)
        {
            bool bol = false;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(" SELECT p.Admin FROM Person p JOIN Account a ON p.NationalID = a.NationalID WHERE a.AccountEmail = @a;", con);
            cmd.Parameters.AddWithValue("@a", email);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                bol = Convert.ToBoolean(reader["Admin"]);

            }
            con.Close();

            return bol;
        }

        public List<Service> GetServiceAll()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from Service;", con);
            List<Service> ls = new List<Service>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string Title = reader.GetString(1);
                string Description = reader.GetString(2);
                string Image = reader.GetString(3);
                string servicetype = reader.GetString(5);


                Service s = new Service(Id, Title, Description, Image, servicetype);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }
        public List<Service> GetServiceAndDep()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT s.ServiceID, s.ServiceTitle, s.ServiceDescription, s.ServiceImage, d.DepartmentTitle FROM Service s JOIN Departments d ON s.DepartmentID = d.DepartmentID;", con);
            List<Service> ls = new List<Service>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string Title = reader.GetString(1);
                string Description = reader.GetString(2);
                string title = reader.GetString(4);


                Service s = new Service(Id, Title, Description, title);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }


        public bool isSubscribed(string email)
        {
            bool bol = false;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT p.isSubscribed FROM Person p JOIN Account a ON p.NationalID = a.NationalID WHERE a.AccountEmail = @a;", con);
            cmd.Parameters.AddWithValue("@a", email);

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // Check if the reader has any rows
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Directly convert since NULL is not a concern
                        bol = Convert.ToBoolean(reader["isSubscribed"]);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional)
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return bol;
        }


        public bool ToggleSubscriptionStatus(string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Person SET isSubscribed = CASE WHEN isSubscribed = 1 THEN 0 ELSE 1 END WHERE NationalID IN (SELECT NationalID FROM Account WHERE AccountEmail = @a)",
                        con))
                    {
                        cmd.Parameters.Add("@a", SqlDbType.NVarChar).Value = email;

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Return true if rows were updated; otherwise false
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ToggleSubscriptionStatus: " + ex.Message);
                return false;
            }
        }

        public bool ToggleIsAdmin(string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Person SET Admin = CASE WHEN Admin = 1 THEN 0 ELSE 1 END WHERE NationalID IN (SELECT NationalID FROM Account WHERE AccountEmail = @a)",
                        con))
                    {
                        cmd.Parameters.Add("@a", SqlDbType.NVarChar).Value = email;

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Return true if rows were updated; otherwise false
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ToggleSubscriptionStatus: " + ex.Message);
                return false;
            }
        }



        public List<Person> GetAdmins()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT person.NationalID, person.FirstName, person.LastName, person.Address, phonenumber.PhoneID, phonenumber.AreaCode, phonenumber.PhoneNumber, account.AccountEmail, account.Password FROM person JOIN phonenumber ON person.NationalID = phonenumber.NationalID JOIN account ON person.NationalID = account.NationalID WHERE person.Admin = 1;", con);
            List<Person> ls = new List<Person>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                string Id = reader.GetString(0);
                string fname = reader.GetString(1);
                string lname = reader.GetString(2);
                string areacode = reader.GetString(5);
                string phone = reader.GetString(6);
                string email = reader.GetString(7);
                string pass = reader.GetString(8);


                Person s = new Person(Id, fname, lname, areacode, phone, email, pass);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public List<Person> GetUsers()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT person.NationalID, person.FirstName, person.LastName, person.Address, phonenumber.PhoneID, phonenumber.AreaCode, phonenumber.PhoneNumber, account.AccountEmail, account.Password FROM person JOIN phonenumber ON person.NationalID = phonenumber.NationalID JOIN account ON person.NationalID = account.NationalID WHERE person.Admin = 0;", con);
            List<Person> ls = new List<Person>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                string Id = reader.GetString(0);
                string fname = reader.GetString(1);
                string lname = reader.GetString(2);
                string areacode = reader.GetString(5);
                string phone = reader.GetString(6);
                string email = reader.GetString(7);
                string pass = reader.GetString(8);

                Person s = new Person(Id, fname, lname, areacode, phone, email, pass);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }
        public void DeleteUser(string nid)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("DELETE FROM PhoneNumber WHERE NationalID = @a;DELETE FROM [Transaction] WHERE NationalID = @a; DELETE FROM Account WHERE NationalID = @a; DELETE FROM Person WHERE NationalID = @a;", con);
            com.Parameters.AddWithValue("@a", nid);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }
        public void AddTransaction(string email, int serviceid, int planid, float amount)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("INSERT INTO [Transaction] (ServiceID, PlanID, NationalID, TransactionAmount) SELECT @ServiceID, @PlanID, a.NationalID, @TransactionAmount FROM Account a WHERE a.AccountEmail = @Email AND a.NationalID IS NOT NULL ", con);
            cmd.Parameters.AddWithValue("@ServiceID", serviceid);
            cmd.Parameters.AddWithValue("@PlanID", planid);
            cmd.Parameters.AddWithValue("@TransactionAmount", amount);
            cmd.Parameters.AddWithValue("@Email", email);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public List<Transactions> GetUserDashboardplan(string email)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT t.*, s.ServiceTitle, p.title AS PlanTitle FROM [Transaction] t JOIN Service s ON t.ServiceID = s.ServiceID LEFT JOIN [plan] p ON t.PlanID = p.PlanID JOIN Account a ON t.NationalID = a.NationalID JOIN Person pe ON a.NationalID = pe.NationalID WHERE t.PlanID IS NOT NULL ORDER BY t.TransactionDate DESC;\r\n", con);
            com.Parameters.AddWithValue("@a", email);
            List<Transactions> ls = new List<Transactions>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                DateTime transactionDate = reader.GetDateTime(1);  // Get the DateTime value
                DateOnly transactionDateOnly = DateOnly.FromDateTime(transactionDate);  // Convert DateTime to DateOnly

                float amount = (float)reader.GetDouble(2);  // Use GetDouble() for double type

                string status = reader.GetString(7);
                string service = reader.GetString(9);
                string plan = reader.IsDBNull(10) ? null : reader.GetString(10);


                Transactions s = new Transactions(id, amount, transactionDateOnly, status, service, plan);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }
        public List<Transactions> GetUserDashboardform(string email)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT t.*, s.ServiceTitle, p.title AS PlanTitle, fs.EventTitle, fs.CourseName FROM [Transaction] t JOIN Service s ON t.ServiceID = s.ServiceID LEFT JOIN [plan] p ON t.PlanID = p.PlanID LEFT JOIN (SELECT TOP 1 ServiceID, EventTitle, CourseName FROM FormSubmission WHERE EventTitle IS NOT NULL ORDER BY SubmissionDate) fs ON t.ServiceID = fs.ServiceID WHERE t.PlanID IS NULL ORDER BY t.TransactionDate DESC;", con);
            com.Parameters.AddWithValue("@a", email);
            List<Transactions> ls = new List<Transactions>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                DateTime transactionDate = reader.GetDateTime(1);  // Get the DateTime value
                DateOnly transactionDateOnly = DateOnly.FromDateTime(transactionDate);  // Convert DateTime to DateOnly

                float amount = (float)reader.GetDouble(2);  // Use GetDouble() for double type

                string status = reader.GetString(7);
                string type =  reader.GetString(8);
                string service = reader.GetString(9);
                string name = reader.IsDBNull(11) ? reader.GetString(12) : reader.GetString(11);



                Transactions s = new Transactions(id,type,transactionDateOnly,amount,status,name);
                ls.Add(s);
            }
            con.Close();
            return ls;

        }


        public List<Courses> GetCourses()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from Courses", con);
            List<Courses> ls = new List<Courses>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string title = reader.GetString(1);
                string description = reader.GetString(2);
                string instructor = reader.GetString(3);
                int price = reader.GetInt32(4);
                int duration = reader.GetInt32(5);
                string Location = reader.GetString(6);
                DateTime Startdate1 = reader.GetDateTime(7);  // Get the DateTime value
                DateOnly StartDate = DateOnly.FromDateTime(Startdate1);

                Courses s = new Courses(id, title, description, instructor, price, duration, Location, StartDate);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public void AddForm(string email, string firstname, string lastname, string Website, string CurrentSeoStatus, int MonthlyBudget, DateOnly SeoStartDate, string notes, int serviceid, string formType)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("INSERT INTO FormSubmission ( Email, FirstName, LastName, Website, CurrentSeoStatus, MonthlyBudget, SeoStartDate, Notes, ServiceId, FormType) VALUES ( @Email, @FirstName, @LastName, @Website, @CurrentSeoStatus, @MonthlyBudget, @SeoStartDate, @Notes, @ServiceId, @FormType);", con);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@Website", Website);
            cmd.Parameters.AddWithValue("@CurrentSeoStatus", CurrentSeoStatus);
            cmd.Parameters.AddWithValue("@MonthlyBudget", MonthlyBudget);
            cmd.Parameters.AddWithValue("@SeoStartDate", SeoStartDate.ToDateTime(new TimeOnly(0, 0))); // Convert DateOnly to DateTime
            cmd.Parameters.AddWithValue("@FormType", formType);

            cmd.Parameters.AddWithValue("@Notes", notes);
            cmd.Parameters.AddWithValue("@ServiceId", serviceid);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddDevForm(string email, string firstname, string lastname, string ProjectName, string ProjectType, bool Responsive, string Platform, DateOnly TimeLine, string notes, int Budget, int serviceid, string formType)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("INSERT INTO FormSubmission ( Email, FirstName, Lastname, ProjectName, ProjectType, Responsive,Platform, TimeLine, Notes,Budget, ServiceID, FormType) VALUES ( @Email, @FirstName, @LastName, @ProjectName, @ProjectType, @Responsive, @Platform,@TimeLine,@Notes,@Budget, @ServiceId, @FormType);", con);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@ProjectName", ProjectName);
            cmd.Parameters.AddWithValue("@ProjectType", ProjectType);
            cmd.Parameters.AddWithValue("@Responsive", Responsive);
            cmd.Parameters.AddWithValue("@TimeLine", TimeLine.ToDateTime(new TimeOnly(0, 0)));
            cmd.Parameters.AddWithValue("@Platform", Platform); // Convert DateOnly to DateTime
            cmd.Parameters.AddWithValue("@FormType", formType);
            cmd.Parameters.AddWithValue("@Notes", notes);
            cmd.Parameters.AddWithValue("@Budget", Budget);
            cmd.Parameters.AddWithValue("@ServiceId", serviceid);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddCourseForm(string email, string firstname, string lastname, String coursetitle, string notes, int serviceid, string formType)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("INSERT INTO FormSubmission ( Email, FirstName, Lastname, CourseName, Notes, ServiceID, FormType) VALUES ( @Email, @FirstName, @LastName,@coursetitle,@Notes, @ServiceId, @FormType);", con);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@coursetitle", coursetitle);
            cmd.Parameters.AddWithValue("@FormType", formType);
            cmd.Parameters.AddWithValue("@Notes", notes);
            cmd.Parameters.AddWithValue("@ServiceId", serviceid);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public List<Events> GetallEvents()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from Events", con);
            List<Events> ls = new List<Events>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0); // ID
                string title = reader.GetString(1); // Title
                string type = reader.GetString(2); // Type (assuming column 2 corresponds to Type)
                DateTime startTime = reader.GetDateTime(3); // StartTime
                DateTime endTime = reader.GetDateTime(4); // EndTime
                DateTime date = reader.GetDateTime(5); // Date
                int capacity = reader.GetInt32(6); // Capacity
                int price = reader.GetInt32(7); // Price
                string location = reader.GetString(8); // Location
                string instructor = reader.GetString(9); // Instructor
                string description = reader.GetString(10);

                Events s = new Events(id, title, type, startTime, endTime, date, capacity, price, location, instructor, description);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public List<Events> GetEvents()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from Events WHERE Date >= getdate()", con);
            List<Events> ls = new List<Events>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0); // ID
                string title = reader.GetString(1); // Title
                string type = reader.GetString(2); // Type (assuming column 2 corresponds to Type)
                DateTime startTime = reader.GetDateTime(3); // StartTime
                DateTime endTime = reader.GetDateTime(4); // EndTime
                DateTime date = reader.GetDateTime(5); // Date
                int capacity = reader.GetInt32(6); // Capacity
                int price = reader.GetInt32(7); // Price
                string location = reader.GetString(8); // Location
                string instructor = reader.GetString(9); // Instructor
                string description = reader.GetString(10);

                Events s = new Events(id, title, type, startTime, endTime, date, capacity, price, location, instructor, description);
                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public Events GetSpecificEvent(int iid)
        {
            Events ev=null;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from Events WHERE Id=@a", con);
            com.Parameters.AddWithValue("@a", iid);
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0); // ID
                string title = reader.GetString(1); // Title
                string type = reader.GetString(2); // Type (assuming column 2 corresponds to Type)
                DateTime startTime = reader.GetDateTime(3); // StartTime
                DateTime endTime = reader.GetDateTime(4); // EndTime
                DateTime date = reader.GetDateTime(5); // Date
                int capacity = reader.GetInt32(6); // Capacity
                int price = reader.GetInt32(7); // Price
                string location = reader.GetString(8); // Location
                string instructor = reader.GetString(9); // Instructor
                string description = reader.GetString(10);

                 ev = new Events(id, title, type, startTime, endTime, date, capacity, price, location, instructor, description);
            }
            con.Close();
            return ev;
        }

        public int EventAmount(int id)
        {
            int amount;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("select Price from Events where Id=@a", con);
            cmd.Parameters.AddWithValue("@a", id);
            con.Open();
            amount = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return amount;
        }


        public void AddEventForm(string email, string firstname, string lastname, int eventid, string notes, int serviceid, string formType)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("INSERT INTO FormSubmission (Email, FirstName, Lastname, EventTitle, Notes, ServiceID, FormType) SELECT @Email, @FirstName, @LastName, e.Title, @Notes, @ServiceId, @FormType FROM Events e WHERE e.Id = @EventID;;", con);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@FirstName", firstname);
            cmd.Parameters.AddWithValue("@LastName", lastname);
            cmd.Parameters.AddWithValue("@EventID", eventid);
            cmd.Parameters.AddWithValue("@FormType", formType);
            cmd.Parameters.AddWithValue("@Notes", notes);
            cmd.Parameters.AddWithValue("@ServiceId", serviceid);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void addService(string title, string desc, int depid, string image, string type)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand command = new SqlCommand("INSERT INTO Service (ServiceTitle, ServiceDescription, ServiceImage, DepartmentID, ServiceType) VALUES (@ServiceTitle, @ServiceDescription, @ServiceImage, @DepartmentID, @ServiceType)", con);
            command.Parameters.AddWithValue("@ServiceTitle", title);
            command.Parameters.AddWithValue("@ServiceDescription", desc);
            command.Parameters.AddWithValue("@ServiceImage", image);
            command.Parameters.AddWithValue("@DepartmentID", depid);
            command.Parameters.AddWithValue("@ServiceType", type);
            con.Open();
            command.ExecuteNonQuery(); 
            con.Close();
        }
        public void DeleteService(int sid)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand command = new SqlCommand("Delete From Service WHERE ServiceID=@a", con);
            command.Parameters.AddWithValue("@a", sid);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();
        }

        public void addDep(string title, string desc, string image)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand command = new SqlCommand("INSERT INTO Departments (DepartmentTitle, DepartmentDescription, DepartmentImage) VALUES (@a, @b, @c)", con);
            command.Parameters.AddWithValue("@a", title);
            command.Parameters.AddWithValue("@b", desc);
            command.Parameters.AddWithValue("@c", image);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteDep(int sid)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand command = new SqlCommand("Delete From Departments WHERE DepartmentID=@a", con);
            command.Parameters.AddWithValue("@a", sid);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();
        }

        public Department getADepartment(int id)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("select * from departments where DepartmentID=@a", con);
            com.Parameters.AddWithValue("@a", id);
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            Department department = null;
            if (reader.Read())
            {
                int did = reader.GetInt32(0);
                string title = reader.GetString(1);
                string description = reader.GetString(2);
                string image = reader.GetString(3);

                department = new Department(did, title, description, image);
            }
            con.Close();
            return department;
        }


        public void updateDep(Department db)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand command = new SqlCommand("update Departments set DepartmentTitle=@b, DepartmentDescription=@c, DepartmentImage=@d where Departmentid=@a", con);
            command.Parameters.AddWithValue("@a", db.DepartmentId);
            command.Parameters.AddWithValue("@b", db.Title);
            command.Parameters.AddWithValue("@c", db.Description);
            command.Parameters.AddWithValue("@d", db.Image);
            con.Open();
            command.ExecuteNonQuery();
            con.Close();
        }

        public int usersCount()
        {
            int count;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand command = new SqlCommand("select count(*) from person where admin=0", con);
            con.Open();
            count = Convert.ToInt32 (command.ExecuteScalar());
            con.Close();
            return count;

        }

        public void AddNewEvent(string title, string type, DateTime startTime, DateTime endTime, DateTime date, int capacity, decimal price, string location, string instructor, string description)
        {
            const string query = @"
            INSERT INTO Events 
                (Title, Type, StartTime, EndTime, Date, Capacity, Price, Location, Instructor, Description) 
            VALUES 
                (@Title, @Type, @StartTime, @EndTime, @EventDate, @Capacity, @Price, @Location, @Instructor, @Description)";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);
            {
                // Add parameters to the query
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                cmd.Parameters.AddWithValue("@EventDate", date);
                cmd.Parameters.AddWithValue("@Capacity", capacity);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@Instructor", instructor);
                cmd.Parameters.AddWithValue("@Description", description);
            }
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void UpdateEvent(Events ev)
        {
            const string query = @"
    UPDATE Events
    SET 
        Title = @Title,
        Type = @Type,
        StartTime = @StartTime,
        EndTime = @EndTime,
        Date = @EventDate,
        Capacity = @Capacity,
        Price = @Price,
        Location = @Location,
        Instructor = @Instructor,
        Description = @Description
    WHERE Id = @Id";

            using (SqlConnection con = new SqlConnection(strcon))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@Id", ev.Id);
                    cmd.Parameters.AddWithValue("@Title", ev.Title);
                    cmd.Parameters.AddWithValue("@Type", ev.Type);
                    cmd.Parameters.AddWithValue("@StartTime", ev.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", ev.EndTime);
                    cmd.Parameters.AddWithValue("@EventDate", ev.Date);
                    cmd.Parameters.AddWithValue("@Capacity", ev.Capacity);
                    cmd.Parameters.AddWithValue("@Price", ev.Price);
                    cmd.Parameters.AddWithValue("@Location", ev.Location);
                    cmd.Parameters.AddWithValue("@Instructor", ev.Instructor);
                    cmd.Parameters.AddWithValue("@Description", ev.Description);

                    // Open the connection, execute the query, and close the connection
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }


        public void DeleteEvent(int id)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("Delete from Events where id=@a", con);
            cmd.Parameters.AddWithValue("@a", id);
            con.Open() ;
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddCourse(string title, string description, int duration, string instructor, decimal price, string location, DateTime startTime)
        {
            const string query = @"
            INSERT INTO Courses (Title, Description, Duration, Instructor, Price, Location, StartDate)
            VALUES (@Title, @Description, @Duration, @Instructor, @Price, @Location, @StartTime)";

            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Duration", duration);
                cmd.Parameters.AddWithValue("@Instructor", instructor);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@StartTime", startTime);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

       public void UpdateCourse(Courses ev)
       {
           const string query = @"
   UPDATE Courses
   SET 
       Title = @Title,
       StartDate = @StartTime,
       Price = @Price,
Duration = @duration,
       Location = @Location,
       Instructor = @Instructor,
       Description = @Description
   WHERE Id = @Id";

           using (SqlConnection con = new SqlConnection(strcon))
           {
               using (SqlCommand cmd = new SqlCommand(query, con))
               {
                    DateTime startDateTime = ev.StartDate.ToDateTime(TimeOnly.MinValue);
                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@Id", ev.Id);
                   cmd.Parameters.AddWithValue("@Title", ev.Title);
                   cmd.Parameters.AddWithValue("@StartTime", startDateTime);
                   cmd.Parameters.AddWithValue("@duration", ev.Duration);
                   cmd.Parameters.AddWithValue("@Price", ev.Price);
                   cmd.Parameters.AddWithValue("@Location", ev.Location);
                   cmd.Parameters.AddWithValue("@Instructor", ev.Instructor);
                   cmd.Parameters.AddWithValue("@Description", ev.Description);

                   // Open the connection, execute the query, and close the connection
                   con.Open();
                   cmd.ExecuteNonQuery();
                   con.Close();
               }
           }
       }

        public Courses GetSpecificCourse(int iid)
        {
            Courses ev = null;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from Courses WHERE Id=@a", con);
            com.Parameters.AddWithValue("@a", iid);
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0); // ID
                string title = reader.GetString(1); // Title
                string description = reader.GetString(2);
                string instructor = reader.GetString(3); // Instructor
                int price = reader.GetInt32(4); // Price
                int duration = reader.GetInt32(5); // Capacity
                string location = reader.GetString(6); // Location
                DateTime startDateTime = reader.GetDateTime(7); // Get the DateTime value
                DateOnly startTime = DateOnly.FromDateTime(startDateTime); // Convert it to DateOnly

                ev = new Courses(id, title,description,instructor,price,duration,location,startTime);
            }
            con.Close();
            return ev;
        }
    
        public void DeleteCourse(int id)
        {
            SqlConnection sql = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("delete from courses where id=@a", sql);
            cmd.Parameters.AddWithValue("@a", id);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
        }
    
        public List<Transactions> GetTransactions()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from [Transaction]", con);
            List<Transactions> ls = new List<Transactions>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tid = reader.GetInt32(0);
                DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(1));
                double amount = reader.GetDouble(2);
                string nid = reader.GetString(3);
                int sid = reader.GetInt32(4);
                int pid = reader.GetInt32(5);
                int? fid = reader.IsDBNull(6) ? (int?)pid : reader.GetInt32(6); // If fid is null, use pid
                string status = reader.GetString(7);
                string formtype = reader.GetString(8);

                Transactions s = new Transactions(nid, tid, sid, pid, amount, date, status,formtype);

                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public List<Transactions> GetPendingTransactions()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from [Transaction] WHERE TransactionStatus ='Pending'", con);
            List<Transactions> ls = new List<Transactions>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tid = reader.GetInt32(0);
                DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(1));
                double amount = reader.GetDouble(2);
                string nid = reader.GetString(3);
                int sid = reader.GetInt32(4);
                int? pid = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                int? fid = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);

                int? result = pid ?? fid;


                string status = reader.GetString(7);
                string formtype = reader.IsDBNull(8) ? null : reader.GetString(8);

                Transactions s = new Transactions(nid, tid, sid, result, amount, date, status,formtype);

                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public List<Transactions> GetOngoingTransactions()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from [Transaction] WHERE TransactionStatus ='Ongoing'", con);
            List<Transactions> ls = new List<Transactions>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tid = reader.GetInt32(0);
                DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(1));
                double amount = reader.GetDouble(2);
                string nid = reader.GetString(3);
                int sid = reader.GetInt32(4);
                int? pid = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                int? fid = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);

                int? result = pid ?? fid; // If fid is null, use pid
                string status = reader.GetString(7);

                string formtype = reader.IsDBNull(8) ? null : reader.GetString(8);

                Transactions s = new Transactions(nid, tid, sid, result, amount, date, status,formtype);

                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public List<Transactions> GetCompletedTransactions()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand com = new SqlCommand("SELECT * from [Transaction] WHERE TransactionStatus ='Completed'", con);
            List<Transactions> ls = new List<Transactions>();
            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                int tid = reader.GetInt32(0);
                DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(1));
                double amount = reader.GetDouble(2);
                string nid = reader.GetString(3);
                int sid = reader.GetInt32(4);
                int? pid = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                int? fid = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6);

                int? result = pid ?? fid;
                string status = reader.GetString(7);

                string formtype = reader.IsDBNull(8) ? null : reader.GetString(8);
                Transactions s = new Transactions(nid, tid, sid, result, amount, date, status,formtype);

                ls.Add(s);
            }
            con.Close();
            return ls;
        }

        public void AddFormTransaction(string email, int serviceid, int price, string formtype )
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("INSERT INTO [Transaction] (ServiceID, FormType, NationalID, TransactionAmount) SELECT @ServiceID, @Formtype, a.NationalID, @TransactionAmount FROM Account a WHERE a.AccountEmail = @Email AND a.NationalID IS NOT NULL ", con);
            cmd.Parameters.AddWithValue("@ServiceID", serviceid);
            cmd.Parameters.AddWithValue("@Formtype", formtype);
            cmd.Parameters.AddWithValue("@TransactionAmount", price);
            cmd.Parameters.AddWithValue("@Email", email);


            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    
        public void ongoingTransaction(int id)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("update [Transaction] set TransactionStatus = 'Ongoing' where TransactionID = @a",con);
            cmd.Parameters.AddWithValue("@a", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close() ;
        }

        public void completeTransaction(int id)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("update [Transaction] set TransactionStatus = 'Completed' where TransactionID = @a", con);
            cmd.Parameters.AddWithValue("@a", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteTransaction(int id)
        {
            SqlConnection sql = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("delete from [Transaction] where TransactionID=@a", sql);
            cmd.Parameters.AddWithValue("@a", id);
            sql.Open();
            cmd.ExecuteNonQuery();
            sql.Close();
        }
    
        public int totalmoney()
        {
            int money;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("select sum(transactionamount) from [transaction]", con);
            con.Open();
            money = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return money;
        }
        public int totaltransaction()
        {
            int money;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("select count(*) from [transaction]", con);
            con.Open();
            money = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return money;
        }
        public int activetransaction(string email)
        {
            int money;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [transaction] t INNER JOIN [Account] a ON t.NationalID = a.NationalID WHERE (t.TransactionStatus = 'Pending' OR t.TransactionStatus = 'Ongoing') AND a.AccountEmail = @a", con);
            cmd.Parameters.AddWithValue ("@a", email);
            con.Open();
            money = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return money;
        }

        public int formtransaction(string email)
        {
            int money;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)\r\nFROM [transaction] t\r\nINNER JOIN [Account] a ON t.NationalID = a.NationalID\r\nWHERE (t.TransactionStatus = 'Pending' OR t.TransactionStatus = 'Ongoing')\r\nAND a.AccountEmail = 'hsen@gmail.com'\r\nAND t.PlanID IS NULL;\r\n", con);
            cmd.Parameters.AddWithValue("@a", email);
            con.Open();
            money = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return money;
        }

        public void whishcheck()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("UPDATE t SET t.TransactionStatus = 'Completed' FROM [Transaction] t JOIN Whish w ON t.TransactionID = w.TransactionID WHERE t.TransactionAmount >= w.Amount;", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}



