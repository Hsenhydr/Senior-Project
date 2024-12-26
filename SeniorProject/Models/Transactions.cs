using System.Numerics;

namespace SeniorProject.Models
{
    public class Transactions
    {
        public int TransactionID { get; set; }
        public string NationalID { get; set; }
        public int ServiceID { get; set; }
        public int SubmissionID { get; set; }
        public int? PlanID { get; set; }
        public double TransactionAmount { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public string Servicetitle { get; set; }
        public string PlanTitle { get; set; }
        public string FormType { get; set; }
        public string Name { get; set; }


        public Transactions(int transactionID, double transactionAmount, DateOnly transactionDate, string transactionStatus, string servicetitle, string planTitle)
        {
            TransactionID = transactionID;
            TransactionAmount = transactionAmount;
            TransactionDate = transactionDate;
            TransactionStatus = transactionStatus;
            Servicetitle = servicetitle;
            PlanTitle = planTitle;
        }
        public Transactions() { }


        //for subscription
        public Transactions(string nationalID, int transactionID,int serviceID,int? planID, double transactionAmount, DateOnly transactionDate, string transactionStatus, string formtype)
        {
            NationalID = nationalID;
            TransactionID = transactionID;
            ServiceID = serviceID;
            PlanID = planID;
            TransactionAmount = transactionAmount;
            TransactionDate = transactionDate;
            TransactionStatus = transactionStatus;
            FormType = formtype;
        }

        public Transactions(int transactionID, string formtype, DateOnly transactionDate, double transactionAmount, string transactionStatus, string name)
        {
            TransactionID = transactionID;
            TransactionAmount = transactionAmount;
            TransactionDate = transactionDate;
            TransactionStatus = transactionStatus;
            FormType = formtype;
            Name = name;
        }
    
    }
}
